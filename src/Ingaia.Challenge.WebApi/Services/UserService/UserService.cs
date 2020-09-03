using Ingaia.Challenge.WebApi.Config;
using Ingaia.Challenge.WebApi.Entities;
using Ingaia.Challenge.WebApi.Models.Commands;
using Ingaia.Challenge.WebApi.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IOptions<AuthConfig> _authConfig;
        private readonly IUserRepository _userRepository;

        public UserService(IOptions<AuthConfig> authConfig, IUserRepository userRepository)
        {
            _authConfig = authConfig;
            _userRepository = userRepository;
        }

        public async Task AddUserAsync(RegisterUserCommand command)
        {
            var user = new UserEntity(command.Username);
            user.Password = HashPassword(user, command.Password);

            await _userRepository.AddUserAsync(user);
        }

        public async Task<string> AuthenticateAsync(AuthenticateCommand command)
        {
            var user = await _userRepository.GetAsync(command.Username);
            if (user == null)
            {
                // User not found
            }

            if (!VerifiyUserPassword(user, command.Password))
            {
                // User or password invalid.
            }

            var token = GenerateToken(user);
            return token;
        }

        public Task<UserEntity> GetAsync(string userName)
        {
            return _userRepository.GetAsync(userName);
        }

        private string GenerateToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfig.Value.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool VerifiyUserPassword(UserEntity user, string password)
        {
            var hashedPassword = HashPassword(user, password);

            var passwordHasher = new PasswordHasher<UserEntity>();
            var isPasswordCorrect = passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
            return isPasswordCorrect == PasswordVerificationResult.Success;
        }

        private string HashPassword(UserEntity user, string password)
        {
            var passwordHasher = new PasswordHasher<UserEntity>();
            return passwordHasher.HashPassword(user, password);
        }
    }
}
