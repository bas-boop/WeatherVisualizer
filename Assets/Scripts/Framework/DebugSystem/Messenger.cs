using System.Linq;
using UnityEngine;

namespace Framework.DebugSystem
{
    public class Messenger : MonoBehaviour
    {
        /// <summary>
        /// Logs the given message in the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void DebugLog(string message) => Debug.Log(message);
        
        /// <summary>
        /// Logs the given message in the console as a warning.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void DebugLogWarning(string message) => Debug.LogWarning(message);
        
        /// <summary>
        /// Logs the given message in the console as an error.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void DebugLogError(string message) => Debug.LogError(message);

        public void DebugWeather(WeatherResponse weatherResponse)
        {
            string logMessage = "City: " + weatherResponse.name + "\n" +
                                "Coordinates: Latitude = " + weatherResponse.coord.lat + ", Longitude = " + weatherResponse.coord.lon + "\n" +
                                "Weather: \n";

            if (weatherResponse.weather is { Length: > 0 })
            {
                logMessage = weatherResponse.weather.Aggregate(logMessage, (current, weather) 
                    => current + "  ID: " + weather.id + "\n" 
                       + "  Main: " + weather.main + "\n" 
                       + "  Description: " + weather.description + "\n" 
                       + "  Icon: " + weather.icon + "\n");
            }
            else
                logMessage += "  Weather array is null or empty.\n";

            logMessage += "Main Weather Info:\n" +
                          "  Temperature: " + weatherResponse.main.temp + "°C\n" +
                          "  Feels Like: " + weatherResponse.main.feelsLike + "°C\n" +
                          "  Temperature Min: " + weatherResponse.main.tempMin + "°C\n" +
                          "  Temperature Max: " + weatherResponse.main.tempMax + "°C\n" +
                          "  Pressure: " + weatherResponse.main.pressure + " hPa\n" +
                          "  Humidity: " + weatherResponse.main.humidity + "%\n" +
                          "Wind Info:\n" +
                          "  Speed: " + weatherResponse.wind.speed + " m/s\n" +
                          "  Degree: " + weatherResponse.wind.deg + "°\n" +
                          "Clouds:\n" +
                          "  Coverage: " + weatherResponse.clouds.all + "%\n" +
                          "Visibility: " + weatherResponse.visibility + " meters\n" +
                          "Timezone: " + weatherResponse.timezone + " seconds from GMT\n" +
                          "System Info:\n" +
                          "  Type: " + weatherResponse.sys.type + "\n" +
                          "  ID: " + weatherResponse.sys.id + "\n" +
                          "  Country: " + weatherResponse.sys.country + "\n" +
                          "  Sunrise: " + weatherResponse.sys.sunrise + "\n" +
                          "  Sunset: " + weatherResponse.sys.sunset + "\n";

            Debug.Log(logMessage);
        }
    }
}