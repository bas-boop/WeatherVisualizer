namespace Framework
{
    [System.Serializable]
    public class WeatherResponse
    {
        public Coord coord;
        public Weather[] weather;
        public Main main;
        public Wind wind;
        public Clouds clouds;
        public Sys sys;
        public int visibility;
        public int timezone;
        public string name;
    }

    [System.Serializable]
    public class Coord
    {
        public float lon;
        public float lat;
    }

    [System.Serializable]
    public class Weather
    {
        public int id;
        public string main;
        public string description;
        public string icon;
    }

    [System.Serializable]
    public class Main
    {
        public float temp;
        public float feelsLike;
        public float tempMin;
        public float tempMax;
        public int pressure;
        public int humidity;
    }

    [System.Serializable]
    public class Wind
    {
        public float speed;
        public int deg;
    }

    [System.Serializable]
    public class Clouds
    {
        public int all;
    }

    [System.Serializable]
    public class Sys
    {
        public int type;
        public int id;
        public string country;
        public int sunrise;
        public int sunset;
    }
}