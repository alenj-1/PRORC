using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRORC.Api.Security;
using PRORC.Application.DTOs.Reservations;
using PRORC.Application.Interfaces;
using System.Security.Claims;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class ReservationsController(IReservationService reservationService) : ControllerBase
    {
        private readonly IReservationService _reservationService = reservationService;

        // POST que permite crear una nueva reserva
        [Authorize(Policy = AuthPolicies.CustomerOnly)]
        [HttpPost]
        public async Task<ActionResult<ReservationDto>> Create([FromBody] CreateReservationRequest request)
        {
            var reservation = await _reservationService.CreateAsync(request);
            return Ok(reservation);
        }


        // GET que permite consultar las reservas de un usuario
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<List<ReservationDto>>> GetByUser(int userId)
        {
            if (!CanAccessUserData(userId))
            {
                return Forbid();
            }

            var reservations = await _reservationService.GetByUserAsync(userId);
            return Ok(reservations);
        }


        // GET que permite consultar las reservas de un restaurante
        [Authorize(Policy = AuthPolicies.RestaurantOrSystemAdmin)]
        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<ActionResult<List<ReservationDto>>> GetByRestaurant(int restaurantId)
        {
            var reservations = await _reservationService.GetByRestaurantAsync(restaurantId);
            return Ok(reservations);
        }


        // Método para evitar que un usuario que no tenga rol de SystemAdmin consulte los datos de otro usuario
        private bool CanAccessUserData(int userId)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == "SystemAdmin")
            {
                return true;
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return false;
            }

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return false;
            }

            return currentUserId == userId;
        }
    }
}
