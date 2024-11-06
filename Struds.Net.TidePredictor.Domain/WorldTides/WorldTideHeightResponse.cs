namespace Struds.Net.TidePredictor.Domain.WorldTides
{
    using System.Net;
    using Newtonsoft.Json;

    public class WorldTideHeightResponse
    {
        [JsonProperty("status")]
        public HttpStatusCode Status { get; set; }

        [JsonProperty("callCount")]
        public int CallCount { get; set; }

        [JsonProperty("requestLat")]
        public double RequestLatitude { get; set; }

        [JsonProperty("requestLon")]
        public double RequestLongitude { get; set; }

        [JsonProperty("copyright")]
        public string Copyright { get; set; }

        [JsonProperty("responseLat")]
        public double ResponseLatitude { get; set; }

        [JsonProperty("responseLon")]
        public double ResponseLongitude { get; set; }

        [JsonProperty("atlas")]
        public string Atlas { get; set; }

        [JsonProperty("station")]
        public string Station { get; set; }

        [JsonProperty("requestDatum")]
        public string RequestDatum { get; set; }

        [JsonProperty("responseDatum")]
        public string ResponseDatum { get; set; }

        [JsonProperty("heights")]
        public WorldTideHeight[] TideHeights { get; set; }
    }
}
