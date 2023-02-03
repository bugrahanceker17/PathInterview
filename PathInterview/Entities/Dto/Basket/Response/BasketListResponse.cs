using System;
using System.Text.Json.Serialization;

namespace PathInterview.Entities.Dto.Basket.Response
{
    public class BasketListResponse
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore] public DateTime CreatedAt { get; set; }
        public string CreatedAtText => CreatedAt.ToString("dd/MM/yyyy HH:mm");
    }
}