using System.Threading.Tasks;
using PathInterview.Core.Result;
using PathInterview.Entities.Dto.Order.Request;

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

        /// <summary>
        /// Sipariş içindeki ürünü iptal eder
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<DataResult> CancelOrderAsync(string orderId, int productId);

        /// <summary>
        /// Üst yöneticiye gelen sipariş iptal isteklerini listeler
        /// </summary>
        /// <returns></returns>
        Task<DataResult> OrderCancelRequestAsync();

        /// <summary>
        /// Sipariş iptal isteğini onaylar veya reddeder
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<DataResult> ConfirmRequestAsync(ConfirmCancelRequest model);
    }
}

