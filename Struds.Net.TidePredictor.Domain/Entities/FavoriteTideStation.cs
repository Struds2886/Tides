namespace Struds.Net.TidePredictor.Domain.Entities
{
    public class FavoriteTideStation : IEntity
    {
        public int Id { get; set; }

        public string StationIdentifier { get; set; }

        public string StationName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsInternational { get; set; }
    }
}
