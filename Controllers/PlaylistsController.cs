using Ingaia.Challenge.WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Controllers
{
    [Route("api/v1/weather-playlist")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IAppService _appService;

        public PlaylistsController(IAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        [Route("v1/weather-forecast")]
        public async Task<IActionResult> Get(string cityName)
        {
            return Ok();
        }
    }
}