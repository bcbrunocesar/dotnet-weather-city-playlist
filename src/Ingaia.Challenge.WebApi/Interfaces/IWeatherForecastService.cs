using Ingaia.Challenge.WebApi.Models;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecastModel> GetByCityAsync(string cityName);
    }
}
