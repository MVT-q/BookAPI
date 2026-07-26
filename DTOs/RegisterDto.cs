using System.ComponentModel.DataAnnotations;

namespace BookApi.DTOs
{
    public class RegisterDto
    {
        [Required]
        [MinLength(4)]
        [StringLength(16)]
        public string Username { get; set; } = "";

        [Required]
        [MinLength(6)]
        [StringLength(16)]
        public string Password { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
