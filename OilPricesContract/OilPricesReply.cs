namespace OilPricesContract
{
    public class OilPricesReply
    {
        public List<OilPriceAtDate> Prices { get; set; } = new List<OilPriceAtDate>();
    }
    public class OilPriceAtDate
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }


    }
}
