namespace Struds.Net.TidePredictor.Domain.Entities
{
    using System;

    public static class Extensions
    {
        public static TideEventsEntity ToTideEventsEntity(this TideEvent tideEvent, int stationId)
        {
            return new TideEventsEntity
            {
                StationId = stationId,
                DateTime = tideEvent.DateTime,
                EventType = tideEvent.EventType,
                Height = tideEvent.Height,
            };
        }

        public static TideStationEntity ToTideStationEntity(this TideStation station)
        {
            return new TideStationEntity
            {
                Latitude = station.Latitude,
                Longitude = station.Longitude,
                Name = station.Name,
                StationIdentifier = station.StationIdentifier,
                IsInternational = station.IsInternational
            };
        }

        public static TideStationEntity ToTideStationEntity(this TideStation station, int id)
        {
            return new TideStationEntity
            {
                Id = id,
                Latitude = station.Latitude,
                Longitude = station.Longitude,
                Name = station.Name,
                StationIdentifier = station.StationIdentifier
            };
        }

        public static TideHeightEntity ToTideHeightEntity(this TideHeight tideHeight, short stationId)
        {
            return new TideHeightEntity
            {
                DateTime = tideHeight.DateTime,
                Height = tideHeight.Height,
                StationId = stationId
            };
        }

        public static TideEventsEntity ToTideEventsEntity(this TideHeight tideHeight, short stationId)
        {
            var tideEvent = new TideEventsEntity
            {
                StationId = stationId,
                DateTime = tideHeight.DateTime,
                Height = Convert.ToSingle(tideHeight.Height),
                EventType = TideEventType.Height
            };

            return tideEvent;
        }

        public static TideEvent ToTideEvent(this TideHeight tideHeight)
        {
            return new TideEvent
            {
                Date = tideHeight.DateTime.Date,
                DateTime = tideHeight.DateTime,
                EventType = TideEventType.Height,
                Height = Convert.ToSingle(tideHeight.Height)
            };
        }

        public static TideEventType ToTideEventType(this string eventType)
        {
            switch (eventType)
            {
                case WellKnownConstants.HighWater:
                    return TideEventType.High;
                case WellKnownConstants.LowWater:
                    return TideEventType.Low;
                default:
                    return TideEventType.Height;
            }
        }

        public static string FromTideEventType(this TideEventType tideEventType)
        {
            switch (tideEventType)
            {
                case TideEventType.High:
                    return WellKnownConstants.HighWater;
                case TideEventType.Low:
                    return WellKnownConstants.LowWater;
                default:
                    return WellKnownConstants.TideHeight;
            }
        }
    }
}
