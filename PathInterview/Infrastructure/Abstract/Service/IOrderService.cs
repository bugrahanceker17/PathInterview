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
    }
}

