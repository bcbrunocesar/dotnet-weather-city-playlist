namespace Ingaia.Challenge.WebApi.Infrastructure.ApiResponses
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse(int statusCode, object errors)
            : base(statusCode)
        {
            Errors = errors;
        }

        public object Errors { get; set; }
    }
}
