using Microsoft.EntityFrameworkCore;
using LockerManagementSystem.Models;

namespace LockerManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Locker> Lockers { get; set; }
        public DbSet<LockerAssignment> LockerAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Locker>()
                .HasKey(l => l.Number);

            modelBuilder.Entity<LockerAssignment>()
                .HasOne(x => x.User)
                .WithMany(u => u.Assignments)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<LockerAssignment>()
                .HasOne(x => x.Locker)
                .WithMany(l => l.Assignments)
                .HasForeignKey(x => x.LockerNumber);
        }
    }
}

