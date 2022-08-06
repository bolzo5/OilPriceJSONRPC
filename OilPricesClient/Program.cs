// See https://aka.ms/new-console-template for more information
using OilPricesContract;
using StreamJsonRpc;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");
TcpClient tcpClient = new ("localhost", 6000);
//TcpClient tcpClient = new(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6001));

 //tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6001));

Stream jsonRpcStream = tcpClient.GetStream();
IJsonRpcMessageFormatter jsonRpcMessageFormatter = new JsonMessageFormatter(Encoding.UTF8);
IJsonRpcMessageHandler jsonRpcMessageHandler = new LengthHeaderMessageHandler(jsonRpcStream, jsonRpcStream, jsonRpcMessageFormatter);

IOilPrices jsonRpcOilPricesClient = JsonRpc.Attach<IOilPrices>(jsonRpcMessageHandler);
OilPricesRequest req = new OilPricesRequest
{
    StartDate = DateTime.Today,
    EndDate = DateTime.UtcNow,
};

OilPricesReply OilPricesReply = await jsonRpcOilPricesClient.GetOilPricesAsync(req);

foreach(OilPriceAtDate oilPriceAtDate in OilPricesReply.Prices)
{
    Console.WriteLine(oilPriceAtDate.Date.ToString()+" "+oilPriceAtDate.Price.ToString());
}



jsonRpcStream.Close();

Console.ReadKey();
