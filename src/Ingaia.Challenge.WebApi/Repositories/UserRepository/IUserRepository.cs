using Ingaia.Challenge.WebApi.Models;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task AddUserAsync(UserModel userModel);
        UserModel GetAsync(string userName, string password);
    }
}
