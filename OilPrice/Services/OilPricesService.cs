using OilPricesContract;
using OilPricesServer;
using OilPricesServer.Data;
using OilPricesServer.Models;
using System.Globalization;

namespace OilPricesServer.Services
{
    public class OilPricesService : IOilPrices
    {

        public List<OilPriceAtDate>? _oilPrices { get; set; }

        public OilPricesService(List<OilPriceAtDate>? oilPrices)
        {
            _oilPrices = oilPrices;
        }


        public Task<OilPricesReply> GetOilPriceTrend(OilPricesRequest request)
        {
            OilPricesReply reply = new()
            {
                Prices = new List<OilPriceAtDateReply>()
            };
            //if the date is not in order, return no values

            try
            {
                if (!DateTime.TryParseExact(request.startDateISO8601, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
                {
                    return Task.FromResult(reply);
                }

                if (!DateTime.TryParseExact(request.endDateISO8601, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
                {
                    return Task.FromResult(reply);
                }


                if (startDate <= endDate && endDate >= startDate)
                {

                    if (_oilPrices is not null && _oilPrices.Any())
                    {
                        List<OilPriceAtDate> selectedDaysInterval = _oilPrices.Where(x => x.Date >= startDate && x.Date <= endDate).ToList();
                        foreach (OilPriceAtDate oilPriceAtDate in selectedDaysInterval)
                        {
                            OilPriceAtDateReply oilPriceAtDateReply = new()
                            {
                                Price = oilPriceAtDate.Price,
                                Date = oilPriceAtDate.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                            };
                            reply.Prices.Add(oilPriceAtDateReply);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //todo log  error
                reply.Prices = new List<OilPriceAtDateReply>();
                return Task.FromResult(reply);
            }
            return Task.FromResult(reply);
        }
    }
}