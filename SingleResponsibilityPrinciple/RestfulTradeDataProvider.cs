using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace SingleResponsibilityPrinciple
{
    public class RestfulTradeDataProvider : ITradeDataProvider
    {
        private readonly string _url;
        private readonly ILogger _logger;
        public RestfulTradeDataProvider(string url, ILogger logger)
        {
            _url = url;
            _logger = logger;

        }

        public IEnumerable<string> GetTradeData()
        {
            return GetTradeDataAsync().Result;
        }

        private async Task<IEnumerable<string>> GetTradeDataAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(_url);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var trades = JsonSerializer.Deserialize<List<string>>(jsonResponse);

                    return trades ?? new List<string>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Error fetching trade data: " + ex.Message);
                return new List<string>();
            }
        }
    }
}
