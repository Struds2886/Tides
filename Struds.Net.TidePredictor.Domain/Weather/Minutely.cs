namespace Struds.Net.TidePredictor.Domain.Weather
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    public class Minutely
    {
        [JsonProperty("dt")]
        public int Timestamp { get; set; }

        public DateTime DateTime { get; set; }

        [JsonProperty("precipitation")]
        [Description("Precipitation volume, mm")]
        public int Precipitation { get; set; }

        [OnDeserialized]
        internal void HandleOnDeSerialized(StreamingContext context)
        {
            this.DateTime = UnixTime.ToUniversalDateTime(this.Timestamp);
        }
    }
}