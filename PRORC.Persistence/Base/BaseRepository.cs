using Microsoft.EntityFrameworkCore;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Context;
using System.Linq.Expressions;

namespace PRORC.Persistence.Base
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly PRORCContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(PRORCContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteEntityAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }

        public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(filter)
                .ToListAsync();
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(filter);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            if (filter is null)
                return await _dbSet.CountAsync();

            return await _dbSet.CountAsync(filter);
        }
    }
}
