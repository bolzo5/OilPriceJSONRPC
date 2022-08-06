using System.Threading.Tasks;
using OilPricesServer.Models;
namespace OilPricesServer.Data
{
    public interface IOilPricesRepository
    {

        public Task<List<OilPriceAtDate>> GetBrentDailyValuesAsync(string url);
    }
}
