using OilPricesContract;
using OilPricesServer;

namespace OilPricesServer.Services
{
    public class OilPricesServer : IOilPrices
    {
        public Task<OilPricesReply> GetOilPricesAsync(OilPricesRequest request)
        {
            OilPricesReply reply= new OilPricesReply();
            reply.Prices = new List<OilPriceAtDate>();
            OilPriceAtDate pd= new OilPriceAtDate();
            pd.Price = 100;
            pd.Date = DateTime.UtcNow;
            reply.Prices.Add(pd);


            return Task.FromResult(reply); 
        }
    }
}