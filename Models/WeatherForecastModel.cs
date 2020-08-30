using Newtonsoft.Json;

namespace Ingaia.Challenge.WebApi.Models
{
    public class WeatherForecastModel
    {
        [JsonProperty("temp")]
        public int Temperature { get; set; }

        [JsonProperty("feels_like")]
        public double FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public int TempMin { get; set; }

        [JsonProperty("temp_max")]
        public int TempMax { get; set; }

        [JsonProperty("pressure")]
        public int Pressure { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }
    }
}
