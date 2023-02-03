namespace PathInterview.Entities.Dto.Order.Response
{
    public class OrderListResponse
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public string OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

