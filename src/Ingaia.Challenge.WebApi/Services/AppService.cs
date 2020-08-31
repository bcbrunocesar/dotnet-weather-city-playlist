using Ingaia.Challenge.WebApi.Enums;
using Ingaia.Challenge.WebApi.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services
{
    public class AppService : IAppService
    {
        private readonly IPlaylistService _playlistService;
        private readonly IWeatherForecastService _weatherForecastService;

        public AppService(IPlaylistService playlistService, IWeatherForecastService weatherForecastService)
        {
            _weatherForecastService = weatherForecastService;
            _playlistService = playlistService;
        }

        public async Task<IEnumerable<string>> GetWeatherPlaylist(string cityName)
        {
            var weatherForecastNow = await _weatherForecastService.GetByCity(cityName);
            var playlistGenre = GetPlaylistGenre(weatherForecastNow.Temperature);

            var tracks = await _playlistService.GetPlaylistTracks(playlistGenre.ToString());
            return tracks;

            // Retornar erro caso algum dos endpoints não respondam ou dê algum erro
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
