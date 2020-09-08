using Ingaia.Challenge.WebApi.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services.AppService
{
    public interface IWeatherPlaylistService
    {
        Task<IEnumerable<CityRequestStatisticsResponse>> GetRequestStatisticsAsync();
        Task<IEnumerable<string>> GetWeatherPlaylistAsync(string cityName);
    }
}
