namespace Ingaia.Challenge.WebApi.Infrastructure.ApiResponses
{
    public class SuccessResponse : BaseResponse
    {
        public SuccessResponse(object data, string message)
            : base(200)
        {
            Data = data;
            Message = message;
        }
        
        public object Data { get; set; }        
        public string Message { get; set; }
    }
}
