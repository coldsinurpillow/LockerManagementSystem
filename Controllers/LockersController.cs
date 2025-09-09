using LockerManagementSystem.Data;
using LockerManagementSystem.Dtos;
using LockerManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LockerManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LockersController : ControllerBase {
    private readonly AppDbContext _db;
    public LockersController(AppDbContext db) => _db = db;

    //POST /api/lockers
    [HttpPost]
    public async Task<ActionResult> AddLocker([FromBody] AddLockerDto dto) {
        if (dto.PlaceCount <= 0)
            return BadRequest("place_count must be >= 1");

        var locker = new Locker {
            Number = dto.Number,
            PlaceCount = dto.PlaceCount,
            Type = dto.Type
        };
        _db.Lockers.Add(locker);
        await _db.SaveChangesAsync();

        // генерируем места 1..PlaceCount
        var places = Enumerable.Range(1, dto.PlaceCount)
            .Select(i => new LockerPlace { LockerId = locker.Id, PlaceIndex = i });
        _db.LockerPlaces.AddRange(places);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLockerInfo), new { number = locker.Number },
            new { locker.Number, locker.PlaceCount, locker.Type });
    }

    // GET /api/lockers/{number}
    [HttpGet("{number}")]
    public async Task<ActionResult> GetLockerInfo(string number) {
        var locker = await _db.Lockers.AsNoTracking()
            .FirstOrDefaultAsync(l => l.Number == number);
        if (locker is null)
            return NotFound();

        var places = await _db.LockerPlaces.AsNoTracking()
            .Where(p => p.LockerId == locker.Id)
            .Include(p => p.User)
            .OrderBy(p => p.PlaceIndex)
            .Select(p => new {
                place = p.PlaceIndex,
                user_id = p.UserId,
                full_name = p.User == null ? null : $"{p.User.LastName} {p.User.FirstName}",
                group = p.User.Group,
                bar_code = p.User.BarCode
            })
            .ToListAsync();

        return Ok(new { number = locker.Number, places });
    }

    // POST /api/lockers/assign
    [HttpPost("assign")]
    public async Task<ActionResult> Assign([FromBody] AssignDto dto) {
        var locker = await _db.Lockers.FirstOrDefaultAsync(l => l.Number == dto.Number);
        if (locker is null)
            return NotFound("Locker not found");

        LockerPlace? place;
        if (dto.Place.HasValue) {
            place = await _db.LockerPlaces
                .FirstOrDefaultAsync(p => p.LockerId == locker.Id && p.PlaceIndex == dto.Place.Value);
            if (place is null)
                return NotFound("Place not found");
            if (place.UserId != null)
                return Conflict("Place already occupied");
        } else {
            place = await _db.LockerPlaces
                .Where(p => p.LockerId == locker.Id && p.UserId == null)
                .OrderBy(p => p.PlaceIndex)
                .FirstOrDefaultAsync();
            if (place is null)
                return Conflict("No free places in locker");
        }

        // снимаем старые назначения у пользователя
        var current = await _db.LockerPlaces.Where(p => p.UserId == dto.UserId).ToListAsync();
        foreach (var c in current)
            c.UserId = null;

        place.UserId = dto.UserId;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // PUT /api/lockers/reassign
    [HttpPut("reassign")]
    public Task<ActionResult> Reassign([FromBody] AssignDto dto) => Assign(dto);

    // DELETE /api/lockers/assigned
    [HttpDelete("assigned")]
    public async Task<ActionResult> RemoveAssigned([FromBody] RemoveAssignedDto dto) {
        var locker = await _db.Lockers.FirstOrDefaultAsync(l => l.Number == dto.Number);
        if (locker is null)
            return NotFound("Locker not found");

        var place = await _db.LockerPlaces
            .FirstOrDefaultAsync(p => p.LockerId == locker.Id && p.PlaceIndex == dto.Place);
        if (place is null)
            return NotFound("Place not found");

        place.UserId = null;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // POST /api/lockers/distribute
    [HttpPost("distribute")]
    public async Task<ActionResult> Distribute([FromBody] DistributeDto dto) {
        if (dto.Count <= 0)
            return BadRequest("Count must be > 0");

        var freeUsers = await _db.Users
            .Where(u => !_db.LockerPlaces.Any(lp => lp.UserId == u.Id))
            .OrderByDescending(u => u.Id) 
            .Take(dto.Count)
            .ToListAsync();

        var freePlaces = await _db.LockerPlaces
            .Where(p => p.UserId == null)
            .OrderBy(p => p.Locker.Number)
            .ThenBy(p => p.PlaceIndex)
            .Take(freeUsers.Count)
            .ToListAsync();

        var n = Math.Min(freeUsers.Count, freePlaces.Count);
        for (int i = 0; i < n; i++)
            freePlaces[i].UserId = freeUsers[i].Id;

        await _db.SaveChangesAsync();
        return Ok(new { requested = dto.Count, assigned = n });
    }
}
