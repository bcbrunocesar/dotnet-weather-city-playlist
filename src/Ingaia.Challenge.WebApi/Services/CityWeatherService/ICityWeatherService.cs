using Ingaia.Challenge.WebApi.Models.Commands;
using Ingaia.Challenge.WebApi.Models.Responses;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services.WeatherForecastService
{
    public interface ICityWeatherService
    {
        Task AddAsync(AddCityRequestCommand command);
        Task<CityWeatherResponse> GetByCityAsync(string cityName);
    }
}
