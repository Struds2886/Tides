namespace Struds.Net.TidePredictor.Api.Tides
{
    using System.Collections.Generic;
    using Domain;

    public interface ITideCalculator
    {
        double CalculateRequiredHeightOfTideOverDryingObject(double vesselDraught, double clearanceRequired, double chartedDepth, double dryingHeight);

        ClearanceTideEvents CalculateClearanceTimes(IList<TideEvent> tideEvents, double tideHeightRequired);

        double CalculateSafeAnchorDepth(double vesselDraught, double clearanceRequired, double tideHeightNow, double lowWaterHeight);
    }
}