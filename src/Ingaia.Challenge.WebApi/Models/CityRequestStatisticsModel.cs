namespace Ingaia.Challenge.WebApi.Models
{
    public class CityRequestStatisticsModel
    {
        public CityRequestStatisticsModel(string cityName, int requestCount)
        {
            CityName = cityName;
            RequestCount = requestCount;
        }

        public string CityName { get; private set; }
        public int RequestCount { get; private set; }
    }
}
