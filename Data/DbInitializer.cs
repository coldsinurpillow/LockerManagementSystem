using LockerManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LockerManagementSystem.Data;

public static class DbInitializer {
    public static async Task MigrateAndSeedAsync(this IHost host) {
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync();

        if (!await db.Lockers.AnyAsync()) {
            // создадим 20 локеров по 2 места: 0001..0020
            var lockers = Enumerable.Range(1, 20).Select(i => new Locker {
                Number = i.ToString("0000"),
                PlaceCount = 2,
                Type = "key"
            }).ToList();

            db.Lockers.AddRange(lockers);
            await db.SaveChangesAsync();

            // генерим места 1..PlaceCount для каждого локера
            var places = new List<LockerPlace>();
            foreach (var l in lockers) {
                for (int p = 1; p <= l.PlaceCount; p++)
                    places.Add(new LockerPlace { LockerID = l.Id, PlaceIndex = p });
            }
            db.LockerPlaces.AddRange(places);
            await db.SaveChangesAsync();
        }
    }
}
