using Ingaia.Challenge.WebApi.Infrastructure.ApiResponses;
using Ingaia.Challenge.WebApi.Infrastructure.Notificator;
using Ingaia.Challenge.WebApi.Services.AppService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Controllers
{
    [Authorize]
    [Route("api/v1/weather-playlist")]
    public class PlaylistController : BaseController
    {
        private readonly IAppService _appService;

        public PlaylistController(INotificator notificator, IAppService appService)
            : base(notificator)
        {
            _appService = appService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> Get()
        {
            var citiesRequests = await _appService.GetRequestStatisticsAsync();
            return CustomResponse(citiesRequests);
        }

        [HttpGet]
        [Route("{cityName}")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> Get(string cityName)
        {
            var playlist = await _appService.GetWeatherPlaylistAsync(cityName);
            return CustomResponse(playlist);
        }
    }
}