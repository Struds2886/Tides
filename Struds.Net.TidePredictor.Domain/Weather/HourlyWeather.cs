namespace Struds.Net.TidePredictor.Domain.Weather
{
    using Newtonsoft.Json;

    public class HourlyWeather
    {
        [JsonProperty("id")]
        public int WeatherConditionId { get; set; }

        [JsonProperty("main")]
        public string WeatherParameters { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string WeatherIcon { get; set; }
    }
}