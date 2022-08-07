using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OilPricesContract;
using OilPricesServer.Models;
using OilPricesServer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OilPricesTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async Task ShouldReturnValidOilPricesForGivenDateAsync()
        {

            OilPricesRequest req = new()
            {
                startDateISO8601 = "2018-02-15",
                endDateISO8601 = "2018-02-25",
            };

            List<OilPriceAtDate> oilPrices = new();
            oilPrices.Add(new OilPriceAtDate()
            {
                Price = 5,
                Date = DateTime.Parse("2018-02-18")
            });
            oilPrices.Add(new OilPriceAtDate()
            {
                Price = 6,
                Date = DateTime.Parse("2018-02-19")
            });
            OilPricesReply correctReply = new();
            correctReply.Prices.Add(new OilPriceAtDateReply()
            {
                Price = 5,
                Date = "2018-02-18"
            });
            correctReply.Prices.Add(new OilPriceAtDateReply()
            {
                Price = 6,
                Date = "2018-02-19"
            });

            //act
            var oilPricesService = new OilPricesService(oilPrices);
            OilPricesReply reply = await oilPricesService.GetOilPriceTrend(req);

            //assert
            Assert.IsNotNull(reply);
            for (int i = 0; i < reply.Prices.Count; i++)
            {
                Assert.AreEqual(reply.Prices[i].Date, correctReply.Prices[i].Date);
                Assert.AreEqual(reply.Prices[i].Price, correctReply.Prices[i].Price);
            }

            return;
        }
        [TestMethod]
        public async Task ShouldReturnEmptyForInvalidDateAsync()
        {

            OilPricesRequest req = new()
            {
                startDateISO8601 = "2018-02-15",
                endDateISO8601 = "2018-02-13",
            };

            List<OilPriceAtDate> oilPrices = new();
            oilPrices.Add(new OilPriceAtDate()
            {
                Price = 5,
                Date = DateTime.Parse("2018-02-13")
            });
            oilPrices.Add(new OilPriceAtDate()
            {
                Price = 6,
                Date = DateTime.Parse("2018-02-14")
            });
            OilPricesReply correctReply = new();
           

            //act
            var oilPricesService = new OilPricesService(oilPrices);
            OilPricesReply reply = await oilPricesService.GetOilPriceTrend(req);

            //assert
            Assert.IsNotNull(reply);
            for (int i = 0; i < reply.Prices.Count; i++)
            {
                Assert.AreEqual(reply.Prices[i].Date, correctReply.Prices[i].Date);
                Assert.AreEqual(reply.Prices[i].Price, correctReply.Prices[i].Price);
            }

            return;
        }
    }
}