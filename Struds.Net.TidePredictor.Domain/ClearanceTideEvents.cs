namespace Struds.Net.TidePredictor.Domain
{
    using System.Collections.Generic;

    public class ClearanceTideEvents
    {
        public ClearanceTideEvents(IEnumerable<TideEvent> suitableTideEvents)
        {
            this.SuitableTideEvents = new List<TideEvent>(suitableTideEvents);
        }

        public IReadOnlyList<TideEvent> SuitableTideEvents { get; set; }
    }
}