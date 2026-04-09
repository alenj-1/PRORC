using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRORC.Persistence.Context;

namespace PRORC.Persistence.Base
{
    public class BaseRepository<TEntity> where TEntity : class
    {
        protected readonly PRORCContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly ILogger _logger;

        public BaseRepository(PRORCContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
            _logger = loggerFactory.CreateLogger(GetType());
        }

        // Método para obtener una entidad por su Id
        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting {EntityName} with Id {Id}.", typeof(TEntity).Name, id);
                throw;
            }
        }

        // Método para obtener todas las entidades
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _dbSet.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all records of {EntityName}.", typeof(TEntity).Name);
                throw;
            }
        }

        // Método para agregar una nueva entidad
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("{EntityName} added successfully.", typeof(TEntity).Name);

                return entity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding {EntityName}.", typeof(TEntity).Name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding {EntityName}.", typeof(TEntity).Name);
                throw;
            }
        }

        // Método para actualizar una entidad
        public virtual async Task UpdateAsync(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("{EntityName} updated successfully.", typeof(TEntity).Name);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating {EntityName}.", typeof(TEntity).Name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating {EntityName}.", typeof(TEntity).Name);
                throw;
            }
        }

        // Método para eliminar una entidad por Id
        public virtual async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);

                if (entity == null)
                {
                    _logger.LogWarning("{EntityName} with Id {Id} was not found for deletion.", typeof(TEntity).Name, id);
                    throw new KeyNotFoundException($"{typeof(TEntity).Name} with Id {id} was not found.");
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("{EntityName} with Id {Id} deleted successfully.", typeof(TEntity).Name, id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while deleting {EntityName} with Id {Id}.", typeof(TEntity).Name, id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting {EntityName} with Id {Id}.", typeof(TEntity).Name, id);
                throw;
            }
        }
    }
}