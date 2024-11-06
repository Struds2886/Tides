namespace Struds.Net.TidePredictor.Domain.Weather
{
    using System;

    public class AreaForecast
    {
        public DateTime Issued { get; set; }

        public ForecastArea Area { get; set; }

        public string Synopsis { get; set; }

        public ForecastInfo TwentFourHourForecast { get; set; }

        public ForecastInfo FollowingTwentFourHourForecast { get; set; }

        public GaleWarning GaleWarning { get; set; }
    }
}