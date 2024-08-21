using UnityEngine;
using System;

using Framework;

namespace Environment
{
    public sealed class Sun : MonoBehaviour
    {
        private const float MINUTES = 60;
        
        [Header("References")]
        [SerializeField] private WeatherManager weatherManager;
        
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

        private void OnEnable() => weatherManager.OnWeatherDataReceived += SetSun;

        private void OnDisable() => weatherManager.OnWeatherDataReceived -= SetSun;

        private void Update()
        {
            if (!_dataReceived) 
                return;

            DateTime currentTime = DateTime.Now;
            timeProgress = GetTimeProgress(currentTime);
            
            if (timeProgress is 1 or 0)
                ChangeLight(currentTime);
            
            UpdateSunRotation(timeProgress);
            UpdateSunColorAndIntensity(timeProgress);
        }

        private void SetSun(WeatherResponse weatherData)
        {
            //todo: check with multiple time zones
            _sunriseTime = DateTimeOffset.FromUnixTimeSeconds(weatherData.sys.sunrise).DateTime.ToLocalTime();
            _sunsetTime = DateTimeOffset.FromUnixTimeSeconds(weatherData.sys.sunset).DateTime.ToLocalTime();
            _sunriseTime = DateTimeOffset.FromUnixTimeSeconds(weatherData.sys.sunrise).DateTime;
            _sunsetTime = DateTimeOffset.FromUnixTimeSeconds(weatherData.sys.sunset).DateTime;

            sunRise = _sunriseTime.Hour + _sunriseTime.Minute / MINUTES;
            sunSet = _sunsetTime.Hour + _sunsetTime.Minute / MINUTES;
            
            _dataReceived = true;
        }

        private float GetTimeProgress(DateTime currentTime)
        {
            TimeSpan totalDaySpan = _sunsetTime - _sunriseTime;
            TimeSpan timeSinceSunrise = currentTime - _sunriseTime;
            return Mathf.Clamp01((float) (timeSinceSunrise.TotalMinutes / totalDaySpan.TotalMinutes));
        }

        private void UpdateSunRotation(float timeProgress)
        {
            float sunElevationAngle = Mathf.Lerp(0, 180, timeProgress) - 90;
            sunLight.transform.rotation = Quaternion.Euler(new (sunElevationAngle, sunLight.transform.rotation.y, 0));
        }

        private void UpdateSunColorAndIntensity(float timeProgress)
        {
            sunLight.intensity = sunIntensityCurve.Evaluate(timeProgress);
            sunLight.color = sunColorGradient.Evaluate(timeProgress);
            mainCamera.backgroundColor = backgroundColorGradient.Evaluate(timeProgress);
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
