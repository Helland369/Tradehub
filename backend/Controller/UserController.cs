namespace Backend.Controller;
using Backend.Models;
using Backend.Services;
using Backend.DTO.Users;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

[Route("/api/users")]
[ApiController]
public class UserController : ControllerBase
{

    private readonly TradehubDbContext _db;
    private readonly PasswordHasher _hasher;

    public UserController(TradehubDbContext db, PasswordHasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    private static UserDto ToDto(User u) =>
        new(u.ID.ToString(), u.Fname, u.Lname, u.Email, u.Role, u.CreatedAt);

    public async Task<ActionResult<UserDto>> GetById(string id, CancellationToken ct)
    {
        if (!ObjectId.TryParse(id, out var oid)) return BadRequest("invalid Id");
        var user = await _db.Users.FindAsync(new object[] { oid }, ct);
        if (user is null) return NotFound();
        return Ok(ToDto(user));
    }

    [HttpPost("/create_user")]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest req, CancellationToken ct)
    {
        var user = new User
        {
            ID = ObjectId.GenerateNewId(),
            Fname = req.Fname,
            Lname = req.Lname,
            Email = req.Email ?? "",
            PasswordHash = _hasher.HashPassword(req.Password),
            CreatedAt = DateTime.UtcNow,
            Role = "User",
        };

        await _db.Users.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);

        var dto = ToDto(user);
        return CreatedAtAction(nameof(GetById), new { id = dto.ID }, dto);
    }
}
