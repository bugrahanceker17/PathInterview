﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PathInterview.Core.Result;
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
    }
}