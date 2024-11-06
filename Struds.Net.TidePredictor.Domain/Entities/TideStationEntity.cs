namespace Struds.Net.TidePredictor.Domain.Entities
{
    using System.Collections.Generic;

    using LiteDB;

    public class TideStationEntity : IEntity
    {
        [BsonId]
        public int Id { get; set; }

        public string StationIdentifier { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsInternational { get; set; } = false;
    }
}