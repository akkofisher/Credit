using System.Linq.Expressions;

namespace Credit.Infrastructure.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        //query
        Task<T?> GetById(int id);

        Task<IEnumerable<T>> GetAll();

        Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>>? filters = null,
            Expression<Func<T, object>>[]? includes = null,
            string[]? columns = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? skip = null,
            int? take = null
        );

        //command
        Task Add(T entity);

        void Update(T entity);
        void DeleteSoft(int id);
    }
}