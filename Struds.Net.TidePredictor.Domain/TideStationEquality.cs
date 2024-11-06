namespace Struds.Net.TidePredictor.Domain
{
    using System.Collections.Generic;

    public class TideStationEquality : IEqualityComparer<TideStation>
    {
        public bool Equals(TideStation x, TideStation y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            var equal = 
                //x.Name == y.Name &&
                //x.IsInternational == y.IsInternational &&
                //x.StationIdentifier == y.StationIdentifier &&
                x.Latitude.Equals(y.Latitude) &&
                x.Longitude.Equals(y.Longitude);

            return equal;
        }

        public int GetHashCode(TideStation obj)
        {
            var hash = 13;
            //hash = (hash * 7) + obj.Name.GetHashCode();
            //hash = (hash * 7) + obj.StationIdentifier.GetHashCode();
            //hash = (hash * 7) + obj.IsInternational.GetHashCode();
            hash = (hash * 7) + obj.Longitude.GetHashCode();
            hash = (hash * 7) + obj.Latitude.GetHashCode();

            return hash;
        }
    }
}
