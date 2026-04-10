using Microsoft.AspNetCore.Mvc;
using PRORC.Application.DTOs.Reservations;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/reservations")]

    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }


        // POST que permite crear una nueva reserva
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
        {
            var result = await _reservationService.CreateReservationAsync(request);
            return Ok(result);
        }


        // GET que permite buscar una reserva por Id
        [HttpGet("{reservationId:int}")]
        public async Task<IActionResult> GetById(int reservationId)
        {
            var result = await _reservationService.GetByIdAsync(reservationId);

            if (result == null)
                return NotFound(new { message = "Reservation not found." });

            return Ok(result);
        }


        // GET que permite obtener las reservas de un usuario
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var result = await _reservationService.GetByUserIdAsync(userId);
            return Ok(result);
        }


        // GET que permite obtener las reservas de un restaurante
        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<IActionResult> GetByRestaurantId(int restaurantId)
        {
            var result = await _reservationService.GetByRestaurantIdAsync(restaurantId);
            return Ok(result);
        }


        // PATCH que permite confirmar una reserva
        [HttpPatch("{reservationId:int}/confirm")]
        public async Task<IActionResult> Confirm(int reservationId)
        {
            await _reservationService.ConfirmReservationAsync(reservationId);
            return Ok(new { message = "Reservation confirmed successfully." });
        }


        // PATCH que permite completar una reserva
        [HttpPatch("{reservationId:int}/complete")]
        public async Task<IActionResult> Complete(int reservationId)
        {
            await _reservationService.CompleteReservationAsync(reservationId);
            return Ok(new { message = "Reservation completed successfully." });
        }


        // PATCH que permite cancelar una reserva
        [HttpPatch("{reservationId:int}/cancel")]
        public async Task<IActionResult> Cancel(int reservationId)
        {
            await _reservationService.CancelReservationAsync(reservationId);
            return Ok(new { message = "Reservation cancelled successfully." });
        }
    }
}