namespace OilPricesContract
{
    public class OilPricesReply
    {
        public List<OilPriceAtDateReply> Prices { get; set; } = new List<OilPriceAtDateReply>();
    }
    public class OilPriceAtDateReply
    {
        public string Date { get; set; }
        public double Price { get; set; }

    }
}
