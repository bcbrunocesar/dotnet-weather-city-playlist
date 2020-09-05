using Ingaia.Challenge.WebApi.Infrastructure.ApiResponses;
using Ingaia.Challenge.WebApi.Infrastructure.Notificator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ingaia.Challenge.WebApi.Controllers
{
    [ApiController]
    public class BaseController : Controller
    {
        private readonly INotificator _notificator;

        public BaseController(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected bool IsValid() => !_notificator.HasNotification();

        protected IActionResult CustomResponse(object result = null)
        {
            if (IsValid())
            {
                return Ok(new SuccessResponse(result));
            }

            if (_notificator.HasNotification())
            {
                return BadRequest(new ErrorResponse(
                    StatusCodes.Status400BadRequest,
                    _notificator.GetMessages()));
            }

            return new ObjectResult(new ErrorResponse(
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro na sua requisição."));
        }
    }
}