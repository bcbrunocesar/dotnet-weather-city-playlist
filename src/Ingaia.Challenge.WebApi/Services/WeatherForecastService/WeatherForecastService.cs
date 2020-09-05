using Ingaia.Challenge.WebApi.Config;
using Ingaia.Challenge.WebApi.Constants;
using Ingaia.Challenge.WebApi.Entities;
using Ingaia.Challenge.WebApi.Models.Responses;
using Ingaia.Challenge.WebApi.Repositories.CityRequestRepository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services.WeatherForecastService
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

        public async Task<CityWeatherResponse> GetByCityAsync(string cityName)
        {
            try
            {
                var cityWeatherResponse = new CityWeatherResponse();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_openWeatherMapConfig.Value.BaseEndpoint);

                    var response = await client.GetAsync($"?q={cityName}&appid={_openWeatherMapConfig.Value.Token}&units={UNIT}");
                    if (!response.IsSuccessStatusCode)
                    {                        
                        _logger.LogWarning(string.Format(LogMessagesConstant.CITY_WEATHER_NOT_FOUND, cityName));
                        return null;
                    }

                    var jsonAsString = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonAsString)["main"];
                    cityWeatherResponse = jsonObject.ToObject<CityWeatherResponse>();
                }

                return cityWeatherResponse;
            }
            catch (Exception error)
            {
                _logger.LogError(error, error.Message);
                return null;
            }
        }

        public async Task AddAsync(string cityName)
        {
            try
            {
                await _cityRepository.AddAsync(new CityRequestEntity(cityName));
            }
            catch (Exception error)
            {
                _logger.LogError(error, error.Message);
            }
        }
    }
}
