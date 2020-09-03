using Ingaia.Challenge.WebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Controllers
{
    [Authorize]
    [Route("api/v1/weather-playlist")]
    public class PlaylistController : ControllerBase
    {
        private readonly IAppService _appService;

        public PlaylistController(IAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]        
        public async Task<IActionResult> Get()
        {
            var citiesRequests = await _appService.GetRequestStatisticsAsync();
            return Ok(citiesRequests);
        }

        [HttpGet]
        [Route("{cityName}")]
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