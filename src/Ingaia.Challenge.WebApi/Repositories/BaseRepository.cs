using Ingaia.Challenge.WebApi.Context;

namespace Ingaia.Challenge.WebApi.Repositories
{
    public class BaseRepository 
    {
        protected readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
