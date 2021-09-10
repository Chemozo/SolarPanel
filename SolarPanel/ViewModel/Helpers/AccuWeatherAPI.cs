using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static SolarPanel.Model.CurrentWeather;

namespace SolarPanel.ViewModel.Helpers
{
    class AccuWeatherAPI
    {
        public const string BASE_URL = "http://dataservice.accuweather.com/";
        public const string CITY_KEY = "242560";
        public const string API_KEY = "xgPp8sqs2MpPtDYbqaExHvL6MrqBp105";
        public const string CURRENT_CONDITIONS = "currentconditions/v1/{0}?apikey={1}&details=true";

        public static async Task<Weather> GetHumidity()
        {
            int humidity;
            List<Weather> weather = new List<Weather>();

            string url = BASE_URL + string.Format(CURRENT_CONDITIONS, CITY_KEY, API_KEY);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();
                
                weather = JsonConvert.DeserializeObject<List<Weather>>(json);
                humidity = weather[0].RelativeHumidity;
            }

            return weather[0];
        }
    }
}
