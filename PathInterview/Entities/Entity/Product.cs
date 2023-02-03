using System.ComponentModel.DataAnnotations.Schema;
using PathInterview.Core.Entities.Concrete;

namespace PathInterview.Entities.Entity
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
    }
}

