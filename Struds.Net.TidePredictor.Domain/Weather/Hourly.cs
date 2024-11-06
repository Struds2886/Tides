namespace Struds.Net.TidePredictor.Domain.Weather
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    public class Hourly
    {
        [JsonProperty("dt")]
        public int Timestamp { get; set; }

        public DateTime DateTime { get; set; }

        [JsonProperty("temp")]
        public float Temperature { get; set; }

        [JsonProperty("feels_like")]
        public float TemperatureFeelsLike { get; set; }

        [JsonProperty("pressure")]
        public int Pressure { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("dew_point")]
        public float DewPoint { get; set; }

        [JsonProperty("clouds")]
        [Description("Cloudiness, %")]
        public int Clouds { get; set; }

        [JsonProperty("visibility")]
        [Description("Average visibility, metres")]
        public int Visibility { get; set; }

        [JsonProperty("wind_speed")]
        [Description("Wind Speed, mph")]
        public float WindSpeed { get; set; }

        [JsonProperty("wind_deg")]
        [Description("Wind direction, degrees (meteorological)")]
        public int WindDegrees { get; set; }

        public CardinalPoints WindDirection { get; set; }

        [JsonProperty("weather")]
        public HourlyWeather[] HourlyWeather { get; set; }

        [JsonProperty("pop")]
        public float ProbabilityOfRain { get; set; }

        [OnDeserialized]
        internal void HandleOnDeSerialized(StreamingContext context)
        {
            this.DateTime = UnixTime.ToUniversalDateTime(this.Timestamp);

            this.WindSpeed = (float)UnitsNet.Speed.FromMetersPerSecond(this.WindSpeed).MilesPerHour;
            this.WindDirection = this.WindDegrees.ToCardinalPoints();
        }
    }
}