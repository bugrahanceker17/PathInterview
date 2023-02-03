using System.ComponentModel.DataAnnotations.Schema;
using PathInterview.Core.Entities.Concrete;

namespace PathInterview.Entities.Entity
{
    [Table("DeliveryCompanies")]
    public class DeliveryCompany : BaseEntity
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
    }
}

