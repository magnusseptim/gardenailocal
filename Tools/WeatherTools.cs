using Microsoft.SemanticKernel;
using Newtonsoft.Json.Linq;

namespace GardenaiLocal.Tools
{
    public class WeatherTool
    {
        [KernelFunction]
        public async Task<string> GetWeather(string location)
        {
            // Geocoding: Get lat/lon from location (use Nominatim API)
            var geoUrl = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(location)}";
            using var client = new HttpClient();
            try
            {
                var geoResp = await client.GetStringAsync(geoUrl);
                var geoArr = JArray.Parse(geoResp);
                if (geoArr.Count == 0) return $"Could not find '{location}' on map.";

                var lat = geoArr[0]["lat"];
                var lon = geoArr[0]["lon"];

                // Weather from Open-Meteo
                var weatherUrl = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true";
                var weatherResp = await client.GetStringAsync(weatherUrl);
                var weatherObj = JObject.Parse(weatherResp);
                var temp = weatherObj["current_weather"]?["temperature"];
                var wind = weatherObj["current_weather"]?["windspeed"];
                var weathercode = weatherObj["current_weather"]?["weathercode"];

                return $"Weather for {location}: {temp}°C, wind {wind} km/h, code {weathercode}";
            }
            catch
            {
                return $"Could not fetch weather for {location}.";
            }
        }
    }
}
