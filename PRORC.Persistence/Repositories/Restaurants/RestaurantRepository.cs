using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRORC.Domain.Entities.Restaurants;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;

namespace PRORC.Persistence.Repositories.Restaurants
{
    public class RestaurantRepository : BaseRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(PRORCContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory) { }

        // Obtiene todos los restaurantes de un propietario
        public async Task<IEnumerable<Restaurant>> GetByOwnerIdAsync(int ownerId)
        {
            try
            {
                return await _context.Restaurants
                    .AsNoTracking()
                    .Where(r => r.OwnerId == ownerId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting restaurants by OwnerId {OwnerId}.", ownerId);
                throw;
            }
        }

        // Busca restaurantes activos por filtros opcionales
        public async Task<IEnumerable<Restaurant>> SearchAsync(string? cuisineType, string? address, double? minimumRating)
        {
            try
            {
                // Empieza con una consulta base de restaurantes activos
                var query = _context.Restaurants
                    .AsNoTracking()
                    .Where(r => r.IsActive)
                    .AsQueryable();

                // Si se indicó el tipo de comida, se filtra
                if (!string.IsNullOrWhiteSpace(cuisineType))
                {
                    query = query.Where(r => r.CuisineType.Contains(cuisineType));
                }

                // Si se indicó la dirección, se filtra
                if (!string.IsNullOrWhiteSpace(address))
                {
                    query = query.Where(r => r.Address.Contains(address));
                }

                // Si se indicó el rating mínimo, también se filtra
                if (minimumRating.HasValue)
                {
                    query = query.Where(r => r.Rating >= minimumRating.Value);
                }

                // Devuelve los resultados ordenados por rating descendente
                return await query
                    .OrderByDescending(r => r.Rating)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching restaurants.");
                throw;
            }
        }

        // Obtiene todos los bloques de disponibilidad de un restaurante
        public async Task<IEnumerable<RestaurantAvailability>> GetAvailabilitiesByRestaurantIdAsync(int restaurantId)
        {
            try
            {
                return await _context.RestaurantAvailabilities
                    .AsNoTracking()
                    .Where(a => a.RestaurantId == restaurantId)
                    .OrderBy(a => a.AvailableDate)
                    .ThenBy(a => a.StartTime)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting availabilities by RestaurantId {RestaurantId}.", restaurantId);
                throw;
            }
        }

        // Busca el bloque de disponibilidad que corresponde a una fecha y hora
        public async Task<RestaurantAvailability?> GetAvailabilitySlotAsync(int restaurantId, DateTime availableDate, TimeSpan reservationTime)
        {
            try
            {
                var onlyDate = availableDate.Date;

                return await _context.RestaurantAvailabilities
                    .FirstOrDefaultAsync(a =>
                        a.RestaurantId == restaurantId &&
                        a.AvailableDate.Date == onlyDate &&
                        a.StartTime <= reservationTime &&
                        a.EndTime > reservationTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting availability slot for RestaurantId {RestaurantId}.", restaurantId);
                throw;
            }
        }

        // Agrega un nuevo bloque de disponibilidad
        public async Task<RestaurantAvailability> AddAvailabilityAsync(RestaurantAvailability availability)
        {
            try
            {
                await _context.RestaurantAvailabilities.AddAsync(availability);
                await _context.SaveChangesAsync();

                _logger.LogInformation("RestaurantAvailability added successfully.");

                return availability;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding RestaurantAvailability.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding RestaurantAvailability.");
                throw;
            }
        }

        // Actualiza un bloque de disponibilidad existente
        public async Task UpdateAvailabilityAsync(RestaurantAvailability availability)
        {
            try
            {
                _context.RestaurantAvailabilities.Update(availability);
                await _context.SaveChangesAsync();

                _logger.LogInformation("RestaurantAvailability updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating RestaurantAvailability.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating RestaurantAvailability.");
                throw;
            }
        }
    }
}