using Microsoft.EntityFrameworkCore;
using UrlShorter.Models;

namespace UrlShorter.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<UrlEntry> UrlEntry { get; set; }
    }
}
