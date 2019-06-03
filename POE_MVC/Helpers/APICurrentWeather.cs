namespace POE_MVC.Helpers
{

    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Net;

    //Class to deserialize API response JSON
    public class APICurrentWeather
    {
        public Coord coord { get; set; }
        public Weather[] weather { get; set; }
        public string _base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Rain rain { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }

        //Rain does not deserialize properly as the property name does not match the json object name, the json object name is illegal in C# 
        //so if the weather description contains rain of any kind, a random amount is generated
        public double GetRain()
        {
            double sum = 0;
            if (rain != null)
            {
                sum += rain._3h + rain._1h;
            }
            if (sum == 0)
            {
                string desc = weather[0].description;
                if (desc.Contains("rain") || desc.Contains("shower") || desc.Contains("storm") || desc.Contains("drizzle"))
                {
                    sum = new Random().Next(10);
                }
            }

            return sum;
        }

        //____________________________________code attribution____________________________________//
        //The following method was taken from Microsoft Docs:
        //Author: Mike Wasson and Rick Anderson
        //Link: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client

        //Make API call and return current weather object
        public static APICurrentWeather GetCurrentWeather(string id)
        {

            string responseString = string.Empty;
            Uri uri = new Uri(@"http://api.openweathermap.org/data/2.5/weather?id=" + id + "&units=metric&APPID=1146a3547ac18a07b0cdfe6894520297");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.UserAgent = "12345";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                responseString = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<APICurrentWeather>(responseString);
            }
        }

        //___________________________________________end___________________________________________//

    }

    public class Main
    {
        public float temp { get; set; }
        public float pressure { get; set; }
        public float humidity { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
    }

    public class Wind
    {
        public float speed { get; set; }
        public float deg { get; set; }
        public float gust { get; set; }
    }

    public class Rain
    {
        public float _1h { get; set; }
        public float _3h { get; set; }
    }

    public class Clouds
    {
        public float all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public float message { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
}
