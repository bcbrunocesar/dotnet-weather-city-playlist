using Ingaia.Challenge.WebApi.Enums;
using Ingaia.Challenge.WebApi.Interfaces;
using Ingaia.Challenge.WebApi.Models;
using Ingaia.Challenge.WebApi.Repositories;
using Microsoft.Extensions.Caching.Memory;
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

        public AppService(IPlaylistService playlistService, IWeatherForecastService weatherForecastService, ICityRequestRepository cityRequestRepository, IMemoryCache memoryCache)
        {
            _weatherForecastService = weatherForecastService;
            _playlistService = playlistService;
            _cityRequestRepository = cityRequestRepository;
            _memoryCache = memoryCache;

            _cacheExpiryOptions = SetServiceCacheOptions();
        }

        public async Task<IEnumerable<CityRequestStatisticsModel>> GetRequestStatisticsAsync()
        {
            var citiesRequests = await _cityRequestRepository.GetAsync();
            if (!citiesRequests.Any())
            {
                return default;
            }

            return citiesRequests
                .GroupBy(g => g.CityName)
                .Select(group => new CityRequestStatisticsModel(group.Key, group.Count()))
                .OrderByDescending(request => request.RequestCount)
                .ToList();
        }

        // TODO: Retornar erro caso algum dos endpoints não respondam ou dê algum erro

        public async Task<IEnumerable<string>> GetWeatherPlaylistAsync(string cityName)
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
