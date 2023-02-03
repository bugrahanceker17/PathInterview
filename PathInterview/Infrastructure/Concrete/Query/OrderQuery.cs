using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PathInterview.Core.DataAccess.EntityFramework;
using PathInterview.DataAccess.Concrete;
using PathInterview.Entities.Dto.Order.Response;
using PathInterview.Entities.Entity;
using PathInterview.Infrastructure.Abstract.Query;

namespace PathInterview.Infrastructure.Concrete.Query
{
    public class OrderQuery : EfEntityRepositoryBase<Order, ProjectDbContext>, IOrderQuery
    {
        public async Task<List<OrderListResponse>> GetOrders(int page, int pageSize, string userId)
        {
            await using var context = new ProjectDbContext();
            
            IQueryable<OrderListResponse> result = from or in context.Orders
                join bs in context.Baskets on or.BasketId equals bs.Id
                join pr in context.Products on bs.ProductId equals pr.Id
                join ct in context.Categories on pr.CategoryId equals ct.Id
                where or.UserId == userId
                select new OrderListResponse
                {
                    Id = or.Id,
                    BasketId = or.BasketId,
                    OrderId = or.OrderId,
                    Price = pr.Price,
                    Quantity = bs.Quantity,
                    CategoryId = pr.CategoryId,
                    ProductId = pr.Id,
                    ProductTitle = pr.Title
                };
            return result.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<OrderListResponse> DetailOrder(string orderId, int productId, string userId)
        {
            await using var context = new ProjectDbContext();
            
            IQueryable<OrderListResponse> result = from or in context.Orders
                join bs in context.Baskets on or.BasketId equals bs.Id
                join pr in context.Products on bs.ProductId equals pr.Id
                join ct in context.Categories on pr.CategoryId equals ct.Id
                where or.OrderId == orderId && pr.Id == productId && or.UserId == userId
                select new OrderListResponse
                {
                    Id = or.Id,
                    BasketId = or.BasketId,
                    OrderId = or.OrderId,
                    Price = pr.Price,
                    Quantity = bs.Quantity,
                    CategoryId = pr.CategoryId,
                    ProductId = pr.Id,
                    ProductTitle = pr.Title
                };

            return result.FirstOrDefault();
        }
    }
}