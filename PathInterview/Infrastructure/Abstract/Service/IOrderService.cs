using System.Threading.Tasks;
using PathInterview.Core.Result;

namespace PathInterview.Infrastructure.Abstract.Service
{
    public interface IOrderService
    {
        /// <summary>
        /// Sepetteki ürünlerin siparişini verir
        /// </summary>
        /// <returns></returns>
        Task<DataResult> AddOrderAsync();

        /// <summary>
        /// Müşteri sipariş listesini getirir
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<DataResult> OrderListAsync(int page, int pageSize);
    }
}

