namespace Struds.Net.TidePredictor.Domain.Weather
{
    using System;

    public class DailyForecast
    {
        public string Type { get; set; }

        public DateTime Date { get; set; }

        public DayTime DayTimeForecast { get; set; }

        public NightTime NightTimeForecast { get; set; }
    }
}
