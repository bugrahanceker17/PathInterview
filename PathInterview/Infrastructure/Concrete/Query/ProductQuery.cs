using PathInterview.Core.DataAccess.EntityFramework;
using PathInterview.DataAccess.Concrete;
using PathInterview.Entities.Entity;
using PathInterview.Infrastructure.Abstract.Query;

namespace PathInterview.Infrastructure.Concrete.Query
{
    public class ProductQuery : EfEntityRepositoryBase<Product, ProjectDbContext>, IProductQuery
    {
    
    } 
}

