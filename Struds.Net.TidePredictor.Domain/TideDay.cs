namespace Struds.Net.TidePredictor.Domain
{
    using System;
    using System.Collections.Generic;

    public class TideDay
    {
        public TideStation Station { get; set; }

        public DateTime Day { get; set; }

        public IList<TideEvent> TideEvents { get; set; }
    }
}
