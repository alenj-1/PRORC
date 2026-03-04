using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Context;
using Microsoft.EntityFrameworkCore;

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

        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
            => _dbSet.AnyAsync(filter);

        public virtual Task<List<TEntity>> GetAllAsync()
            => _dbSet.AsNoTracking().ToListAsync();

        public virtual Task<TEntity?> GetByIdAsync(TKey id)
            => _dbSet.FindAsync(id).AsTask();
    }
}
