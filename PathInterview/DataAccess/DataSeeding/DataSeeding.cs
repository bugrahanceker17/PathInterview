using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PathInterview.DataAccess.Concrete;
using PathInterview.Entities.Entity;

namespace PathInterview.DataAccess.DataSeeding
{
    public static class DataSeeding
    {
        public static void Seed(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<ProjectDbContext>();
            context.Database.Migrate();
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new List<Category>()
                    {
                        new Category {  Name = "Giyim", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true },
                        new Category {  Name = "Gıda", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true }
                    }
                );
            }

            if (!context.DeliveryCompanies.Any())
            {
                context.DeliveryCompanies.AddRange(new List<DeliveryCompany>()
                {
                    new DeliveryCompany() { CompanyName = "Aras Kargo", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true },
                    new DeliveryCompany() { CompanyName = "Yurtiçi Kargo", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true }
                });
            }
            
            context.SaveChanges();

            if (!context.Products.Any())
            {
                context.Products.AddRange(new List<Product>()
                {
                    new Product() {  Title = "Siyah Erkek Tişört", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true,Price = 120, CategoryId = 1},
                    new Product() {  Title = "Gri Boğazlı Kazak", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true,Price = 195, CategoryId = 1},
                    new Product() {  Title = "Mavi Etek", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true,Price = 149, CategoryId = 1},
                    new Product() {  Title = "Yağ 5 KG", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true,Price = 130, CategoryId = 2},
                    new Product() {  Title = "Spagetti", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true,Price = 13, CategoryId = 2},
                    new Product() {  Title = "Ice Tea Mango", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true,Price = 8, CategoryId = 2}
          
                });
            }

            context.SaveChanges();
        }
    }
}