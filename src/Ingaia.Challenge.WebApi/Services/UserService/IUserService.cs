using Ingaia.Challenge.WebApi.Models.Commands;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services.UserService
{
    public interface IUserService
    {
        Task AddUserAsync(RegisterUserCommand command);
        Task<string> AuthenticateAsync(AuthenticateCommand command);
    }
}
