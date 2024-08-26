using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

using Framework.DebugSystem;

namespace Framework
{
    public class WeatherManager : MonoBehaviour
    {
        private const string CITY_API_CALL = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&lang";
        private const string LOCATION_API_CALL = "https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&units=metric&lang"; // &lang=nl
        
        [SerializeField] private Messenger messenger;
        [SerializeField] private string city = "Zaandam";

        public WeatherResponse CurrentWeatherData { get; private set; }

        public delegate void WeatherDataReceivedHandler(WeatherResponse weatherData);
        
        public event WeatherDataReceivedHandler OnWeatherDataReceived;

        private void Start() => StartApi();
        
        public void StartApi()
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
            
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    CurrentWeatherData = JsonUtility.FromJson<WeatherResponse>(request.downloadHandler.text);
                    OnWeatherDataReceived?.Invoke(CurrentWeatherData);
                    messenger.DebugWeather(CurrentWeatherData);
                    break;
                
                // todo: fill switch case
                case UnityWebRequest.Result.InProgress:
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    break;
                
                default:
                    Debug.LogError("Error fetching weather data: " + request.error);
                    break;
            }
        }
    }
}