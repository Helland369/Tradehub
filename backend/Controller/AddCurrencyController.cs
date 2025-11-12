using System.Security.Claims;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Backend.Controller;

[ApiController]
[Route("api/addcurrency")]
public class AddCurrencyController : ControllerBase
{
    private readonly TradehubDbContext _db;
    
    public AddCurrencyController(TradehubDbContext db)
    {
        _db = db;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddCurrency([FromForm]int amount, CancellationToken ct)
    {
        var claimsIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(claimsIdentity) || !ObjectId.TryParse(claimsIdentity, out var userId))
            return Unauthorized("Invalid or missing user id");

        var user = _db.Users.FirstOrDefault(u => u.ID == userId);
        if (user == null)
            return NotFound("user not found");
        
        user.Points += amount;
        await _db.SaveChangesAsync(ct);
        return Ok();
    }
}