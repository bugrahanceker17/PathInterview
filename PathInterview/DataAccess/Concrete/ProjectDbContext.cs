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
            // optionsBuilder.ApplyConfiguration(new Configuration());
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<DeliveryCompany> DeliveryCompanies { get; set; }
        public DbSet<Product> Products { get; set; }
        
        public class Configuration : IEntityTypeConfiguration<Category>
        {
            public void Configure(EntityTypeBuilder<Category> builder)
            {
                ConfigureCategory(builder);
            }
        }

        private static void ConfigureCategory(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.Property(s => s.Id).IsRequired(false);
            builder.Property(s => s.Name).HasDefaultValue(true);
            builder.HasData(
                    new Category
                    {
                        Id = 1,
                        Name = "Giyim",
                        CreatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsStatus = true
                    },
                    new Category
                    {
                        Id = 2,
                        Name = "Gıda",
                        CreatedAt = DateTime.Now,
                        IsDeleted = false,
                        IsStatus = true
                    }
                );
        }
    }
}