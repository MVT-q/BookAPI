using BookApi.DTOs;
using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if(user == null) 
                return NotFound();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(claim, out var currentUserId))
                return Unauthorized();

            var delete = await _userService.DeleteAsync(id, currentUserId);

            if(!delete)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/role")]
        public async Task<IActionResult> ChangeRole(int id, ChangeRoleDto dto)
        {
            var user = await _userService.ChangeRoleAsync(id, dto.Role);

            if(user == false)
                return NotFound();

            return NoContent();
        }
    }
}
