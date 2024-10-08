using Credit.Domain.Entities.Common;
using Credit.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Credit.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : CommonEntity
    {
        private readonly MainDbContext _mainDbContext;
        protected readonly DbSet<T> _dbSet;

        protected GenericRepository(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
            _dbSet = _mainDbContext.Set<T>();
        }

        public async Task<T?> GetById(int id)
        {
            return await _dbSet.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id);
        }

        //get all
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.Where(x => x.IsDeleted == false).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> Get(
          Expression<Func<T, bool>>? filters = null,
          Expression<Func<T, object>>[]? includes = null,
          string[]? columns = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
          int? skip = null,
          int? take = null
        )
        {
            IQueryable<T> query = _mainDbContext.Set<T>();

            //add filter IsDeleted == false
            var isDeletedProperty = typeof(T).GetProperty("IsDeleted");
            if (isDeletedProperty != null)
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var property = Expression.Property(parameter, isDeletedProperty);
                var value = Expression.Constant(false);
                var body = Expression.Equal(property, value);
                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
                query = query.Where(lambda);
            }

            if (filters != null)
            {
                query = query.Where(filters);
            }

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (columns != null)
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var bindings = columns
                    .Select(name => Expression.PropertyOrField(parameter, name))
                    .Select(member => Expression.Bind(member.Member, member));
                var body = Expression.MemberInit(Expression.New(typeof(T)), bindings);
                var selector = Expression.Lambda<Func<T, T>>(body, parameter);
                query = query.Select(selector);
            }

            if (skip != null)
            {
                query = query.Skip(skip.Value);
            }

            if (take != null)
            {
                query = query.Take(take.Value);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _mainDbContext.Entry(entity).State = EntityState.Modified;

            //exclude IsDeleted and CreateAt from update
            _mainDbContext.Entry(entity).Property("IsDeleted").IsModified = false;
            _mainDbContext.Entry(entity).Property("CreatedAt").IsModified = false;
        }

        //soft delete
        public void DeleteSoft(int id)
        {
            var entity = _mainDbContext.Set<T>().Find(id);
            entity.GetType().GetProperty("IsDeleted")?.SetValue(entity, true);
            _mainDbContext.Set<T>().Update(entity);
        }
    }
}