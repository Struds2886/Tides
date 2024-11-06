namespace Struds.Net.TidePredictor.Domain.Weather
{
    using System;

    public class ForecastArea : IEquatable<ForecastArea>
    {
        public ForecastArea(string areaId, string areaName)
        {
            this.AreaId = areaId;
            this.AreaName = areaName;
        }

        public string AreaId { get; }

        public string AreaName { get; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public AreaForecast Forecast { get; set; }

        public bool Equals(ForecastArea other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.AreaId == other.AreaId && this.AreaName == other.AreaName;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((ForecastArea)obj);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + this.AreaId.GetHashCode();
            hash = (hash * 7) + this.AreaName.GetHashCode();
            return hash;
        }
    }
}