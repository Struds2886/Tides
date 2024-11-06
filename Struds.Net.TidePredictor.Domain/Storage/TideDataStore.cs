namespace Struds.Net.TidePredictor.Domain.Storage
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class TideDataStore
    {
        private readonly ConcurrentDictionary<string, TideDataStoreItem> dataStore;

        public TideDataStore()
        {
            this.dataStore = new ConcurrentDictionary<string, TideDataStoreItem>();
        }

        public IReadOnlyDictionary<string, TideDataStoreItem> Data => this.dataStore;

        public TideDataStoreItem this[string key]
        {
            get
            {
                if (this.dataStore.TryGetValue(key, out var tideDataStoreItem))
                {
                    return tideDataStoreItem;
                }

                return null;
            }
        }

        public void TryAdd(TideDataStoreItem tideDataStoreItem)
        {
            if (this.dataStore.ContainsKey(tideDataStoreItem.TideStation.StationIdentifier))
            {
                return;
            }

            this.dataStore.TryAdd(tideDataStoreItem.TideStation.StationIdentifier, tideDataStoreItem);
        }

        public void TryAdd(TideStation tideStation, IEnumerable<TideEvent> tideEvents)
        {
            if (!this.dataStore.TryGetValue(tideStation.StationIdentifier, out var tideDataStoreItem))
            {
                tideDataStoreItem = new TideDataStoreItem
                {
                    TideStation = tideStation,
                    TideEvents = new SortedList<DateTime, TideEvent>()
                };

                this.dataStore.TryAdd(tideStation.StationIdentifier, tideDataStoreItem);
            }

            foreach (var tideEvent in tideEvents)
            {
                if (!tideDataStoreItem.TideEvents.ContainsKey(tideEvent.DateTime))
                {
                    tideDataStoreItem.TideEvents.Add(tideEvent.DateTime, tideEvent);
                }
            }
        }
    }
}
