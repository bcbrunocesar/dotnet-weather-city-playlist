using Ingaia.Challenge.WebApi.Config;
using Ingaia.Challenge.WebApi.Constants;
using Ingaia.Challenge.WebApi.Interfaces;
using Ingaia.Challenge.WebApi.Models;
using Ingaia.Challenge.WebApi.Repositories.CityRequestRepository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private const string UNIT = "metric";

        private readonly IOptions<WeatherForecastConfig> _openWeatherMapConfig;
        private readonly ICityRequestRepository _cityRepository;
        private readonly ILogger<WeatherForecastService> _logger;

        public WeatherForecastService(
            IOptions<WeatherForecastConfig> openWeatherMapConfig, 
            ICityRequestRepository cityRepository, 
            ILogger<WeatherForecastService> logger)
        {
            _openWeatherMapConfig = openWeatherMapConfig;
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public async Task<WeatherForecastModel> GetByCityAsync(string cityName)
        {
            try
            {
                var weatherForecastModel = new WeatherForecastModel();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_openWeatherMapConfig.Value.BaseEndpoint);

                    var response = await client.GetAsync($"?q={cityName}&appid={_openWeatherMapConfig.Value.Token}&units={UNIT}");
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning(string.Format(LogMessages.CITY_WEATHER_NOT_FOUND, cityName));
                    }

                    var jsonAsString = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonAsString)["main"];
                    weatherForecastModel = jsonObject.ToObject<WeatherForecastModel>();
                }

                return weatherForecastModel;
            }
            catch (Exception error)
            {
                _logger.LogError(error, error.Message);
                return default;
            }
        }

        public async Task AddAsync(string cityName)
        {
            try
            {
                await _cityRepository.AddAsync(new CityRequestModel(cityName));
            }
            catch (Exception error)
            {
                _logger.LogError(error, error.Message);
            }
        }
    }
}
