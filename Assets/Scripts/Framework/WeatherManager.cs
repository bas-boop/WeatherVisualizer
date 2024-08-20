using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

using Framework.DebugSystem;

namespace Framework
{
    public class WeatherManager : MonoBehaviour
    {
        [SerializeField] private Messenger messenger;
        [SerializeField] private string city = "Zaandam";
        
        public delegate void WeatherDataReceivedHandler(WeatherResponse weatherData);
        public event WeatherDataReceivedHandler OnWeatherDataReceived;
        
        private const string URL = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&lang"; // &lang=nl

        private void Start()
        {
            string key = ApiKey.LoadApiKey();
            
            if (key == null)
                return;
            
            StartCoroutine(GetWeatherData(key));
        }

        private IEnumerator GetWeatherData(string key)
        {
            string requestUrl = string.Format(URL, city, key);
            UnityWebRequest request = UnityWebRequest.Get(requestUrl);
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                WeatherResponse weatherData = JsonUtility.FromJson<WeatherResponse>(request.downloadHandler.text);
                OnWeatherDataReceived?.Invoke(weatherData);
                
                messenger.DebugWeather(weatherData);
            }
            else
                Debug.LogError("Error fetching weather data: " + request.error);
        }
    }
}