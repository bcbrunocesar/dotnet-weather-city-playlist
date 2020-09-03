using Ingaia.Challenge.WebApi.Models.Commands;
using Ingaia.Challenge.WebApi.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Controllers
{
    [Route("api/v1/authenticate")]
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateCommand command)
        {
            var token = await _userService.AuthenticateAsync(command);
            return Ok(new
            {
                token
            });
        }
    }
}