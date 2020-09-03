using Ingaia.Challenge.WebApi.Models;
using Ingaia.Challenge.WebApi.Repositories.UserRepository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Controllers
{
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Index([FromBody] RegisterUserModel registerUserModel)
        {
            if (registerUserModel == null)
            {
                return BadRequest();
            }

            var userModel = new UserModel(registerUserModel.Username, registerUserModel.Password);
            await _userRepository.AddUserAsync(userModel);

            return Ok(new
            {
                message = "Usuário inserido!"
            });
        }
    }
}