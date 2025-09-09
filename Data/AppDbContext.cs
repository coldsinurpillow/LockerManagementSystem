using LockerManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LockerManagementSystem.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Locker> Lockers => Set<Locker>();
    public DbSet<LockerPlace> LockerPlaces => Set<LockerPlace>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.FirstName).HasMaxLength(100);
            e.Property(x => x.MiddleName).HasMaxLength(100);
            e.Property(x => x.LastName).HasMaxLength(100);
            e.Property(x => x.Group).HasMaxLength(100);
            e.Property(x => x.BarCode).HasMaxLength(100);
            e.Property(x => x.Iin).HasMaxLength(12);
        });

        modelBuilder.Entity<Locker>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Number).IsRequired().HasMaxLength(32);
            e.HasIndex(x => x.Number).IsUnique();
            e.Property(x => x.PlaceCount).HasDefaultValue(1);
            e.Property(x => x.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<LockerPlace>(e => {
            e.HasKey(x => x.Id);

            e.Property(x => x.PlaceIndex).IsRequired();

            e.HasIndex(x => new { x.LockerId, x.PlaceIndex }).IsUnique();

            e.HasOne(x => x.Locker)
                .WithMany(l => l.Places)
                .HasForeignKey(x => x.LockerId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.User)
                .WithMany(u => u.LockerPlaces)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
