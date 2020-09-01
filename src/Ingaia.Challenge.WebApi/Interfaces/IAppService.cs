using Ingaia.Challenge.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Interfaces
{
    public interface IAppService
    {
        Task<IEnumerable<CityRequestStatisticsModel>> GetRequestStatisticsAsync();
        Task<IEnumerable<string>> GetWeatherPlaylistAsync(string cityName);
    }
}
