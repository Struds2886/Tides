namespace Struds.Net.TidePredictor.Domain
{
    using System;

    using Entities;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class TideEvent
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public TideEventType EventType { get; set; }

        public DateTime DateTime { get; set; }

        public bool IsApproximateTime { get; set; }

        public float Height { get; set; }

        public bool IsApproximateHeight { get; set; }

        public bool Filtered { get; set; }

        public DateTime Date { get; set; }

        public double Range { get; set; }
    }
}
