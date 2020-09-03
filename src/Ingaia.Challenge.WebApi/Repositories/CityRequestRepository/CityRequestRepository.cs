using Ingaia.Challenge.WebApi.Context;
using Ingaia.Challenge.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Repositories.CityRequestRepository
{
    public class CityRequestRepository : BaseRepository, ICityRequestRepository
    {
        public CityRequestRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task AddAsync(CityRequestEntity cityRequestEntity)
        {
            await _context.CitiesRequests.AddAsync(cityRequestEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CityRequestEntity>> GetAsync()
        {
            return await _context.CitiesRequests.ToListAsync();
        }
    }
}
