using Ingaia.Challenge.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Repositories
{
    public interface ICityStatisticRepository
    {
        Task<IEnumerable<CityStatisticModel>> GetAsync();
        Task AddAsync(CityStatisticModel cityModel);
    }
}
