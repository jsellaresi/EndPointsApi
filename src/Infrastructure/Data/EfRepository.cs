using Ardalis.Specification.EntityFrameworkCore;
using LocationSearch.ApplicationCore.Interfaces;

namespace LocationSearch.Infrastructure.Data
{
    public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
    {
        public EfRepository(LocationContext dbContext) : base(dbContext)
        {
        }
    }
}
