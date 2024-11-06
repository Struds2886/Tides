namespace Struds.Net.TidePredictor.Domain.Weather
{
    using Newtonsoft.Json;

    public class Temperature
    {
        [JsonProperty("day")]
        public float DayTemperature { get; set; }

        [JsonProperty("min")]
        public float MinimumDailyTemperature { get; set; }

        [JsonProperty("max")]
        public float MaximumDailyTemperature { get; set; }

        [JsonProperty("night")]
        public float NightTemperarture { get; set; }

        [JsonProperty("eve")]
        public float EveningTemperature { get; set; }

        [JsonProperty("morn")]
        public float MorningTemperature { get; set; }
    }
}