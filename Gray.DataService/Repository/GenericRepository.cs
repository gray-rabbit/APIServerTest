using Gray.DataService.Data;
using Gray.DataService.IRepository;
using Gray.Entities.DbSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gray.DataService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext context;

        protected ILogger Logger { get; }

        internal DbSet<T> dbSet;

        public GenericRepository(AppDbContext context, ILogger logger)
        {
            this.context = context;
            Logger = logger;
            dbSet = context.Set<T>();
        }

        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await dbSet.ToListAsync<T>();
        }

        public Task<bool> Delete(Guid id, string userId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Edit(Guid id, T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await dbSet.FindAsync(id);
        }
    }
}
