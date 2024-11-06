namespace Struds.Net.TidePredictor.Domain.Entities
{
    public class Preferences : IEntity
    {
        public int Id { get; set; }

        public double VesselDraught { get; set; }

        public double VesselSafetyFactor { get; set; }
    }
}
