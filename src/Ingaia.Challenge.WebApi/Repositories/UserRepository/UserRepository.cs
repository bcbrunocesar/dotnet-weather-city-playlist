using Ingaia.Challenge.WebApi.Context;
using Ingaia.Challenge.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Repositories.UserRepository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task AddUserAsync(UserEntity userEntity)
        {
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<UserEntity> GetAsync(string userName)
        {
            var user = await _context
                .Users
                .AsNoTracking()
                .Where(x => x.Username.ToLower().Equals(userName.ToLower()))
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
