using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OilPricesServer.Models;
using OilPricesContract;

namespace OilPricesServer.Data
{
    public class OilPricesDataHubRepository : IOilPricesRepository
    {
        private readonly HttpClient _httpClient;
        public List<OilPriceAtDate>? OilPrices=new ();
        public OilPricesDataHubRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<OilPriceAtDate>?> GetBrentDailyValuesAsync(string url)
        {
            OilPrices = await _httpClient.GetFromJsonAsync<List<OilPriceAtDate>>(url, CancellationToken.None).ConfigureAwait(true);

            return OilPrices;
        }
    }
}