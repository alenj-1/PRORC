using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity, TKey> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteEntityAsync(TEntity entity);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null);
    }
}
