using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PathInterview.Core.DataAccess;
using PathInterview.Entities.Dto.Order.Response;
using PathInterview.Entities.Entity;

namespace PathInterview.Infrastructure.Abstract.Query
{
    public interface IOrderQuery : IEntityRepository<Order>
    {
        Task<List< OrderListResponse>> GetOrders(int page, int pageSize, string userId);
    }
}

