
using Ingaia.Challenge.WebApi.Context;
using Ingaia.Challenge.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Repositories
{
    public class CityStatisticRepository : BaseRepository, ICityStatisticRepository
    {
        public CityStatisticRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task AddAsync(CityStatisticModel cityModel)
        {
            await _context.AddAsync(cityModel);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CityStatisticModel>> GetAsync()
        {
            return await _context.CitiesStatistics.ToListAsync();
        }
    }
}
