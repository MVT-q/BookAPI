using BookApi.DTOs;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponseDto?> Login(LoginDto request)
        {
            var result = _authService.Login(request);

            if (result == null)
                return Unauthorized();

            return Ok(result);
        }
    }
}
