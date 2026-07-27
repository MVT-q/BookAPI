using BookApi.Models;

namespace BookApi.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string Email { get; set; } = "";

        public UserRole Role { get; set; }
    }
}
