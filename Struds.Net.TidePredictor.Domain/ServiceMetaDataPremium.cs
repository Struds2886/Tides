namespace Struds.Net.TidePredictor.Domain
{
    //TODO: This class should be replaced by a Settings object coming from appsettings.config
    public class ServiceMetaDataPremium : IServiceMetaData
    {
        // 2807c070d53b4465b1c1ff7478f66f4c
        public string UKHOApiKey => "2807c070d53b4465b1c1ff7478f66f4c";

        // https://admiraltyapi.azure-api.net/uktidalapi/api/V1
        public string UKHOBaseUrl => "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/";

        public string WorldTidesApiKey => "cc83aa64-e351-49fd-a3c5-7bcb17a9983e";

        public string WorldTidesBaseUrl => "https://www.worldtides.info/api/v2";

        public string MetOfficeApiKey => "a07603a1-ffe9-4b76-8b1c-048c5e66fe1f";

        public string OpenWeatherMapApiKey => "1258f3158a740caba6aa6dae5557e96d";

        public string OpenWeatherMapBaseUrl => $"https://api.openweathermap.org/data/2.5/onecall?appid={this.OpenWeatherMapApiKey}&units=metric&exclude=minutely";

        public string MetOfficeInshoreWatersForecastUrl => "https://www.metoffice.gov.uk/weather/specialist-forecasts/coast-and-sea/inshore-waters-forecast";

        public string MetOfficeShippingForecastUrl => "https://www.metoffice.gov.uk/weather/specialist-forecasts/coast-and-sea/shipping-forecast";
    }
}