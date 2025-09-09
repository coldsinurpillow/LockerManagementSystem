using LockerManagementSystem.Data;
using LockerManagementSystem.Dtos;
using LockerManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LockerManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
    private readonly AppDbContext _db;
    public UsersController(AppDbContext db) => _db = db;

    // POST /api/users  — добавить юзера
    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> Create([FromBody] CreateUserDto dto) {
        var user = new User {
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Group = dto.Group,
            BarCode = dto.BarCode,
            Iin = dto.Iin
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id },
            new UserResponseDto(user.Id, user.FirstName, user.MiddleName, user.LastName,
                user.Group, user.BarCode, user.Iin, null, null));
    }

    // GET /api/users/{id} — инфо о юзере (включая активный локер/место)
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponseDto>> GetById(int id) {
        var user = await _db.Users.FindAsync(id);
        if (user is null)
            return NotFound();

        var active = await _db.LockerPlaces
            .Include(lp => lp.Locker)
            .FirstOrDefaultAsync(lp => lp.UserId == id);

        return Ok(new UserResponseDto(
            user.Id, user.FirstName, user.MiddleName, user.LastName,
            user.Group, user.BarCode, user.Iin,
            active?.PlaceIndex,
            active?.Locker.Number
        ));
    }

    // PUT /api/users/{id} — обновить данные
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto) {
        var user = await _db.Users.FindAsync(id);
        if (user is null)
            return NotFound();

        user.FirstName = dto.FirstName;
        user.MiddleName = dto.MiddleName;
        user.LastName = dto.LastName;
        user.Group = dto.Group;
        user.BarCode = dto.BarCode;
        user.Iin = dto.Iin;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/users/{id} — удалить юзера (и освободить место, если было)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) {
        var user = await _db.Users.FindAsync(id);
        if (user is null)
            return NotFound();

        var places = await _db.LockerPlaces.Where(lp => lp.UserId == id).ToListAsync();
        foreach (var p in places)
            p.UserId = null;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // (опционально) GET /api/users — список (для удобства)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> List() {
        var users = await _db.Users
            .OrderByDescending(u => u.Id)
            .Select(u => new UserResponseDto(
                u.Id, u.FirstName, u.MiddleName, u.LastName,
                u.Group, u.BarCode, u.Iin, null, null))
            .ToListAsync();

        return Ok(users);
    }
}
