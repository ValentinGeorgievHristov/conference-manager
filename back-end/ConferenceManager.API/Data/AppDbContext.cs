
namespace ConferenceManager.API.Data
{
    using Microsoft.EntityFrameworkCore;
    using ConferenceManager.API.Models;
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Conference> Conferences { get; set; }
    }
}
