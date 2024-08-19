using System.IO;
using UnityEngine;

namespace Framework
{
    public static class ApiKey
    {
        public static string LoadApiKey()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "config.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                ApiConfig config = JsonUtility.FromJson<ApiConfig>(json);
                return config.openWeatherMapApiKey;
            }
            
            Debug.LogError("Configuration file not found.");
            return null;
        }
    }
    
    [System.Serializable]
    public class ApiConfig
    {
        public string openWeatherMapApiKey;
    }
}