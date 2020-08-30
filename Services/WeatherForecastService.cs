using Ingaia.Challenge.WebApi.Config;
using Ingaia.Challenge.WebApi.Interfaces;
using Ingaia.Challenge.WebApi.Models;
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

        public WeatherForecastService(IOptions<WeatherForecastConfig> openWeatherMapConfig)
        {
            _openWeatherMapConfig = openWeatherMapConfig;
        }

        public async Task<WeatherForecastModel> GetByCity(string cityName)
        {
            var weatherForecastModel = new WeatherForecastModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_openWeatherMapConfig.Value.BaseEndpoint);

                var response = await client.GetAsync($"?q={cityName}&appid={_openWeatherMapConfig.Value.Token}&units={UNIT}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonAsString = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonAsString)["main"];
                    weatherForecastModel = jsonObject.ToObject<WeatherForecastModel>();
                }
            }

            return weatherForecastModel;
        }
    }
}
