using Ingaia.Challenge.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Repositories.CityRequestRepository
{
    public interface ICityRequestRepository
    {
        Task<IEnumerable<CityRequestModel>> GetAsync();
        Task AddAsync(CityRequestModel cityRequestModel);
    }
}
