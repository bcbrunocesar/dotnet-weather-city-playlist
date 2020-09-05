using Ingaia.Challenge.WebApi.Enums;
using Ingaia.Challenge.WebApi.Infrastructure.Enums;
using Ingaia.Challenge.WebApi.Infrastructure.Notificator;
using Ingaia.Challenge.WebApi.Models.Responses;
using Ingaia.Challenge.WebApi.Repositories.CityRequestRepository;
using Ingaia.Challenge.WebApi.Services.PlaylistService;
using Ingaia.Challenge.WebApi.Services.WeatherForecastService;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services.AppService
{
    public class AppService : IAppService
    {
        private readonly MemoryCacheEntryOptions _cacheExpiryOptions;

        private readonly IPlaylistService _playlistService;
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly ICityRequestRepository _cityRequestRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly INotificator _notificator;
        private readonly ILogger<AppService> _logger;

        public AppService(
            IPlaylistService playlistService,
            IWeatherForecastService weatherForecastService,
            ICityRequestRepository cityRequestRepository,
            IMemoryCache memoryCache,
            INotificator notificator,
            ILogger<AppService> logger)
        {
            _weatherForecastService = weatherForecastService;
            _playlistService = playlistService;
            _cityRequestRepository = cityRequestRepository;
            _memoryCache = memoryCache;
            _notificator = notificator;
            _logger = logger;

            _cacheExpiryOptions = SetMemoryCacheOptions();
        }

        public async Task<IEnumerable<CityRequestStatisticsResponse>> GetRequestStatisticsAsync()
        {
            try
            {
                var citiesRequests = await _cityRequestRepository.GetAsync();
                if (!citiesRequests.Any()) return default;

                return citiesRequests
                    .GroupBy(g => g.CityName)
                    .Select(group => new CityRequestStatisticsResponse(group.Key, group.Count()))
                    .OrderByDescending(request => request.RequestCount)
                    .ToList();
            }
            catch (Exception error)
            {
                _notificator.Handle(error.Message, ENotificationType.Failed);
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
                    var cityWeatherNow = await _weatherForecastService.GetByCityAsync(cityName);
                    if (cityWeatherNow is null) 
                    {
                        _notificator.Handle("Cidade não encontrada");
                        _logger.LogInformation("Cidade {cityName} não encontrada.", cityName);

                        return default;
                    }

                    var playlistGenre = GetPlaylistGenre(cityWeatherNow.Temperature);
                    tracks = await _playlistService.GetPlaylistTracksAsync(playlistGenre.ToString());

                    SetMemoryCache(cityName, tracks, _cacheExpiryOptions);
                }

                if (tracks.Any())
                {
                    await _weatherForecastService.AddAsync(cityName);
                }

                return tracks;
            }
            catch (Exception error)
            {
                _notificator.Handle(error.Message, ENotificationType.Failed);
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

        private MemoryCacheEntryOptions SetMemoryCacheOptions()
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(15),
                Priority = CacheItemPriority.High,
                Size = 1024
            };
        }

        private void SetMemoryCache(string key, object value, MemoryCacheEntryOptions options)
        {
            _memoryCache.Set(key, value, options);
        }
    }
}