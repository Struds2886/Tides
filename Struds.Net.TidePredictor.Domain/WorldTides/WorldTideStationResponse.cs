namespace Struds.Net.TidePredictor.Domain.WorldTides
{
    using System.Net;
    using Newtonsoft.Json;

    public class WorldTideStationResponse
    {
        [JsonProperty("status")]
        public HttpStatusCode Status { get; set; }

        [JsonProperty("callCount")]
        public int CallCount { get; set; }

        [JsonProperty("requestLat")]
        public double RequestLatitude { get; set; }

        [JsonProperty("requestLon")]
        public double RequestLongitude { get; set; }

        [JsonProperty("stationDistance")]
        public int StationDistance { get; set; }

        [JsonProperty("stations")]
        public WorldTideStation[] Stations { get; set; }
    }
}
