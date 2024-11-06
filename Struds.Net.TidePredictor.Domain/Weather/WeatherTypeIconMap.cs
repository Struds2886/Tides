namespace Struds.Net.TidePredictor.Domain.Weather
{
    using System.Collections.Generic;

    public class WeatherTypeIconMap
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public static string BaseUrl => "http://openweathermap.org/img/wn/";

        public static IDictionary<int, WeatherTypeIconMap> BuildMap()
        {
            var map = new Dictionary<int, WeatherTypeIconMap>
            {
                {0, new WeatherTypeIconMap {Id = 0, Description = "Clear night", Icon = "01n.png"}},
                {1, new WeatherTypeIconMap {Id = 1, Description = "Suny day", Icon = "01d.png"}},
                {2, new WeatherTypeIconMap {Id = 2, Description = "Partly cloudy", Icon = "02n.png"}},
                {3, new WeatherTypeIconMap {Id = 3, Description = "Partly cloudy", Icon = "02d.png"}},
                //{4, new WeatherTypeIconMap {Id = 4, Description = "Not used", Icon = ""}},
                {5, new WeatherTypeIconMap {Id = 5, Description = "Mist", Icon = "50d.png"}},
                {6, new WeatherTypeIconMap {Id = 6, Description = "Fog", Icon = "50d.png"}},
                {7, new WeatherTypeIconMap {Id = 7, Description = "Cloudy", Icon = "02d.png"}},
                {8, new WeatherTypeIconMap {Id = 8, Description = "Overcast", Icon = "04d.png"}},
                {9, new WeatherTypeIconMap {Id = 9, Description = "Light rain shower", Icon = "09n.png"}},
                {10, new WeatherTypeIconMap {Id = 10, Description = "Light rain shower", Icon = "09d.png"}},
                {11, new WeatherTypeIconMap {Id = 11, Description = "Drizzle", Icon = "09d.png"}},
                {12, new WeatherTypeIconMap {Id = 12, Description = "Light rain", Icon = "10d.png"}},
                {13, new WeatherTypeIconMap {Id = 13, Description = "Heavy rain shower", Icon = "09n.png"}},
                {14, new WeatherTypeIconMap {Id = 14, Description = "Heavy rain shower", Icon = "09d.png"}},
                {15, new WeatherTypeIconMap {Id = 15, Description = "Heavy rain", Icon = "10d.png"}},
                {16, new WeatherTypeIconMap {Id = 16, Description = "Sleet shower", Icon = "13n.png"}},
                {17, new WeatherTypeIconMap {Id = 17, Description = "Sleet shower", Icon = "13d.png"}},
                {18, new WeatherTypeIconMap {Id = 18, Description = "Sleet", Icon = "13d.png"}},
                {19, new WeatherTypeIconMap {Id = 19, Description = "Hail shower", Icon = "10n.png"}},
                {20, new WeatherTypeIconMap {Id = 20, Description = "Hail shower", Icon = "10d.png"}},
                {21, new WeatherTypeIconMap {Id = 21, Description = "Hail", Icon = "10d.png"}},
                {22, new WeatherTypeIconMap {Id = 22, Description = "Light snow shower", Icon = "13n.png"}},
                {23, new WeatherTypeIconMap {Id = 23, Description = "Light snow shower", Icon = "13d.png"}},
                {24, new WeatherTypeIconMap {Id = 24, Description = "Light snow", Icon = "13d.png"}},
                {25, new WeatherTypeIconMap {Id = 25, Description = "Heavy snow shower", Icon = "13n.png"}},
                {26, new WeatherTypeIconMap {Id = 26, Description = "Heavy snow shower", Icon = "13d.png"}},
                {27, new WeatherTypeIconMap {Id = 27, Description = "Heavy snow", Icon = "13d.png"}},
                {28, new WeatherTypeIconMap {Id = 28, Description = "Thunder shower", Icon = "11n.png"}},
                {29, new WeatherTypeIconMap {Id = 29, Description = "Thunder shower", Icon = "11d.png"}},
                {30, new WeatherTypeIconMap {Id = 30, Description = "Thunder", Icon = "11d.png"}}
            };

            return map;
        }
    }
}
