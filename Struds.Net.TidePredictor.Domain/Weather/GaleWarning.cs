namespace Struds.Net.TidePredictor.Domain.Weather
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{Issued}, {Description}")]
    public class GaleWarning
    {
        public DateTime Issued { get; set; }

        public string Description { get; set; }
    }
}
