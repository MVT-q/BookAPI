using BookApi.DTOs;

namespace BookApi.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LoginResponseDto? Login(LoginDto request)
        {
            if (request.Username == "admin" && request.Password == "123456")
            {
                return new LoginResponseDto
                {
                    Token = "temporary-token"
                };
            }

            else
                return null;
        }
    }
}
