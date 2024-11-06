namespace Struds.Net.TidePredictor.Domain
{
    using System;
    using System.Runtime.Serialization;

    [Flags]
    public enum TideEventType : byte
    {
        [EnumMember(Value = WellKnownConstants.HighWater)]
        High = 1,

        [EnumMember(Value = WellKnownConstants.LowWater)]
        Low = 2,

        Height = 4,

        HighLow = High | Low,

        All = HighLow | Height
    }
}
