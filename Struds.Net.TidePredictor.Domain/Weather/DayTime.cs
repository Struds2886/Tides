namespace Struds.Net.TidePredictor.Domain.Weather
{
    public class DayTime
    {
        public string WindDirection { get; set; }

        public int WindGustNoon { get; set; }

        public int ScreenRelativeHumidityNoon { get; set; }

        public int PrecipitationProbabilityDay { get; set; }

        public int WindSpeed { get; set; }

        public string Visibility { get; set; }

        public int DayMaximumTemperature { get; set; }

        public int FeelsLikeDayMaximumTemperature { get; set; }

        public int WeatherType { get; set; }

        public string WeatherDescription { get; set; }

        public byte[] WeatherTypeIcon { get; set; }

        public int MaxUvIndex { get; set; }

        public string Type { get; set; }
    }
}