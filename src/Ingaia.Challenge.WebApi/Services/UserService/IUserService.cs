using Ingaia.Challenge.WebApi.Entities;
using Ingaia.Challenge.WebApi.Models.Commands;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services.UserService
{
    public interface IUserService
    {
        Task AddUserAsync(RegisterUserCommand command);        
        Task<UserEntity> GetAsync(string userName);
        Task<string> AuthenticateAsync(AuthenticateCommand command);
    }
}
