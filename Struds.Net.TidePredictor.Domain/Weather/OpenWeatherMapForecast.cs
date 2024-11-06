namespace Struds.Net.TidePredictor.Domain.Weather
{
    using System.ComponentModel;
    using Newtonsoft.Json;

    public class OpenWeatherMapForecast
    {
        [JsonProperty("lat")]
        public float Latitude { get; set; }

        [JsonProperty("lon")]
        public float Longitude { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("timezone_offset")]
        [Description("Shift in seconds from UTC")]
        public int TimezoneOffset { get; set; }

        [JsonProperty("current")]
        public Current Current { get; set; }

        [JsonProperty("minutely")]
        public Minutely[] MinutelyForecast { get; set; }

        [JsonProperty("hourly")]
        public Hourly[] HourlyForecast { get; set; }

        [JsonProperty("daily")]
        public Daily[] DailyForecast { get; set; }
    }
}