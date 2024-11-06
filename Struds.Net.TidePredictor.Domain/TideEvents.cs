// // ***********************************************************************
// // <copyright file=TideEvents.cs company=Struds.Net Ltd>
// //     Copyright (c) 2020 Struds.Net Ltd. All rights reserved.
// // </copyright>
// // ***********************************************************************
namespace Struds.Net.TidePredictor.Domain
{
    using System;
    using System.Collections.Generic;
    using Struds.Net.TidePredictor.Domain.Weather;

    public class TideEvents : List<TideEvent>
    {
        public TideEvents(TideStation tideStation, DateTime day, List<TideEvent> tideEvents) : base(tideEvents)
        {
            this.Station = tideStation;
            this.Day = day;
        }

        public TideStation Station { get; set; }

        public DateTime Day { get; set; }

        public DailyForecast Forecast { get; set; }   
    }
}
