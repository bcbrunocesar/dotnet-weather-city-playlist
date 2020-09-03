using Ingaia.Challenge.WebApi.Entities;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task AddUserAsync(UserEntity userEntity);
        Task<UserEntity> GetAsync(string userName);
    }
}
