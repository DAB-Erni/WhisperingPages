using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WhisperingPages.Models
{
    public class Book
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        [MaxLength(100)]
        public string Author { get; set; } = "";

        [MaxLength(100)]
        public string Genre { get; set; } = "";

        [Precision(16, 2)]
        public decimal Price { get; set; }

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";
    }
}
