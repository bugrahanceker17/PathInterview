using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PathInterview.Core.Entities.Concrete;

namespace PathInterview.Entities.Entity
{
    [Table("Baskets")]
    public class Basket : BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}