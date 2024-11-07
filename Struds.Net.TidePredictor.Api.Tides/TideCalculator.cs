namespace Struds.Net.TidePredictor.Api.Tides
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;

    public class TideCalculator : ITideCalculator
    {
        private static readonly NLog.ILogger Logger = NLog.LogManager.GetCurrentClassLogger();

        public double CalculateRequiredHeightOfTideOverDryingObject(double vesselDraught, double clearanceRequired, double chartedDepth, double dryingHeight)
        {
            var vesselDraughtAndClearance = vesselDraught + clearanceRequired;

            var requiredHeightOfTide = (vesselDraughtAndClearance - chartedDepth) + dryingHeight;

            Logger.Debug($"{nameof(this.CalculateRequiredHeightOfTideOverDryingObject)}: ({vesselDraughtAndClearance:F2} - {chartedDepth:F2}) + {dryingHeight:F2} = {requiredHeightOfTide:F2} Metres");

            return requiredHeightOfTide;
        }

        public ClearanceTideEvents CalculateClearanceTimes(IList<TideEvent> tideEvents, double tideHeightRequired)
        {
            var suitableTideTimes = tideEvents.OrderBy(x => x.DateTime).Where(x => x.Height >= tideHeightRequired).ToList();

            return new ClearanceTideEvents(suitableTideTimes);
        }

        public double CalculateSafeAnchorDepth(double vesselDraught, double clearanceRequired, double tideHeightNow, double lowWaterHeight)
        {
            var vesselDraughtAndClearance = vesselDraught + clearanceRequired;
            var fallToLowWater = tideHeightNow - lowWaterHeight;

            var safeAnchorDepth = vesselDraughtAndClearance + fallToLowWater;

            Logger.Debug($"{nameof(this.CalculateSafeAnchorDepth)}: ({vesselDraughtAndClearance:F2}) + ({fallToLowWater:F2}) = {safeAnchorDepth:F2} Metres");

            return safeAnchorDepth;
        }
    }
}