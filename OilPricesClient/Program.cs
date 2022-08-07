// See https://aka.ms/new-console-template for more information
using OilPricesContract;
using StreamJsonRpc;
using System.Net;
using System.Net.Sockets;
using System.Text;

//local config
TcpClient tcpClient = new ("localhost", 6000);
//docker config
//TcpClient tcpClient = new(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6001));
 //tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6001));

Stream jsonRpcStream = tcpClient.GetStream();
IJsonRpcMessageFormatter jsonRpcMessageFormatter = new JsonMessageFormatter(Encoding.UTF8);
IJsonRpcMessageHandler jsonRpcMessageHandler = new LengthHeaderMessageHandler(jsonRpcStream, jsonRpcStream, jsonRpcMessageFormatter);

IOilPrices jsonRpcOilPricesClient = JsonRpc.Attach<IOilPrices>(jsonRpcMessageHandler);
OilPricesRequest req = new ()
{
    startDateISO8601 = "2018-02-15",
    endDateISO8601 = "2018-02-25",
};

Console.WriteLine(req.startDateISO8601.ToString() + " " + req.endDateISO8601.ToString());
OilPricesReply OilPricesReply = await jsonRpcOilPricesClient.GetOilPriceTrend(req);

foreach(OilPriceAtDateReply oilPriceAtDate in OilPricesReply.Prices)
{
    Console.WriteLine(oilPriceAtDate.Date.ToString()+" "+oilPriceAtDate.Price.ToString());
}



jsonRpcStream.Close();

Console.ReadKey();
