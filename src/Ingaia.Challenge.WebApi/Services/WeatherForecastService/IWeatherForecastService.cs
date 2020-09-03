using Ingaia.Challenge.WebApi.Models.Responses;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services.WeatherForecastService
{
    public interface IWeatherForecastService
    {
        Task AddAsync(string cityName);
        Task<CityWeatherResponse> GetByCityAsync(string cityName);
    }
}
