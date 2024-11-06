namespace Struds.Net.TidePredictor.Domain.Entities
{
    using System;
    using LiteDB;

    public class TideEventsEntity
    {
        [BsonId]
        public int Id { get; set; }

        public int StationId { get; set; }

        public TideEventType EventType { get; set; }

        public DateTime DateTime { get; set; }

        public float Height { get; set; }
    }
}