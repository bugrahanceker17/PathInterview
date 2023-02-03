using System.Collections.Generic;
using System.Threading.Tasks;
using PathInterview.Core.DataAccess;
using PathInterview.Entities.Dto.Order.Response;
using PathInterview.Entities.Entity;

namespace PathInterview.Infrastructure.Abstract.Query
{
    public interface IOrderQuery : IEntityRepository<Order>
    {
        Task<List<OrderListResponse>> GetOrders(int page, int pageSize, string userId);
        Task<OrderListResponse> DetailOrder(string orderId, int productId, string userId);
    }
}