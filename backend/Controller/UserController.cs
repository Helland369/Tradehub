using Backend.DTO.Users;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Backend.Controller;

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

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(string id, CancellationToken ct)
    {
        if (!ObjectId.TryParse(id, out var oid)) return BadRequest("invalid Id");
        var user = await _db.Users.FindAsync(new object[] { oid }, ct);
        if (user is null) return NotFound();
        return Ok(ToDto(user));
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest req, CancellationToken ct)
    {
        var role = string.IsNullOrWhiteSpace(req.Role) ? "user" : req.Role;

        var user = new User
        {
            ID = ObjectId.GenerateNewId(),
            Fname = req.Fname,
            Lname = req.Lname,
            Email = req.Email,
            UserName = req.UserName,
            Address = new Address
            {
                Street = req.Address.Street,
                City = req.Address.City ,
                Zip = req.Address.Zip,
                Country = req.Address.Country,
            },
            Phone = req.Phone,
            PasswordHash = _hasher.HashPassword(req.Password),
            Role = role,
            CreatedAt = DateTime.UtcNow,
        };

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync(ct);

        var dto = ToDto(user);
        return CreatedAtAction(nameof(GetById), new { id = dto.ID }, dto);
    }
}

