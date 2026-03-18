using Microsoft.AspNetCore.Mvc;
using PRORC.Application.DTOs.Users;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetByIdAsync(id);

            if (result is null)
                return NotFound(new { message = "User not found." });

            return Ok(result);
        }

        [HttpGet("search")]

        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            var result = await _userService.SearchByNameAsync(name);
            return Ok(result);
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            try
            {
                var result = await _userService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
