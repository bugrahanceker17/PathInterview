using PathInterview.Core.DataAccess;
using PathInterview.Entities.Entity;

namespace PathInterview.Infrastructure.Abstract.Query
{
    public interface IAuthQuery : IEntityRepository<User>
    {
    }
}

