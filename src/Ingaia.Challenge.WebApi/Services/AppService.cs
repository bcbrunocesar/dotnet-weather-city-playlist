using Ingaia.Challenge.WebApi.Enums;
using Ingaia.Challenge.WebApi.Interfaces;
using Ingaia.Challenge.WebApi.Models;
using Ingaia.Challenge.WebApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services
{
    public class AppService : IAppService
    {
        private readonly IPlaylistService _playlistService;
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly ICityRequestRepository _cityRequestRepository;

        public AppService(IPlaylistService playlistService, IWeatherForecastService weatherForecastService, ICityRequestRepository cityRequestRepository)
        {
            _weatherForecastService = weatherForecastService;
            _playlistService = playlistService;
            _cityRequestRepository = cityRequestRepository;
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

        public async Task<IEnumerable<string>> GetWeatherPlaylistAsync(string cityName)
        {
            var weatherForecastNow = await _weatherForecastService.GetByCityAsync(cityName);
            var playlistGenre = GetPlaylistGenre(weatherForecastNow.Temperature);

            var tracks = await _playlistService.GetPlaylistTracksAsync(playlistGenre.ToString());
            return tracks;

            // TODO: Retornar erro caso algum dos endpoints não respondam ou dê algum erro
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
    }
}
