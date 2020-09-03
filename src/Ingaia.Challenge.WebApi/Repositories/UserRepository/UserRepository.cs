using Ingaia.Challenge.WebApi.Context;
using Ingaia.Challenge.WebApi.Models;
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

        public async Task AddUserAsync(UserModel userModel)
        {
            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
        }

        public UserModel GetAsync(string userName, string password)
        {
            return _context.Users.FirstOrDefault(x => x.Username.Equals(userName) && x.Password.Equals(password));
        }
    }
}
