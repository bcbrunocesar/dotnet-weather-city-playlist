using Ingaia.Challenge.WebApi.Infrastructure.ApiResponses;
using Ingaia.Challenge.WebApi.Infrastructure.Notificator;
using Ingaia.Challenge.WebApi.Models.Commands;
using Ingaia.Challenge.WebApi.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Controllers
{
    [Route("api/v1/users")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(INotificator notificator, IUserService userService)
            : base(notificator)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> AddUser([FromBody] RegisterUserCommand command)
        {
            await _userService.AddUserAsync(command);
            return CustomResponse();
        }
    }
}