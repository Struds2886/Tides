namespace Struds.Net.TidePredictor.Domain.WorldTides
{
    using System;
    using Newtonsoft.Json;

    public class WorldTideHeight
    {
        [JsonProperty("dt")]
        public int SecondsSinceEpoch { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }
    }
}
