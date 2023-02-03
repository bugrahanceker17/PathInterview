using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PathInterview.Core.Extensions;
using PathInterview.Core.Result;
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

        public OrderService(IOrderQuery orderQuery, IBasketQuery basketQuery, IHttpContextAccessor httpContextAccessor)
        {
            _orderQuery = orderQuery;
            _basketQuery = basketQuery;
            _httpContextAccessor = httpContextAccessor;
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

            List<Order> bulkInsertModel = new List<Order>();

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

            dataResult.Data = "Siparişiniz alındı";
            return dataResult;
        }
    }
}

