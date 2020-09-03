using Ingaia.Challenge.WebApi.Constants;
using Ingaia.Challenge.WebApi.Enums;
using Ingaia.Challenge.WebApi.Interfaces;
using Ingaia.Challenge.WebApi.Models;
using Ingaia.Challenge.WebApi.Repositories.CityRequestRepository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services
{
    public class AppService : IAppService
    {
        private MemoryCacheEntryOptions _cacheExpiryOptions;

        private readonly IPlaylistService _playlistService;
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly ICityRequestRepository _cityRequestRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<AppService> _logger;

        public AppService(
            IPlaylistService playlistService,
            IWeatherForecastService weatherForecastService,
            ICityRequestRepository cityRequestRepository,
            IMemoryCache memoryCache,
            ILogger<AppService> logger)
        {
            _weatherForecastService = weatherForecastService;
            _playlistService = playlistService;
            _cityRequestRepository = cityRequestRepository;
            _memoryCache = memoryCache;
            _logger = logger;

            _cacheExpiryOptions = SetServiceCacheOptions();
        }

        public async Task<IEnumerable<CityRequestStatisticsModel>> GetRequestStatisticsAsync()
        {
            try
            {
                var citiesRequests = await _cityRequestRepository.GetAsync();
                if (!citiesRequests.Any())
                {
                    _logger.LogInformation(LogMessages.CITY_REQUEST_NOT_FOUND);
                    return default;
                }

                return citiesRequests
                    .GroupBy(g => g.CityName)
                    .Select(group => new CityRequestStatisticsModel(group.Key, group.Count()))
                    .OrderByDescending(request => request.RequestCount)
                    .ToList();
            }
            catch (Exception error)
            {
                _logger.LogError(error, error.Message);
                return default;
            }
        }

        public async Task<IEnumerable<string>> GetWeatherPlaylistAsync(string cityName)
        {
            try
            {
                if (!_memoryCache.TryGetValue(cityName, out IEnumerable<string> tracks))
                {
                    var weatherForecastNow = await _weatherForecastService.GetByCityAsync(cityName);
                    var playlistGenre = GetPlaylistGenre(weatherForecastNow.Temperature);

                    tracks = await _playlistService.GetPlaylistTracksAsync(playlistGenre.ToString());

                    SetCache(cityName, tracks, _cacheExpiryOptions);
                }

                if (tracks.Any())
                {
                    await _weatherForecastService.AddAsync(cityName);
                }

                return tracks;
            }
            catch (Exception error)
            {
                _logger.LogError(error, error.Message);
                return default;
            }
        }

        private EPlaylist GetPlaylistGenre(int temperature)
        {
            if (temperature < 10)
            {
                return EPlaylist.Classica;
            }
            else if (temperature >= 10 && temperature < 26)
            {
                return EPlaylist.Rock;
            }

            return EPlaylist.Pop;
        }

        private MemoryCacheEntryOptions SetServiceCacheOptions()
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(15),
                Priority = CacheItemPriority.High,
                Size = 1024
            };
        }

        private void SetCache(string key, object value, MemoryCacheEntryOptions options)
        {
            _memoryCache.Set(key, value, options);
        }
    }
}