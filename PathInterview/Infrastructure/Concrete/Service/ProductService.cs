using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PathInterview.Core.Result;
using PathInterview.Entities.Dto.Product.Response;
using PathInterview.Entities.Entity;
using PathInterview.Infrastructure.Abstract.Query;
using PathInterview.Infrastructure.Abstract.Service;

namespace PathInterview.Infrastructure.Concrete.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductQuery _productQuery;
        private readonly IMapper _mapper;

        public ProductService(IProductQuery productQuery, IMapper mapper)
        {
            _productQuery = productQuery;
            _mapper = mapper;
        }

        public async Task<DataResult> GetProductListAsync()
        {
            DataResult dataResult = new();

            List<Product> list = await _productQuery.GetAll(c => c.IsStatus);

            if (list.Any())
            {
                List<ProductListResponse> response = list.Select(item =>
                    _mapper.Map<ProductListResponse>(item)
                ).ToList();

                dataResult.Data = response;
                dataResult.Total = response.Count;
            }

            return dataResult;
        }
    }
}