using System.ComponentModel.DataAnnotations;

namespace BookApi.DTOs
{
    public class UpdateBookDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = "";
    }
}
