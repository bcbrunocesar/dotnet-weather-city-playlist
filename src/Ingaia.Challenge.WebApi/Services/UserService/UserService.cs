using Ingaia.Challenge.WebApi.Config;
using Ingaia.Challenge.WebApi.Constants;
using Ingaia.Challenge.WebApi.Constants.Logs;
using Ingaia.Challenge.WebApi.Entities;
using Ingaia.Challenge.WebApi.Infrastructure.Enums;
using Ingaia.Challenge.WebApi.Infrastructure.Notificator;
using Ingaia.Challenge.WebApi.Models.Commands;
using Ingaia.Challenge.WebApi.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
        private readonly IUserRepository _userRepository;
        private readonly INotificator _notificator;
        private readonly IOptions<AuthConfig> _authConfig;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            INotificator notificator,
            IOptions<AuthConfig> authConfig,
            ILogger<UserService> logger)
        {
            _authConfig = authConfig;
            _userRepository = userRepository;
            _notificator = notificator;
            _logger = logger;
        }

        public async Task AddUserAsync(RegisterUserCommand command)
        {
            var user = new UserEntity(command.Fullname, command.Username);
            user.SetPasswordHashed(HashPassword(user, command.Password));

            if (!user.IsValid())
            {
                _logger.LogInformation(UserLogConstants.ADD_USER_ERROR);
                return;
            }

            await _userRepository.AddUserAsync(user);

            _notificator.Handle(UserConstants.ADD_SUCCESS, ENotificationType.Success);
        }

        public async Task<string> AuthenticateAsync(AuthenticateCommand command)
        {
            var user = await _userRepository.GetAsync(command.Username);
            if (user is null)
            {
                _notificator.Handle(UserConstants.NOT_FOUND, ENotificationType.NotFound);
                _logger.LogInformation(UserLogConstants.NOT_FOUND);

                return default;
            }

            if (!VerifiyUserPassword(user, command.Password))
            {
                _notificator.Handle(UserConstants.USER_OR_PASSWORD_INVALID, ENotificationType.Business);
                _logger.LogInformation(UserLogConstants.USER_OR_PASSWORD_INVALID);

                return default;
            }

            return GenerateToken(user);
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
