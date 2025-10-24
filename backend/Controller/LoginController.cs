namespace Backend.Controller;
using Backend.Models;
using Backend.Services;
using Backend.DTO.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MongoDB.Driver;

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

    [HttpPost("/login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginUserRequest req)
    {
        //var filet = Builders<User>.Filter.Or(Builders<User>.Filter.Eq(u => u.UserName, req.UserName));

        var user = await _db.FindAsync<User>(req.UserName);
        if (user is null)
            return Unauthorized(new { error = "Invalid credentials" });

        var password = _hasher.ChechPasswordHash(req.Password, user.PasswordHash);
        if (!password)
            return Unauthorized(new { error = "Invalid credentials" });

        var token = JwtTokenFactory.CreateToken(user, _config);
        return token;
    }
}
