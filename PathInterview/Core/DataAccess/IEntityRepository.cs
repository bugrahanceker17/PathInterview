using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PathInterview.Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, new()
    {
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null);
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task<int> Add(T entity);
        Task<int> BulkAdd(List<T> entity);
        
        Task<int> Update(T entity);
        Task<int> Delete(T entity);
    }
}