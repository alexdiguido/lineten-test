using Ardalis.Specification.EntityFrameworkCore;
using LineTenTest.SharedKernel;

namespace LineTenTest.Infrastructure
{
    public class EfRepository<T> : RepositoryBase<T>, IRepository<T>
        where T : class, IDbEntity
    {
        public EfRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}