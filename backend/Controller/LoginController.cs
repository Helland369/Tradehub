using Backend.DTO.Users;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controller;

[Route("/api/auth")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly TradehubDbContext _db;
    private readonly PasswordHasher _hasher;
    private readonly IConfiguration _config;

    public LoginController(TradehubDbContext db, PasswordHasher hasher, IConfiguration config)
    {
        _db = db;
        _hasher = hasher;
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest req)
    {
        try
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == req.UserName);
            if (user is null)
                return Unauthorized(new { error = "Invalid credentials" });
            
            var password = _hasher.CheckPasswordHash(req.Password, user.PasswordHash);
            if (!password)
                return Unauthorized(new { error = "Invalid credentials" });
            
            var token = JwtTokenFactory.CreateToken(user, _config);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
