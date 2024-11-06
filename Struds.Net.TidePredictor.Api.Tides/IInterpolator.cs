namespace Struds.Net.TidePredictor.Api.Tides
{
    public interface IInterpolator
    {
        double Interpolate(double time0, double value0, double time1, double value1, double timeRequired);
    }
}