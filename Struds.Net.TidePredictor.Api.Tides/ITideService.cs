namespace Struds.Net.TidePredictor.Api.Tides
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;

    public interface ITideService : IDisposable
    {
        Task<IList<TideEvent>> GetTideEvents(TideStation station, DateTime start, DateTime end, bool utc = true);

        Task<IList<TideHeight>> GetTideHeights(TideStation station, DateTime startDateTime, DateTime endDateTime, int interval = 30, bool utc = true);

        Task<TideEvent> GetTideHeight(TideStation station, DateTime dateTime, bool utc = true);

        Task<IList<TideStation>> GetTideStations();

        Task<TideStation> GetTideStation(string stationId);

        IList<TideEvent> CalculateTidalRanges(IList<TideEvent> tideData);
    }
}