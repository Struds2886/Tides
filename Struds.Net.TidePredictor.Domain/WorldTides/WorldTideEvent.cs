namespace Struds.Net.TidePredictor.Domain.WorldTides
{
    using System;
    using Newtonsoft.Json;

    public class WorldTideEvent
    {
        [JsonProperty("dt")]
        public int SecondsSinceEpoch { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
