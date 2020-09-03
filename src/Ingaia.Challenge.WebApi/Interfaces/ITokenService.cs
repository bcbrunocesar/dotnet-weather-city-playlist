using Ingaia.Challenge.WebApi.Models;

namespace Ingaia.Challenge.WebApi.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserModel userModel);
    }
}
