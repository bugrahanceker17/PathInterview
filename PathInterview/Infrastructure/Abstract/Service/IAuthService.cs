using System.Threading.Tasks;
using PathInterview.Core.Result;
using PathInterview.Entities.Dto.Auth.Request;

namespace PathInterview.Infrastructure.Abstract.Service
{
    public interface IAuthService
    {
        /// <summary>
        /// Kayıt işlemi yapar
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<DataResult> RegisterAsync(RegisterRequest model);

        /// <summary>
        /// Giriş işlemi yapar ve token oluşturur
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<DataResult> LoginAsync(LoginRequest model);
    }
}