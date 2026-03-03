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
    public abstract class BaseRepository<TEntity, TType> : IBaseRepository<TEntity, TType> where TEntity : class
    {
        private readonly PRORCContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected BaseRepository(PRORCContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public abstract Task AddAsync(TEntity entity);
        public abstract Task DeleteEntityAsync(TEntity entity);
        public abstract Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
        public abstract Task<List<TEntity>> GetAllAsync();
        public abstract Task<TEntity?> GetByIdAsync(TType id);
        public abstract Task UpdateAsync(TEntity entity);
    }
}
