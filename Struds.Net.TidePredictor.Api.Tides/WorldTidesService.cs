namespace Struds.Net.TidePredictor.Api.Tides
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Domain;
    using Domain.WorldTides;
    using GeoTimeZone;
    using Newtonsoft.Json;

    public class WorldTidesService
    {
        private static readonly NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly HttpMessageHandler httpMessageHandler;
        private readonly IServiceMetaData serviceMetaData;

        public WorldTidesService(HttpMessageHandler httpMessageHandler, IServiceMetaData serviceMetaData)
        {
            this.httpMessageHandler = httpMessageHandler;
            this.serviceMetaData = serviceMetaData;
        }

        public async Task<IList<TideStation>> GetWorldTidesStations()
        {
            const string latLongKey = "latlon=";
            const string endPositionLookupKey = "\">";
            const string endOfLineLookupKey = "</a></div>";
            const char newline = '\n';
            const char comma = ',';

            var stations = new List<TideStation>();

            var siteUri = new Uri("https://www.worldtides.info/tidestations");

            var wr = WebRequest.Create(siteUri);

            using (var response = await wr.GetResponseAsync())
            {
                if (response == null)
                {
                    return stations;
                }

                var responseStream = response.GetResponseStream();

                if (responseStream == null)
                {
                    return stations;
                }

                using (var reader = new StreamReader(responseStream))
                {
                    var responseString = await reader.ReadToEndAsync();

                    var lines = responseString.Split(new[] { newline }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var dataLine in lines)
                    {
                        if (!dataLine.Contains($"?{latLongKey}"))
                        {
                            continue;
                        }

                        var startpos = dataLine.IndexOf(latLongKey, StringComparison.CurrentCulture) + latLongKey.Length;
                        var endPos = dataLine.LastIndexOf(endPositionLookupKey, StringComparison.CurrentCulture);

                        var latlongString = dataLine.Substring(startpos, endPos - startpos);
                        var nameString = dataLine.Substring(endPos + 2).Replace(endOfLineLookupKey, string.Empty);

                        var latLongBits = latlongString.Split(new[] { comma }, StringSplitOptions.RemoveEmptyEntries);

                        var latitude = Convert.ToDouble(latLongBits[0]);
                        var longitude = Convert.ToDouble(latLongBits[1]);

                        var tideStation = new TideStation
                        {
                            Name = nameString,
                            Latitude = latitude,
                            Longitude = longitude,
                            IsInternational = true,
                            TimeZone = this.GetTimeZoneFor(latitude, longitude)
                        };

                        stations.Add(tideStation);
                    }
                }
            }

            return stations;
        }

        public async Task<IList<TideStation>> GetTideStations(double latitude, double longitude, int radiusFromCentre = 100)
        {
            return await this.GetTideStationsForLocation(latitude, longitude, radiusFromCentre);
        }

        private async Task<IList<TideEvent>> GetTideEventsForLocation(TideStation station, DateTime startDateTime, int days, bool utc = true)
        {
            var data = await this.GetTideEventsNearMe(station.Latitude, station.Longitude, startDateTime, days);

            if (station.TimeZone == null)
            {
                station.TimeZone = this.GetTimeZoneFor(station.Latitude, station.Longitude);
            }

            var tideEvents = new List<TideEvent>();

            if (data.TideEvents != null)
            {
                foreach (var worldTideEvent in data.TideEvents)
                {
                    var tideTime = new DateTimeWithZone(worldTideEvent.Date, station.TimeZone);

                    var tideEvent = new TideEvent
                    {
                        DateTime = utc ? tideTime.UniversalTime : tideTime.LocalTime,
                        Date = utc ? tideTime.UniversalTime.Date.Date : tideTime.LocalTime.Date.Date,
                        Height = (float)worldTideEvent.Height,
                        EventType = worldTideEvent.Type == WellKnownConstants.WorldTidesLowWater ? TideEventType.Low : TideEventType.High
                    };

                    tideEvents.Add(tideEvent);
                }
            }

            return tideEvents;
        }

        private async Task<WorldTideEventsResponse> GetTideEventsNearMe(double latitude, double longitude, DateTime startDateTime, int days)
        {
            var url =
                $"{this.serviceMetaData.WorldTidesBaseUrl}?extremes&date={startDateTime:yyyy-MM-dd}&lat={latitude}&lon={longitude}&days={days}&key={this.serviceMetaData.WorldTidesApiKey}&datum=CD";

            var client = new HttpClient(this.httpMessageHandler);

            var response = await client.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<WorldTideEventsResponse>(responseContent);
        }

        private async Task<IList<TideHeight>> GetTideHeightsForLocation(TideStation station, DateTime startDateTime, int days, int interval, bool utc = true)
        {
            if (station.TimeZone == null)
            {
                station.TimeZone = this.GetTimeZoneFor(station.Latitude, station.Longitude);
            }

            var data = await this.GetTideHeightsNearMe(station.Latitude, station.Longitude, startDateTime, days, interval);

            var tideHeights = new List<TideHeight>();

            if (data.TideHeights != null)
            {
                foreach (var worldTideHeight in data.TideHeights)
                {
                    try
                    {
                        var tideTime = new DateTimeWithZone(worldTideHeight.Date, station.TimeZone);

                        var tideHeight = new TideHeight
                        {
                            DateTime = utc ? tideTime.UniversalTime : tideTime.LocalTime,
                            Height = worldTideHeight.Height
                        };

                        tideHeights.Add(tideHeight);
                    }
                    catch (Exception e)
                    {
                        logger.Error(e);
                    }
                }
            }

            return tideHeights;
        }

        private async Task<WorldTideStationResponse> GetTideStationsNearMe(double latitude, double longitude, int radiusFromCentre = 100)
        {
            if (radiusFromCentre > 100)
            {
                radiusFromCentre = 100;
            }

            var url = $"{this.serviceMetaData.WorldTidesBaseUrl}?stations&lat={latitude}&lon={longitude}&stationDistance={radiusFromCentre}&key={this.serviceMetaData.WorldTidesApiKey}";

            var client = new HttpClient(this.httpMessageHandler);

            var response = await client.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<WorldTideStationResponse>(responseContent);
        }

        private async Task<WorldTideHeightResponse> GetTideHeightsNearMe(double latitude, double longitude, DateTime startDateTime, int days, int interval)
        {
            var url =
                $"{this.serviceMetaData.WorldTidesBaseUrl}?heights&date={startDateTime:yyyy-MM-dd}&lat={latitude}&lon={longitude}&days={days}&key={this.serviceMetaData.WorldTidesApiKey}&datum=CD&Step={interval}";

            var client = new HttpClient(this.httpMessageHandler);

            var response = await client.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<WorldTideHeightResponse>(responseContent);
        }

        private async Task<IList<TideStation>> GetTideStationsForLocation(double latitude, double longitude, int radiusFromCentre = 100)
        {
            var data = await this.GetTideStationsNearMe(latitude, longitude, radiusFromCentre);

            var tideStations = new List<TideStation>();

            foreach (var worldTideStation in data.Stations)
            {
                var tideStation = new TideStation
                {
                    Name = worldTideStation.Name,
                    Longitude = worldTideStation.Longitude,
                    Latitude = worldTideStation.Latitude,
                    StationIdentifier = worldTideStation.StationIdentifier,
                    IsInternational = true,
                    TimeZone = this.GetTimeZoneFor(worldTideStation.Latitude, worldTideStation.Longitude)
                };

                tideStations.Add(tideStation);
            }

            return tideStations;
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
                logger.Error(e);
            }

            return tzInfo;
        }
    }
}
