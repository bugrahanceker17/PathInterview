using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PathInterview.Core.Extensions;
using PathInterview.Core.Result;
using PathInterview.Entities.Dto.Order.Request;
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
        private readonly IDistributedCache _distributedCache;
        private readonly IMapper _mapper;

        public OrderService(IOrderQuery orderQuery, IBasketQuery basketQuery, IHttpContextAccessor httpContextAccessor, IMapper mapper, IDistributedCache distributedCache)
        {
            _orderQuery = orderQuery;
            _basketQuery = basketQuery;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _distributedCache = distributedCache;
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

                int execute = await _orderQuery.Add(entity);
                
                if (execute <= 0)
                {
                    dataResult.ErrorMessageList.Add("Sipariş verilemedi");
                    return dataResult;
                }

                // bulkInsertModel.Add(entity);
            }

            // int execute = await _orderQuery.BulkAdd(bulkInsertModel);

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

            string cacheName = $"orderList-{userId}";

            string cache = await _distributedCache.GetStringAsync(cacheName);
            List<OrderListResponse> list = new List<OrderListResponse>();

            if (!string.IsNullOrEmpty(cache))
            {
                list = JsonConvert.DeserializeObject<List<OrderListResponse>>(Convert.ToString(cache) ?? "");
            }
            else
            {
                list = await _orderQuery.GetOrders(page, pageSize, userId);

                await _distributedCache.SetStringAsync(cacheName, JsonConvert.SerializeObject(list),
                    new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
            }

            if (list.Any())
            {
                List<OrderListResponse> response = list.Select(item =>
                    _mapper.Map<OrderListResponse>(item)
                ).ToList();

                dataResult.Data = response.GroupBy(c => c.OrderId).Select(c => new
                {
                    OrderId = c.Key,
                    Details = c
                });
            }

            return dataResult;
        }

        public async Task<DataResult> CancelOrderAsync(string orderId, int productId)
        {
            DataResult dataResult = new();

            if (string.IsNullOrEmpty(orderId) || productId <= 0)
            {
                dataResult.ErrorMessageList.Add("Model hatalı");
                return dataResult;
            }

            (bool login, string message) = _httpContextAccessor.LoginExists();

            if (!login)
            {
                dataResult.ErrorMessageList.Add(message);
                return dataResult;
            }

            string userId = _httpContextAccessor.AccessToken().userId;

            OrderListResponse order = await _orderQuery.DetailOrder(orderId, productId, userId);

            if (order is null)
            {
                dataResult.ErrorMessageList.Add("Sipariş içindeki ürün bulunamadı");
                return dataResult;
            }

            string info = string.Empty;

            Order orderUpdateModel = await _orderQuery.Get(c => c.Id.Equals(order.Id));

            if (order.CategoryId.Equals(1))
            {
                orderUpdateModel.IsCancellationConfirmed = true;
                orderUpdateModel.DeliveryStatus = (short)DeliveryStatus.IPTAL_EDILDI;

                info = "Sipariş iptal gerçekleşti";
            }
            else
            {
                orderUpdateModel.IsCanceledRequest = true;
                info = "Sipariş iptali için onaya gönderildi";
            }

            int execute = await _orderQuery.Update(orderUpdateModel);

            if (execute > 0)
            {
                dataResult.Data = info;
                return dataResult;
            }

            dataResult.ErrorMessageList.Add("Sipariş iptal işlemi başarısız");
            return dataResult;
        }

        public async Task<DataResult> OrderCancelRequestAsync()
        {
            DataResult dataResult = new();

            List<Order> list = await _orderQuery.GetAll(c => c.IsCanceledRequest && !c.IsCancellationConfirmed && c.IsStatus);

            if (list.Any())
            {
                List<OrderListResponse> response = list.Select(item =>
                    _mapper.Map<OrderListResponse>(item)
                ).ToList();

                dataResult.Data = response;
            }

            return dataResult;
        }

        public async Task<DataResult> ConfirmRequestAsync(ConfirmCancelRequest model)
        {
            DataResult dataResult = new();

            if (model.Id <= 0)
            {
                dataResult.ErrorMessageList.Add("Sipariş bilgisi hatalı");
                return dataResult;
            }

            Order order = await _orderQuery.Get(c => c.Id.Equals(model.Id) && c.IsCanceledRequest);

            if (order is null)
            {
                dataResult.ErrorMessageList.Add("Sipariş bulunamadı");
                return dataResult;
            }

            if (model.IsConfirm)
            {
                order.IsCancellationConfirmed = true;
                order.IsStatus = false;
            }
            else
            {
                order.IsCanceledRequest = false;
            }

            int execute = await _orderQuery.Update(order);

            if (execute > 0)
            {
                dataResult.Data = "İşlem başarılı";
                return dataResult;
            }

            dataResult.ErrorMessageList.Add("Onaylama/Reddetme sırasında hata oluştu");
            return dataResult;
        }
    }
}