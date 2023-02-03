using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PathInterview.Core.Result;
using PathInterview.Entities.Dto.Auth.Request;
using PathInterview.Infrastructure.Abstract.Service;

namespace PathInterview.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest model)
        {
            DataResult dataResult = await _authService.RegisterAsync(model);
            return dataResult.HttpResponse();
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest model)
        {
            DataResult dataResult = await _authService.LoginAsync(model);
            return dataResult.HttpResponse();
        }
    }
}