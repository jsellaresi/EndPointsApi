using Ardalis.Specification;

namespace LocationSearch.ApplicationCore.Interfaces
{
    public interface IRepository<T> : IReadRepositoryBase<T>, IRepositoryBase<T> where T : class
    {
    }
}
