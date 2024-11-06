// ReSharper disable InconsistentNaming
namespace Struds.Net.TidePredictor.Domain.Weather
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    public class Daily
    {
        [JsonProperty("dt")]
        public int Timestamp { get; set; }

        public DateTime DateTime { get; set; }
        
        [JsonProperty("sunrise")]
        public int SunriseTimestamp { get; set; }

        public DateTime Sunrise { get; set; }

        [JsonProperty("sunset")]
        public int SunsetTimestamp { get; set; }

        public DateTime Sunset { get; set; }

        [JsonProperty("temp")]
        public Temperature Temperature { get; set; }

        [JsonProperty("feels_like")]
        public TemperatureFeelsLike TemperatureFeelsLike { get; set; }

        [JsonProperty("pressure")]
        public int Pressure { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("dew_point")]
        public float DewPoint { get; set; }

        [JsonProperty("wind_speed")]
        [Description("Wind Speed, mph")]
        public float WindSpeed { get; set; }

        [JsonProperty("wind_deg")]
        [Description("Wind direction, degrees (meteorological)")]
        public int WindDegrees { get; set; }

        public CardinalPoints WindDirection { get; set; }

        [JsonProperty("weather")]
        public DailyWeather[] DailyWeather { get; set; }

        [JsonProperty("visibility")]
        [Description("Average visibility, metres")]
        public int Visibility { get; set; }

        [JsonProperty("clouds")]
        [Description("Cloudiness, %")]
        public int Clouds { get; set; }

        [JsonProperty("pop")]
        public float ProbabilityOfRain { get; set; }

        [JsonProperty("rain")]
        [Description("Precipitation volume, mm")]
        public float RainVolume { get; set; }

        [JsonProperty("uvi")]
        public float UVIndex { get; set; }

        [OnDeserialized]
        internal void HandleOnDeSerialized(StreamingContext context)
        {
            this.DateTime = UnixTime.ToUniversalDateTime(this.Timestamp);
            this.Sunrise = UnixTime.ToUniversalDateTime(this.SunriseTimestamp);
            this.Sunset = UnixTime.ToUniversalDateTime(this.SunsetTimestamp);

            this.WindSpeed = (float)UnitsNet.Speed.FromMetersPerSecond(this.WindSpeed).MilesPerHour;
            this.WindDirection = this.WindDegrees.ToCardinalPoints();
        }
    }
}