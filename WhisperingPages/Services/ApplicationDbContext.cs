using Microsoft.EntityFrameworkCore;
using WhisperingPages.Models;

namespace WhisperingPages.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
    }
}
