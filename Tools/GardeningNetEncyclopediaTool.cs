using Microsoft.SemanticKernel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GardenaiLocal.Tools
{
    public class GardeningNetEncyclopediaTool
    {
        [KernelFunction]
        public async Task<string> LookupPlant(string plant)
        {
            string url = $"https://en.wikipedia.org/api/rest_v1/page/summary/{Uri.EscapeDataString(plant)}";
            using var client = new HttpClient();
            try
            {
                var json = await client.GetStringAsync(url);
                var obj = JObject.Parse(json);

                // Wikipedia API returns a "extract" (summary) field
                var extractToken = obj["extract"];
                string extract = extractToken?.ToString() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(extract))
                    return $"Sorry, I couldn't find information about '{plant}'.";

                return extract;
            }
            catch
            {
                return $"Sorry, I couldn't find information about '{plant}'.";
            }
        }
    }
}