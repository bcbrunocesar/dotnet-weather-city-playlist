namespace Ingaia.Challenge.WebApi.Infrastructure.ApiResponses
{
    public abstract class BaseResponse
    {
        public BaseResponse(int statusCode)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
    }
}
