namespace Ingaia.Challenge.WebApi.Infrastructure.ApiResponses
{
    public class SuccessResponse : BaseResponse
    {
        public SuccessResponse(object data)
            : base(200)
        {
            Data = data;
        }

        public object Data { get; set; }
    }
}
