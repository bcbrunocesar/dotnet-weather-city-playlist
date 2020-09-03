using Ingaia.Challenge.WebApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Repositories.CityRequestRepository
{
    public interface ICityRequestRepository
    {
        Task<IEnumerable<CityRequestEntity>> GetAsync();
        Task AddAsync(CityRequestEntity cityRequestModel);
    }
}
