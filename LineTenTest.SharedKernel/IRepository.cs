using Ardalis.Specification;

namespace LineTenTest.SharedKernel
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class, IDbEntity
    {
    }
}