namespace Struds.Net.TidePredictor.Api.Tides
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading.Tasks;
    using System.Web;

    using Domain;
    using Domain.Entities;

    using GeoJSON.Net.Feature;

    using GeoTimeZone;

    using Newtonsoft.Json;

    public class ObservableTideService : IObservableTideService
    {
        private const string DateTimeFormatString = "yyyy-MM-dd HH:mm:ss";
        private const string StationIdKey = "Id";
        private const string StationNameKey = "Name";

        private static readonly NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly HttpMessageHandler httpMessageHandler;
        private readonly IServiceMetaData serviceMetaData;

        public ObservableTideService(HttpMessageHandler httpMessageHandler, IServiceMetaData serviceMetaData)
        {
            this.httpMessageHandler = httpMessageHandler;
            this.serviceMetaData = serviceMetaData;
        }

        public IObservable<TideEvent> GetTideEvents(TideStation station, DateTime start, DateTime end, bool utc = true)
        {
            return Observable.Create<TideEvent>((observer, token) =>
            {
                return Task.Run(async () =>
                {
                    try
                    {
                        if (station.TimeZone == null)
                        {
                            station.TimeZone = this.GetTimeZoneFor(station.Latitude, station.Longitude);
                        }

                        var tideDataResponse = await this.GetTidalEventData(station.StationIdentifier, start, end);

                        var tideDataContent = await tideDataResponse.Content.ReadAsStringAsync();

                        JsonConvert.DeserializeObject<TideEvent[]>(tideDataContent)
                            .ToObservable()
                            .Subscribe(
                                tideEvent =>
                                {
                                    var tideDate = DateTime.SpecifyKind(tideEvent.Date, DateTimeKind.Utc);
                                    var tideTime = DateTime.SpecifyKind(tideEvent.DateTime, DateTimeKind.Utc);

                                    tideEvent.Date =
                                        utc ? tideDate : TimeZoneInfo.ConvertTimeFromUtc(tideDate, station.TimeZone);
                                    tideEvent.DateTime =
                                        utc ? tideTime : TimeZoneInfo.ConvertTimeFromUtc(tideTime, station.TimeZone);

                                    observer.OnNext(tideEvent);
                                }, onError:
                                ex => logger.Error(ex), 
                                observer.OnCompleted);
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }

                }, token);
            });
        }

        public IObservable<TideHeight> GetTideHeights(TideStation station, DateTime startDateTime, DateTime endDateTime, int interval = 30, bool utc = true)
        {
            return Observable.Create<TideHeight>((observer, token) =>
            {
                return Task.Run(async () =>
                {
                    try
                    {
                        var tideHeightResponseMessage =
                            await this.GetTideHeights(station.StationIdentifier, startDateTime, endDateTime, interval);

                        var tideHeightContent = await tideHeightResponseMessage.Content.ReadAsStringAsync();

                        JsonConvert.DeserializeObject<TideHeight[]>(tideHeightContent)
                            .ToObservable()
                            .Subscribe(
                                tideHeight =>
                                {
                                    var tideTime = DateTime.SpecifyKind(tideHeight.DateTime, DateTimeKind.Utc);
                                    tideHeight.DateTime =
                                        utc ? tideTime : TimeZoneInfo.ConvertTimeFromUtc(tideTime, station.TimeZone);

                                    observer.OnNext(tideHeight);
                                }, observer.OnCompleted);
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }

                }, token);
            });
        }

        public async Task<TideEvent> GetTideHeight(TideStation station, DateTime dateTime, bool utc = true)
        {
            return await this.GetTideHeightForStation(station, dateTime, utc);
        }

        public IObservable<TideStation> GetTideStations()
        {
            return Observable.Create<TideStation>((observer, token) =>
            {
                return Task.Run(async () =>
                {
                    try
                    {
                        var stationDataResponse = await this.GetStations();
                        var stationDataContent = await stationDataResponse.Content.ReadAsStringAsync();

                        var featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(stationDataContent);

                        foreach (var feature in featureCollection.Features)
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

                                    observer.OnNext(new TideStation
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
                                observer.OnError(e);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }

                    observer.OnCompleted();

                }, token);
            });
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
                logger.Error(e);
            }

            return null;
        }

        public IObservable<TideEvent> CalculateTidalRanges(IList<TideEvent> tideData)
        {
            var subject = new ReplaySubject<TideEvent>();

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

                    current.Range = Math.Abs(currentHeight - previousHeight);

                    /*
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
                    */

                    subject.OnNext(current);
                }
                catch (Exception e)
                {
                    subject.OnError(e);
                }
            }

            subject.OnCompleted();

            return subject;
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
                $"{this.serviceMetaData.UKHOBaseUrl}Stations/{stationId}/TidalEvents";

            return await client.GetAsync(uri);
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

        private async Task<TideEvent> GetTideHeightForStation(TideStation station, DateTime dateTime, bool utc = true)
        {
            TideEvent tideEvent = null;

            try
            {
                if (station.TimeZone == null)
                {
                    station.TimeZone = this.GetTimeZoneFor(station.Latitude, station.Longitude);
                }

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
                logger.Error(e);
            }

            return tideEvent;
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
    }
}