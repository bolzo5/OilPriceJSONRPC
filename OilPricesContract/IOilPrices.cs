using System.Threading.Tasks;

namespace OilPricesContract
{
    public interface IOilPrices
    {
        Task<OilPricesReply> GetOilPriceTrend(OilPricesRequest request);
    }
}
