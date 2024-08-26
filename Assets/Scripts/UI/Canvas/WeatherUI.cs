using UnityEngine;
using TMPro;

using Framework;

namespace UI.Canvas
{
    public sealed class WeatherUI : MonoBehaviour
    {
        private const string C = "°C";
        private const string DEGREE_SYMBOL = "°";
        private const string H = "hPa";
        private const string P = "%";
        private const string M = " meters";
        private const string METERS_PER_SECONDS = "m/s";
        
        private const string CC = "Cloud coverage: ";
        private const string WS = "Wind speed: ";
        private const string D = "Degree: ";
        private const string HD = "Humidity: ";
        private const string PR = "Pressure: ";
        private const string V = "Visibilty: ";
        
        [Header("Reference")]
        [SerializeField] private WeatherManager weatherManager;
        
        [Header("Always on")]
        [SerializeField] private TMP_Text temp;
        [SerializeField] private TMP_Text main;
        [SerializeField] private TMP_Text description;
        
        [Header("Clouds")]
        [SerializeField] private TMP_Text cloud;
        [SerializeField] private TMP_Text speed;
        [SerializeField] private TMP_Text degree;
        
        [Header("Air")]
        [SerializeField] private TMP_Text humidity;
        [SerializeField] private TMP_Text pressure;

        [Header("Other")]
        [SerializeField] private TMP_Text visibility;

        private void OnEnable() => weatherManager.OnWeatherDataReceived += UpdateUiWithData;

        private void OnDisable() => weatherManager.OnWeatherDataReceived -= UpdateUiWithData;

        private void UpdateUiWithData(WeatherResponse weatherData)
        {
            if (weatherData.weather is { Length: > 0 })
            {
                main.text = weatherData.name + ": " + weatherData.weather[0].main;
                description.text = weatherData.weather[0].description;
            }

            temp.text = weatherData.main.temp + C;
            cloud.text = CC + weatherData.clouds.all + P;
            speed.text = WS + weatherData.wind.speed + METERS_PER_SECONDS;
            degree.text = D + weatherData.wind.deg + DEGREE_SYMBOL;
            humidity.text = HD + weatherData.main.humidity + P;
            pressure.text = PR + weatherData.main.pressure + H;
            visibility.text = V + weatherData.visibility + M;
        }
    }
}