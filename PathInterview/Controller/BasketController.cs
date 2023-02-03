using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PathInterview.Core.Result;
using PathInterview.Entities.Dto.Basket.Request;
using PathInterview.Infrastructure.Abstract.Service;

namespace PathInterview.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketAsync()
        {
            DataResult dataResult = await _basketService.GetBasketAsync();
            return dataResult.HttpResponse(); 
        }

        [HttpPost]
        public async Task<IActionResult> AddBasketAsync([FromBody] AddBasketRequest model)
        {
            DataResult dataResult = await _basketService.AddBasketAsync(model);
            return dataResult.HttpResponse();
        }

    }
}