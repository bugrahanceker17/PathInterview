using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using PathInterview.Core.Extensions;
using PathInterview.Core.Result;
using PathInterview.Entities.Dto.Basket.Request;
using PathInterview.Entities.Dto.Basket.Response;
using PathInterview.Entities.Entity;
using PathInterview.Infrastructure.Abstract.Query;
using PathInterview.Infrastructure.Abstract.Service;

namespace PathInterview.Infrastructure.Concrete.Service
{
    public class BasketService : IBasketService
    {
        private readonly IBasketQuery _basketQuery;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public BasketService(IBasketQuery basketQuery, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _basketQuery = basketQuery;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DataResult> AddBasketAsync(AddBasketRequest model)
        {
            DataResult dataResult = new();

            if (model.ProductId <= 0 || model.Quantity <= 0)
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

            Basket entity = _mapper.Map<Basket>(model);

            entity.UserId = userId;
            entity.IsDeleted = false;
            entity.IsStatus = true;
            entity.CreatedAt = DateTime.Now;

            int execute = await _basketQuery.Add(entity);

            if (execute > 0)
            {
                dataResult.Data = "Sepete Eklendi";
                return dataResult;
            }

            dataResult.ErrorMessageList.Add("İşlem başarısız");
            return dataResult;
        }

        public async Task<DataResult> GetBasketAsync()
        {
            DataResult dataResult = new();

            (bool login, string message) = _httpContextAccessor.LoginExists();

            if (!login)
            {
                dataResult.ErrorMessageList.Add(message);
                return dataResult;
            }

            string userId = _httpContextAccessor.AccessToken().userId;

            List<Basket> list = await _basketQuery.GetAll(c => c.UserId.Equals(userId) && c.IsStatus);

            if (list.Any())
            {
                List<BasketListResponse> response = list.Select(item =>
                    _mapper.Map<BasketListResponse>(item)
                ).ToList();

                dataResult.Data = response;
                dataResult.Total = response.Count;
            }

            return dataResult;
        }
    }
}