using UnityEngine;
using System;

using Framework;
using UI.Canvas;

namespace Environment
{
    public sealed class Sun : MonoBehaviour
    {
        private const float MINUTES = 60;
        
        [Header("References")]
        [SerializeField] private WeatherManager[] weatherManagers;
        [SerializeField] private ScrollUI scrollUI;
        
        [Header("Light")]
        [SerializeField] private Light sunLight;
        [SerializeField] private Light nightLight;
        [SerializeField] private Gradient sunColorGradient;
        [SerializeField] private AnimationCurve sunIntensityCurve;
        
        [Header("Camera/background")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Gradient backgroundColorGradient;

        [Header("Debug data")]
        [SerializeField] private float timeProgress;
        [SerializeField] private float sunRise;
        [SerializeField] private float sunSet;
        
        private DateTime _sunriseTime;
        private DateTime _sunsetTime;
        private bool _dataReceived;
        private int _current;

        private void OnEnable() => weatherManagers[0].OnWeatherDataReceived += SetSun;

        private void OnDisable() => weatherManagers[0].OnWeatherDataReceived -= SetSun;

        private void Update()
        {
            if (!_dataReceived) 
                return;
            
            int timezoneOffsetInSeconds = weatherManagers[_current].CurrentWeatherData.timezone;
            DateTime utcNow = DateTime.UtcNow;
            DateTime currentCityTime = utcNow.AddSeconds(timezoneOffsetInSeconds);
            timeProgress = GetTimeProgress(currentCityTime);
            
            if (timeProgress is 1 or 0)
                ChangeLight(currentCityTime);
            
            UpdateSunRotation(timeProgress);
            UpdateSunColorAndIntensity(timeProgress);
        }

        public void SetIndexSun()
        {
            if (_current == scrollUI.CurrentPage)
                return;
            
            _current = Math.Abs(scrollUI.CurrentPage);
            Debug.Log(_current);
            SetSun(weatherManagers[_current].CurrentWeatherData);
            
            int timezoneOffsetInSeconds = weatherManagers[_current].CurrentWeatherData.timezone;
            DateTime utcNow = DateTime.UtcNow;
            DateTime currentCityTime = utcNow.AddSeconds(timezoneOffsetInSeconds);
            ChangeLight(currentCityTime);
        }

        private void SetSun(WeatherResponse weatherData)
        {
            DateTime sunriseUtc = DateTimeOffset.FromUnixTimeSeconds(weatherData.sys.sunrise).UtcDateTime;
            DateTime sunsetUtc = DateTimeOffset.FromUnixTimeSeconds(weatherData.sys.sunset).UtcDateTime;

            int timezoneOffsetInSeconds = weatherData.timezone;
            _sunriseTime = sunriseUtc.AddSeconds(timezoneOffsetInSeconds);
            _sunsetTime = sunsetUtc.AddSeconds(timezoneOffsetInSeconds);
            
            sunRise = _sunriseTime.Hour +  _sunriseTime.Minute / MINUTES;
            sunSet = _sunsetTime.Hour +  _sunsetTime.Minute / MINUTES;
            
            _dataReceived = true;
        }

        private float GetTimeProgress(DateTime currentTime)
        {
            TimeSpan totalDaySpan = _sunsetTime - _sunriseTime;
            TimeSpan timeSinceSunrise = currentTime - _sunriseTime;
            return Mathf.Clamp01((float) (timeSinceSunrise.TotalMinutes / totalDaySpan.TotalMinutes));
        }

        private void UpdateSunRotation(float currentTimeProgress)
        {
            float sunElevationAngle = Mathf.Lerp(0, 180, currentTimeProgress) - 90;
            sunLight.transform.rotation = Quaternion.Euler(new (sunElevationAngle, sunLight.transform.rotation.y, 0));
        }

        private void UpdateSunColorAndIntensity(float currentTimeProgress)
        {
            sunLight.intensity = sunIntensityCurve.Evaluate(currentTimeProgress);
            sunLight.color = sunColorGradient.Evaluate(currentTimeProgress);
            mainCamera.backgroundColor = backgroundColorGradient.Evaluate(currentTimeProgress);
        }
        
        private void ChangeLight(DateTime currentTime)
        {
            bool isNightTime = currentTime < _sunriseTime 
                               || currentTime > _sunsetTime;

            sunLight.enabled = !isNightTime;
            nightLight.enabled = isNightTime;
        }
    }
}
