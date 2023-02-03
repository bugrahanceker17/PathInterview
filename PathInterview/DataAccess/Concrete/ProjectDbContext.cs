using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using PathInterview.Entities.Entity;

namespace PathInterview.DataAccess.Concrete
{
    public class ProjectDbContext : IdentityDbContext<User>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot myConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            optionsBuilder.UseSqlServer(myConfig.GetSection("Database:ConnectionString").Value);
        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DeliveryCompany> DeliveryCompanies { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}