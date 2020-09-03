namespace Ingaia.Challenge.WebApi.Models.Responses
{
    public class CityRequestStatisticsResponse
    {
        public CityRequestStatisticsResponse(string cityName, int requestCount)
        {
            CityName = cityName;
            RequestCount = requestCount;
        }

        public string CityName { get; private set; }
        public int RequestCount { get; private set; }
    }
}
