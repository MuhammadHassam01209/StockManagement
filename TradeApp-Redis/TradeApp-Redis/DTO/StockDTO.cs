namespace TradeApp_Redis.DTO
{
    public class StockDTO
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
