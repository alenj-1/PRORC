using Microsoft.AspNetCore.Mvc;
using PRORC.Application.DTOs.Users;
using PRORC.Application.Interfaces;
using PRORC.Domain.Enums;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/users")]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        // POST que permite crear un usuario
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateAsync(request);
            return Ok(result);
        }


        // GET que permite buscar un usuario por Id
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetById(int userId)
        {
            var result = await _userService.GetByIdAsync(userId);

            if (result == null)
                return NotFound(new { message = "User not found." });

            return Ok(result);
        }


        // GET que permite buscar un usuario por correo
        [HttpGet("by-email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var result = await _userService.GetByEmailAsync(email);

            if (result == null)
                return NotFound(new { message = "User not found." });

            return Ok(result);
        }


        // GET que permite obtener usuarios por rol
        [HttpGet("by-role")]
        public async Task<IActionResult> GetByRole([FromQuery] string role)
        {
            if (!Enum.TryParse<UserRole>(role, true, out var parsedRole))
                return BadRequest(new { message = "Invalid role value." });

            var result = await _userService.GetByRoleAsync(parsedRole);
            return Ok(result);
        }


        // PATCH que permite activar un usuario
        [HttpPatch("{userId:int}/activate")]
        public async Task<IActionResult> Activate(int userId)
        {
            await _userService.ActivateAsync(userId);
            return Ok(new { message = "User activated successfully." });
        }


        // PATCH que permite desactivar un usuario
        [HttpPatch("{userId:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int userId)
        {
            await _userService.DeactivateAsync(userId);
            return Ok(new { message = "User deactivated successfully." });
        }


        // PATCH que permite cambiar el rol de un usuario
        [HttpPatch("{userId:int}/role")]
        public async Task<IActionResult> ChangeRole(int userId, [FromBody] ChangeUserRoleApiRequest request)
        {
            if (!Enum.TryParse<UserRole>(request.NewRole, true, out var parsedRole))
                return BadRequest(new { message = "Invalid role value." });

            await _userService.ChangeRoleAsync(userId, parsedRole);
            return Ok(new { message = "User role changed successfully." });
        }


        // Clase simple para recibir datos desde el body
        public class ChangeUserRoleApiRequest
        {
            public string NewRole { get; set; } = string.Empty;
        }
    }
}