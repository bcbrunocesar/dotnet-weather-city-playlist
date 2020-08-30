using Ingaia.Challenge.WebApi.Enums;
using Ingaia.Challenge.WebApi.Interfaces;

namespace Ingaia.Challenge.WebApi.Services
{
    public class AppService
    {
        private readonly ISpotifyService _spotifyService;
        private readonly IWeatherForecastService _weatherForecastService;

        public AppService(ISpotifyService spotifyService, IWeatherForecastService weatherForecastService)
        {
            _weatherForecastService = weatherForecastService;
            _spotifyService = spotifyService;
        }

        public void GetPlaylistByTemperature(int temperature)
        {
        // Consumir weatherForecast
        // Consumir playlist com o retorno do weatherForecast
        // Retornar erro caso algum dos endpoints não respondam ou dê algum erro
        // Tratar reconexão com o spotify caso o token vença
        // Utilizar cache

        // https://github.com/JohnnyCrazy/SpotifyAPI-NET
        }

        private EPlaylist GetPlaylist(int temperature)
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
