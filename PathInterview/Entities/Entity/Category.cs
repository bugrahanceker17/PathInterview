using System.ComponentModel.DataAnnotations.Schema;
using PathInterview.Core.Entities.Concrete;

namespace PathInterview.Entities.Entity
{
    [Table("Categories")]
    public class Category : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}