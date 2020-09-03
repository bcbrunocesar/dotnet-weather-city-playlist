using Ingaia.Challenge.WebApi.Config;
using Ingaia.Challenge.WebApi.Interfaces;
using Ingaia.Challenge.WebApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ingaia.Challenge.WebApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<AuthConfig> _authConfig;

        public TokenService(IOptions<AuthConfig> authConfig)
        {
            _authConfig = authConfig;
        }

        public string GenerateToken(UserModel userModel)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfig.Value.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userModel.Username.ToString()),
                    new Claim(ClaimTypes.Role, userModel.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
