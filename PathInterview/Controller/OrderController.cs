using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PathInterview.Core.Result;
using PathInterview.Entities.Dto.Order.Request;
using PathInterview.Infrastructure.Abstract.Service;

namespace PathInterview.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddOrderAsync()
        {
            DataResult dataResult = await _orderService.AddOrderAsync();
            return dataResult.HttpResponse();
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> OrderListAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            DataResult dataResult = await _orderService.OrderListAsync(page, pageSize);
            return dataResult.HttpResponse();
        }

        [HttpDelete]
        [Route("cancel")]
        public async Task<IActionResult> CancelOrderAsync([FromQuery] string orderId, [FromQuery] int productId)
        {
            DataResult dataResult = await _orderService.CancelOrderAsync(orderId, productId);
            return dataResult.HttpResponse();
        }
        
        [HttpGet]
        [Route("cancel-requests")]
        public async Task<IActionResult> OrderCancelRequestAsync()
        {
            DataResult dataResult = await _orderService.OrderCancelRequestAsync();
            return dataResult.HttpResponse();
        }

        [HttpPost]
        [Route("confirm-cancel-request")]
        public async Task<IActionResult> ConfirmRequestAsync([FromBody] ConfirmCancelRequest model)
        {
            DataResult dataResult = await _orderService.ConfirmRequestAsync(model);
            return dataResult.HttpResponse();
        }
    }
}