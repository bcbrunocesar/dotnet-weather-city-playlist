using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Interfaces
{
    public interface IAppService
    {
        Task<IEnumerable<string>> GetWeatherPlaylist(string cityName);
    }
}
