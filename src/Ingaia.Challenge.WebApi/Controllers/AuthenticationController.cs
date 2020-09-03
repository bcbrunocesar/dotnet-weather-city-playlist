using Ingaia.Challenge.WebApi.Interfaces;
using Ingaia.Challenge.WebApi.Models;
using Ingaia.Challenge.WebApi.Repositories.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ingaia.Challenge.WebApi.Controllers
{
    [Route("api/v1/authenticate")]
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthenticateModel authenticateModel)
        {
            var user = _userRepository.GetAsync(authenticateModel.Username, authenticateModel.Password);
            if (user == null)
            {
                return NotFound(new
                {
                    message = "User or password invalid"
                });
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new
            {
                token
            });
        }
    }
}