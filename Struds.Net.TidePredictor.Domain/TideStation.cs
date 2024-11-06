namespace Struds.Net.TidePredictor.Domain
{
    using System;

    public class TideStation
    {
        public string StationIdentifier { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsInternational { get; set; } = false;

        public TimeZoneInfo TimeZone { get; set; }
    }
}
