using System.ComponentModel.DataAnnotations;

namespace WhisperingPages.Models
{
    public class BookDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Required, MaxLength(100)]
        public string Author { get; set; } = "";

        [Required, MaxLength(100)]
        public string Genre { get; set; } = "";

        [Required]
        public decimal Price { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
