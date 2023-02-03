using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PathInterview.Core.DataAccess;
using PathInterview.Core.DataAccess.EntityFramework;
using PathInterview.DataAccess.Concrete;
using PathInterview.Entities.Entity;
using PathInterview.Infrastructure.Abstract.Query;

namespace PathInterview.Infrastructure.Concrete.Query
{
    public class AuthQuery : EfEntityRepositoryBase<User, ProjectDbContext>, IAuthQuery
    {
        public async Task<List<User>> GetAll(Expression<Func<User, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public User Get(Expression<Func<User, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public void Add(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }
    }
}