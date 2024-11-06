namespace Struds.Net.TidePredictor.Domain.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TideDataStoreItem
    {
        public TideStation TideStation { get; set; }

        public IDictionary<DateTime, TideEvent> TideEvents { get; set; }

        public IEnumerable<TideEvent> GetTideEvents(DateTime startTime, DateTime endTime)
        {

            var data = this.TideEvents.Where(x => x.Key >= startTime && x.Key <= endTime);

            return data.Select(keyValuePair => keyValuePair.Value).ToList();
        }
    }
}