using System.Threading.Tasks;
using PathInterview.Core.Result;
using PathInterview.Entities.Dto.Basket.Request;

namespace PathInterview.Infrastructure.Abstract.Service
{
    public interface IBasketService
    {
        /// <summary>
        /// Sepete ürün ekler
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<DataResult> AddBasketAsync(AddBasketRequest model);

        /// <summary>
        /// Kullanıcı sepetini listeler
        /// </summary>
        /// <returns></returns>
        Task<DataResult> GetBasketAsync();
    }
}

