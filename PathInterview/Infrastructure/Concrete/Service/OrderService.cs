using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using PathInterview.Core.Extensions;
using PathInterview.Core.Result;
using PathInterview.Entities.Dto.Order.Response;
using PathInterview.Entities.Entity;
using PathInterview.Entities.Enums;
using PathInterview.Infrastructure.Abstract.Query;
using PathInterview.Infrastructure.Abstract.Service;

namespace PathInterview.Infrastructure.Concrete.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderQuery _orderQuery;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBasketQuery _basketQuery;
        private readonly IMapper _mapper;

        public OrderService(IOrderQuery orderQuery, IBasketQuery basketQuery, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _orderQuery = orderQuery;
            _basketQuery = basketQuery;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }


        public async Task<DataResult> AddOrderAsync()
        {
            DataResult dataResult = new();

            (bool login, string message) = _httpContextAccessor.LoginExists();

            if (!login)
            {
                dataResult.ErrorMessageList.Add(message);
                return dataResult;
            }

            string userId = _httpContextAccessor.AccessToken().userId;

            List<Basket> basketList = await _basketQuery.GetAll(c => c.UserId.Equals(userId) && c.IsStatus);

            if (!basketList.Any())
            {
                dataResult.ErrorMessageList.Add("Sepette ürün bulunamadı");
                return dataResult;
            }

            List<Order> bulkInsertModel = new List<Order>();

            string orderId = Guid.NewGuid().ToString().Split("-")[4];

            foreach (Basket basket in basketList)
            {
                Order entity = new Order()
                {
                    BasketId = basket.Id,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    IsStatus = true,
                    UserId = userId,
                    IsCancellationConfirmed = false,
                    OrderId = orderId,
                    IsCanceledRequest = false,
                    DeliveryStatus = (short)DeliveryStatus.SIPARIS_ALINDI
                };

                bulkInsertModel.Add(entity);
            }

            int execute = await _orderQuery.BulkAdd(bulkInsertModel);

            if (execute <= 0)
            {
                dataResult.ErrorMessageList.Add("Sipariş verilemedi");
                return dataResult;
            }

            basketList.ForEach(Action);

            async void Action(Basket item)
            {
                item.IsStatus = false;
                await _basketQuery.Update(item);
            }

            dataResult.Data = "Siparişiniz alındı";
            return dataResult;
        }

        public async Task<DataResult> OrderListAsync(int page, int pageSize)
        {
            DataResult dataResult = new();

            (bool login, string message) = _httpContextAccessor.LoginExists();

            if (!login)
            {
                dataResult.ErrorMessageList.Add(message);
                return dataResult;
            }

            string userId = _httpContextAccessor.AccessToken().userId;

            var list = await _orderQuery.GetOrders(page, pageSize, userId);

            if (list.Any())
            {
                List<OrderListResponse> response = list.Select(item =>
                    _mapper.Map<OrderListResponse>(item)
                ).ToList();

                dataResult.Data = response.GroupBy(c => c.OrderId).Select(c=>new
                {
                    OrderId = c.Key,
                    Details = c
                });
            }

            return dataResult;
        }
    }
}