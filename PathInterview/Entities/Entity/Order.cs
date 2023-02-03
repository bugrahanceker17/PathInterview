using System.ComponentModel.DataAnnotations.Schema;
using PathInterview.Core.Entities.Concrete;

namespace PathInterview.Entities.Entity
{
    [Table("Orders")]
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public short DeliveryStatus { get; set; }
        public int BasketId { get; set; }
        public bool IsCancellationConfirmed { get; set; }
        public Basket Basket { get; set; }
    }
}