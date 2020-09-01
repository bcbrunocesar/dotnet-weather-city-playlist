using Ingaia.Challenge.WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Controllers
{    
    public class PlaylistsController : ControllerBase
    {
        private readonly IAppService _appService;

        public PlaylistsController(IAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        [Route("api/v1/weather-playlist")]
        public async Task<IActionResult> Get()
        {
            var citiesRequests = await _appService.GetRequestStatisticsAsync();
            return Ok(citiesRequests);
        }

        [HttpGet]
        [Route("api/v1/weather-playlist/{cityName}")]
        public async Task<IActionResult> Get(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                return BadRequest();
            }

            var playlist = await _appService.GetWeatherPlaylistAsync(cityName);
            return Ok(playlist);
        }
    }
}