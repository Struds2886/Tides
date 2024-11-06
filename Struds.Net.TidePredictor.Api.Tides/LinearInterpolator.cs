namespace Struds.Net.TidePredictor.Api.Tides
{
    using System;

    public class LinearInterpolator : IInterpolator
    {
        public double Interpolate(double time0, double value0, double time1, double value1, double timeRequired)
        {
            var timeDifference = time1 - time0;
            if (Math.Abs(timeDifference - 0.0) < double.Epsilon)
            {
                return timeRequired <= value0 ? value0 : value1;
            }

            return value0 + (((timeRequired - time0) * (value1 - value0)) / timeDifference);
        }

	}
}
