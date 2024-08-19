using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Framework
{
    public class WeatherManager : MonoBehaviour
    {
        [SerializeField] private string city = "Zaandam";
        
        private const string URL = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";

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
                gameObject.AddComponent<DebugSystem.Messenger>().DebugWeather(weatherData);
            }
            else
                Debug.LogError("Error fetching weather data: " + request.error);
        }
        
        
    }
}