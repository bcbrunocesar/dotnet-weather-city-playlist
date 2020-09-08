using Ingaia.Challenge.WebApi.Config;
using Ingaia.Challenge.WebApi.Constants.Logs;
using Ingaia.Challenge.WebApi.Constants.Validations;
using Ingaia.Challenge.WebApi.Entities;
using Ingaia.Challenge.WebApi.Infrastructure.Notificator;
using Ingaia.Challenge.WebApi.Models.Commands;
using Ingaia.Challenge.WebApi.Models.Responses;
using Ingaia.Challenge.WebApi.Repositories.CityRequestRepository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services.WeatherForecastService
{
    public class CityWeatherService : ICityWeatherService
    {
        private const string UNIT = "metric";

        private readonly ICityRequestRepository _cityRepository;
        private readonly IOptions<WeatherForecastConfig> _openWeatherMapConfig;
        private readonly ILogger<CityWeatherService> _logger;
        private readonly INotificator _notificator;

        public CityWeatherService(
            ICityRequestRepository cityRepository,
            INotificator notificator,
            IOptions<WeatherForecastConfig> openWeatherMapConfig,
            ILogger<CityWeatherService> logger)
        {
            _openWeatherMapConfig = openWeatherMapConfig;
            _cityRepository = cityRepository;
            _logger = logger;
            _notificator = notificator;
        }

        public CityWeatherResponse GetByCity(string cityName)
        {
            try
            {
                var cityWeatherResponse = new CityWeatherResponse();
                var client = new RestClient(_openWeatherMapConfig.Value.BaseEndpoint);
                var request = new RestRequest("")
                    .AddParameter("q", cityName)
                    .AddParameter("appid", _openWeatherMapConfig.Value.Token)
                    .AddParameter("units", UNIT);

                var response = client.Get(request);
                if (!response.IsSuccessful)
                {
                    _notificator.Handle(CityWeatherConstants.CITY_NOT_FOUND);
                    _logger.LogWarning(CityWeatherLogConstants.CITY_NOT_FOUND, cityName);

                    return null;
                }

                var jsonAsString = response.Content;
                var jsonObject = JObject.Parse(jsonAsString)["main"];
                cityWeatherResponse = jsonObject.ToObject<CityWeatherResponse>();

                return cityWeatherResponse;
            }        
            catch (Exception error)
            {
                _notificator.Handle(CityWeatherConstants.WEATHER_CLIENT_ERROR);
                _logger.LogError(error, error.Message);

                return null;
            }
        }

        public async Task AddAsync(AddCityRequestCommand command)
        {
            try
            {
                var cityRequestEntity = new CityRequestEntity(command.CityName);
                if (!cityRequestEntity.IsValid())
                {
                    _logger.LogInformation(CityWeatherLogConstants.ADD_CITY_WEATHER_ERROR, cityRequestEntity.CityName);
                    return;
                }

                await _cityRepository.AddAsync(cityRequestEntity);
            }
            catch (Exception error)
            {
                _logger.LogError(error, error.Message);
            }
        }
    }
}
