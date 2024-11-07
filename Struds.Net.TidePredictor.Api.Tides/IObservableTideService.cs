namespace Struds.Net.TidePredictor.Api.Tides
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Domain;

    public interface IObservableTideService
    {
        IObservable<TideEvent> GetTideEvents(TideStation station, DateTime start, DateTime end, bool utc = true);

        IObservable<TideHeight> GetTideHeights(TideStation station, DateTime startDateTime, DateTime endDateTime, int interval = 30, bool utc = true);

        Task<TideEvent> GetTideHeight(TideStation station, DateTime dateTime, bool utc = true);

        IObservable<TideStation> GetTideStations();

        Task<TideStation> GetTideStation(string stationId);

        IObservable<TideEvent> CalculateTidalRanges(IList<TideEvent> tideData);
    }
}
