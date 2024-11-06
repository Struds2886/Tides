namespace Struds.Net.TidePredictor.Domain.Weather
{
    public class NightTime
    {
        public NightTime()
        {
            this.WeatherTypeIcon = new byte[] { };
        }

        public string WindDirection { get; set; }

        public int WindGustMidnight { get; set; }

        public int ScreenRelativeHumidityMidnight { get; set; }

        public int PrecipitationProbabilityNight { get; set; }

        public int WindSpeed { get; set; }

        public string Visibility { get; set; }

        public int NightMinimumTemperature { get; set; }

        public int FeelsLikeNightMinimumTemperature { get; set; }

        public int WeatherType { get; set; }

        public string WeatherDescription { get; set; }

        public byte[] WeatherTypeIcon { get; set; }

        public string Type { get; set; }
    }
}