using Ingaia.Challenge.WebApi.Context;
using Ingaia.Challenge.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Repositories
{
    public class CityRequestRepository : BaseRepository, ICityRequestRepository
    {
        public CityRequestRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task AddAsync(CityRequestModel cityModel)
        {
            await _context.AddAsync(cityModel);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CityRequestModel>> GetAsync()
        {
            return await _context.CitiesStatistics.ToListAsync();
        }
    }
}
