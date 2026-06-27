
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
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<PromoterProfile> PromoterProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.User)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Conference)
                .WithMany(c => c.Registrations)
                .HasForeignKey(r => r.ConferenceId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.PromoterProfile)
                .WithOne(p => p.User)
                .HasForeignKey<PromoterProfile>(p => p.UserId);
        }
    }
}
