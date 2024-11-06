namespace Struds.Net.TidePredictor.Domain
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IServiceMetaData
    {
        string UKHOApiKey { get; }

        string UKHOBaseUrl { get; }

        string WorldTidesApiKey { get; }

        string WorldTidesBaseUrl { get; }

        string MetOfficeApiKey { get; }

        string OpenWeatherMapApiKey { get; }

        string OpenWeatherMapBaseUrl { get; }

        string MetOfficeInshoreWatersForecastUrl { get; }

        string MetOfficeShippingForecastUrl { get; }
    }
}
