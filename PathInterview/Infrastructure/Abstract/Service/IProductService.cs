using System.Threading.Tasks;
using PathInterview.Core.Result;

namespace PathInterview.Infrastructure.Abstract.Service
{
    public interface IProductService
    {
        /// <summary>
        /// Ürünleri listeler
        /// </summary>
        /// <returns></returns>
        Task<DataResult> GetProductListAsync();
    }
}

