using System.Threading.Tasks;

namespace OilPricesContract
{
    public interface IOilPrices
    {
        Task<OilPricesReply> GetOilPricesAsync(OilPricesRequest request);
    }
}
