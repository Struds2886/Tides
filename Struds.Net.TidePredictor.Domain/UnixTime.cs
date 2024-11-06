namespace Struds.Net.TidePredictor.Domain
{
    using System;

    public class UnixTime
    {
        /// <summary>Represents the date January 1 1970, 00:00:00 UTC.</summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public static DateTime Epoch => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a Unix Time to a System.DateTime object in
        /// universal time.
        /// </summary>
        /// <param name="value">A 64-bit integer representing the Unix Time.</param>
        /// <returns>System.DateTime object.</returns>
        public static DateTime ToUniversalDateTime(long value)
        {
            return Epoch.Add(TimeSpan.FromSeconds(value));
        }
    }
}
