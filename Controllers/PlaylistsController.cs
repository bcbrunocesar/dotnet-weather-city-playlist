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
        public async Task<IActionResult> Get(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                return BadRequest();
            }

            var playlist = await _appService.GetWeatherPlaylist(cityName);
            return Ok(playlist);
        }
    }
}