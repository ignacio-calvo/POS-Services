using Microsoft.EntityFrameworkCore;
using POS.Products.Data.Interfaces;
using POS.Products.Data.Models;

namespace POS.Products.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ProductDbContext _databaseContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ProductDbContext context)
        {
            _databaseContext = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            var result = await _dbSet.AddAsync(entity);
            await SaveAsync();
            return result.Entity;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);

            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);
                await SaveAsync();
            }
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool tracked = true)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var result = _dbSet.Update(entity);
            await SaveAsync();
            return result.Entity;
        }

        public async Task SaveAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
