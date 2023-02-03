using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PathInterview.Entities.Entity
{
    [Table("Users")]
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsStatus { get; set; }
        public bool IsDeleted { get; set; }
    }
}