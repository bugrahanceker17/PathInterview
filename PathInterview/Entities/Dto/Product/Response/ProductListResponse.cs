namespace PathInterview.Entities.Dto.Product.Response
{
    public class ProductListResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
    }
}

