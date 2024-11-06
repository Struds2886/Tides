using System;
using System.Collections.Generic;
using System.Text;

namespace Struds.Net.TidePredictor.Domain.WorldTides
{
    using Newtonsoft.Json;

    public class WorldTideStation
    {
        [JsonProperty("id")]
        public string StationIdentifier { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }
}
