namespace Struds.Net.TidePredictor.Domain.Weather
{
    using System;
    using System.Collections.Generic;

    public class MetOfficeMarineForecast
    {
        public DateTime Issued { get; set; }

        public DateTime ValidUntil { get; set; }

        public string GeneralSynopsis { get; set; }

        public List<ForecastArea> AreaForecasts { get; set; }
    }
}