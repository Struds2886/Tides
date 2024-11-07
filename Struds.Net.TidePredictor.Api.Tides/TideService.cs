namespace Struds.Net.TidePredictor.Api.Tides
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using Domain;
    using GeoJSON.Net.Feature;
    using GeoTimeZone;
    using Newtonsoft.Json;

    public class TideService : ITideService
    {
        private const string DateTimeFormatString = "yyyy-MM-dd HH:mm:ss";
        private const string StationIdKey = "Id";
        private const string StationNameKey = "Name";

        private static readonly NLog.ILogger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly HttpMessageHandler httpMessageHandler;
        private readonly IServiceMetaData serviceMetaData;

        public TideService(HttpMessageHandler httpMessageHandler, IServiceMetaData serviceMetaData)
        {
            this.httpMessageHandler = httpMessageHandler;
            this.serviceMetaData = serviceMetaData;
        }

        public async Task<IList<TideEvent>> GetTideEvents(TideStation station, DateTime start, DateTime end, bool utc = true)
        {
            return await this.GetTideEventsForStation(station, start, end, utc);
        }

        public async Task<IList<TideHeight>> GetTideHeights(TideStation station, DateTime startDateTime, DateTime endDateTime, int interval = 30,
            bool utc = true)
        {
            return await this.GetTideHeightsForStation(station, startDateTime, endDateTime, interval,
                utc);
        }

        public async Task<TideEvent> GetTideHeight(TideStation station, DateTime dateTime, bool utc = true)
        {
            return await this.GetTideHeightForStation(station, dateTime, utc);
        }

        public async Task<IList<TideStation>> GetTideStations()
        {
            var stations = new List<TideStation>();

            try
            {
                var stationDataResponse = await this.GetStations();
                var stationDataContent = await stationDataResponse.Content.ReadAsStringAsync();

                var featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(stationDataContent);
                stations.AddRange(this.ProcessStations(featureCollection.Features));
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return stations;
        }

        public async Task<TideStation> GetTideStation(string stationId)
        {
            try
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.serviceMetaData.UKHOApiKey);

                var uri = $"{this.serviceMetaData.UKHOBaseUrl}Stations/{stationId}?" + queryString;

                var response = await client.GetAsync(uri);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var stationDataContent = await response.Content.ReadAsStringAsync();

                    var feature = JsonConvert.DeserializeObject<Feature>(stationDataContent);

                    if (feature.Properties.TryGetValue(StationIdKey, out var id) &&
                        feature.Properties.TryGetValue(StationNameKey, out var name))
                    {
                        var tmpStationId = Convert.ToString(id);
                        var stationName = Convert.ToString(name);
                        var stationLocation = feature.Geometry as GeoJSON.Net.Geometry.Point;
                        var stationLatitude = stationLocation != null ? stationLocation.Coordinates.Latitude : 0.0;
                        var stationLongitude = stationLocation != null ? stationLocation.Coordinates.Longitude : 0.0;

                        if (string.IsNullOrWhiteSpace(tmpStationId) || string.IsNullOrWhiteSpace(stationName))
                        {
                            return null;
                        }

                        var station = new TideStation
                        {
                            StationIdentifier = tmpStationId,
                            Name = stationName,
                            Latitude = stationLatitude,
                            Longitude = stationLongitude,
                            IsInternational = false,
                            TimeZone = this.GetTimeZoneFor(stationLatitude, stationLongitude)
                        };

                        return station;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return null;
        }

        public IList<TideEvent> CalculateTidalRanges(IList<TideEvent> tideData)
        {
            var tidalRangeData = new List<TideEvent>();

            for (var i = 0; i < tideData.Count; i++)
            {
                try
                {
                    var previousHeight = 0.0;
                    var currentHeight = 0.0;

                    if (i > 0)
                    {
                        previousHeight = tideData[i - 1].Height;
                        currentHeight = tideData[i].Height;
                    }

                    var current = tideData[i];

                    var tideEvent = new TideEvent
                    {
                        DateTime = current.DateTime,
                        Date = current.Date,
                        EventType = current.EventType,
                        Height = current.Height,
                        Filtered = current.Filtered,
                        IsApproximateHeight = current.IsApproximateHeight,
                        IsApproximateTime = current.IsApproximateTime,
                        Range = Math.Abs(currentHeight - previousHeight)
                    };

                    tidalRangeData.Add(tideEvent);

                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }

            return tidalRangeData;
        }

        private async Task<IList<TideEvent>> GetTideEventsForStation(TideStation station, DateTime start, DateTime end, bool utc = true)
        {
            var tideEvents = new List<TideEvent>();

            try
            {
                station.TimeZone ??= this.GetTimeZoneFor(station.Latitude, station.Longitude);

                var tideDataResponse = await this.GetTidalEventData(station.StationIdentifier, start, end);

                var tideDataContent = await tideDataResponse.Content.ReadAsStringAsync();
                tideEvents.AddRange(JsonConvert.DeserializeObject<TideEvent[]>(tideDataContent));

                foreach (var tideEvent in tideEvents)
                {
                    var tideDate = DateTime.SpecifyKind(tideEvent.Date, DateTimeKind.Utc);
                    var tideTime = DateTime.SpecifyKind(tideEvent.DateTime, DateTimeKind.Utc);

                    tideEvent.Date = utc ? tideDate : TimeZoneInfo.ConvertTimeFromUtc(tideDate, station.TimeZone);
                    tideEvent.DateTime = utc ? tideTime : TimeZoneInfo.ConvertTimeFromUtc(tideTime, station.TimeZone);
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return tideEvents;
        }

        private async Task<IList<TideHeight>> GetTideHeightsForStation(TideStation station, DateTime startDateTime, DateTime endDateTime, int interval, bool utc = true)
        {
            var tideHeights = new List<TideHeight>();

            try
            {
                station.TimeZone ??= this.GetTimeZoneFor(station.Latitude, station.Longitude);

                var tideHeightResponseMessage =
                    await this.GetTideHeights(station.StationIdentifier, startDateTime, endDateTime, interval);

                var tideHeightContent = await tideHeightResponseMessage.Content.ReadAsStringAsync();

                tideHeights.AddRange(JsonConvert.DeserializeObject<TideHeight[]>(tideHeightContent));

                foreach (var tideHeight in tideHeights)
                {
                    var tideTime = DateTime.SpecifyKind(tideHeight.DateTime, DateTimeKind.Utc);
                    tideHeight.DateTime = utc ? tideTime : TimeZoneInfo.ConvertTimeFromUtc(tideTime, station.TimeZone);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return tideHeights;
        }

        private async Task<TideEvent> GetTideHeightForStation(TideStation station, DateTime dateTime, bool utc = true)
        {
            TideEvent tideEvent = null;

            try
            {
                station.TimeZone ??= this.GetTimeZoneFor(station.Latitude, station.Longitude);

                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                var startDateTime = dateTime.ToString(DateTimeFormatString);

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{this.serviceMetaData.UKHOApiKey}");

                var uri = $"{this.serviceMetaData.UKHOBaseUrl}Stations/{station.StationIdentifier}/TidalHeight?DateTime={startDateTime}&" + queryString;

                var response = await client.GetAsync(uri);

                var tideHeightContent = await response.Content.ReadAsStringAsync();

                var tideHeight = JsonConvert.DeserializeObject<TideHeight>(tideHeightContent);

                var tideTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                var tideDate = DateTime.SpecifyKind(dateTime.Date, DateTimeKind.Utc);

                tideEvent = new TideEvent
                {
                    Date = utc ? tideDate : TimeZoneInfo.ConvertTimeFromUtc(tideDate, station.TimeZone),
                    DateTime = utc ? tideTime : TimeZoneInfo.ConvertTimeFromUtc(tideTime, station.TimeZone),
                    Height = Convert.ToSingle(tideHeight.Height),
                    EventType = TideEventType.Height
                };

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return tideEvent;
        }

        private IList<TideStation> ProcessStations(IEnumerable<Feature> features)
        {
            var stations = new List<TideStation>();

            foreach (var feature in features)
            {
                try
                {
                    var stationLocation = feature.Geometry as GeoJSON.Net.Geometry.Point;
                    if (stationLocation == null)
                    {
                        continue;
                    }

                    if (feature.Properties.TryGetValue(StationIdKey, out var id) &&
                        feature.Properties.TryGetValue(StationNameKey, out var name))
                    {
                        var stationId = Convert.ToString(id);
                        var stationName = Convert.ToString(name);
                        if (string.IsNullOrWhiteSpace(stationId) || string.IsNullOrWhiteSpace(stationName))
                        {
                            continue;
                        }

                        var stationTimeZone = this.GetTimeZoneFor(stationLocation.Coordinates.Latitude,
                            stationLocation.Coordinates.Longitude);

                        stations.Add(new TideStation
                        {
                            StationIdentifier = stationId,
                            Name = stationName,
                            Latitude = stationLocation.Coordinates.Latitude,
                            Longitude = stationLocation.Coordinates.Longitude,
                            IsInternational = false,
                            TimeZone = stationTimeZone
                        });
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }

            return stations;
        }

        private TimeZoneInfo GetTimeZoneFor(double latitude, double longitude)
        {
            TimeZoneInfo tzInfo = null;
            try
            {
                var tz = TimeZoneLookup.GetTimeZone(latitude, longitude);
                tzInfo = TimeZoneConverter.TZConvert.GetTimeZoneInfo(tz.Result);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return tzInfo;
        }

        private async Task<HttpResponseMessage> GetTidalEventData(string stationId, DateTime start, DateTime end)
        {
            var client = new HttpClient(this.httpMessageHandler);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.serviceMetaData.UKHOApiKey);

            // Request parameters
            var startDate = start.ToString(DateTimeFormatString);
            var endDate = end.ToString(DateTimeFormatString);
            var uri =
                $"{this.serviceMetaData.UKHOBaseUrl}Stations/{stationId}/TidalEventsForDateRange?StartDate={startDate}&EndDate={endDate}";

            return await client.GetAsync(uri);
        }

        private async Task<HttpResponseMessage> GetStations(string stationName = null)
        {
            var client = new HttpClient(this.httpMessageHandler);
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.serviceMetaData.UKHOApiKey);

            // Request parameters
            queryString["name"] = stationName;
            var uri = $"{this.serviceMetaData.UKHOBaseUrl}Stations?" + queryString;

            return await client.GetAsync(uri);
        }

        private async Task<HttpResponseMessage> GetTideHeights(string stationId, DateTime startDateTime, DateTime endDateTime, int interval)
        {
            var client = new HttpClient(this.httpMessageHandler);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.serviceMetaData.UKHOApiKey);

            // Request parameters
            var startDate = startDateTime.ToString(DateTimeFormatString);
            var endDate = endDateTime.ToString(DateTimeFormatString);
            var uri =
                $"{this.serviceMetaData.UKHOBaseUrl}Stations/{stationId}/TidalHeights?StartDateTime={startDate}&EndDateTime={endDate}&IntervalInMinutes={interval}";

            return await client.GetAsync(uri);
        }

        public void Dispose()
        {
            // Empty by design
        }
    }
}
