using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OilPricesServer.Models;

namespace OilPricesServer.Data
{
    public class OilPricesDataHubRepository : IOilPricesRepository
    {
        private readonly HttpClient _httpClient;
        public List<OilPriceAtDate> OilPrices=new List<OilPriceAtDate>();
        public OilPricesDataHubRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<OilPriceAtDate>> GetBrentDailyValuesAsync(string url)//url "https://datahub.io/core/oil-prices/r/brent-daily.json"
        {
            OilPrices = await _httpClient.GetFromJsonAsync<List<OilPriceAtDate>>("https://datahub.io/core/oil-prices/r/brent-daily.json", CancellationToken.None).ConfigureAwait(true);

            return OilPrices;
        }
    }
}