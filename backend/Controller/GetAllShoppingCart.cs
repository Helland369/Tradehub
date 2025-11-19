using System.Security.Claims;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Backend.Controller;

[ApiController]
[Route("api/get_all_shopping_cart")]
public class GetAllShoppingCart : ControllerBase
{
    private readonly TradehubDbContext _db;
    
    public GetAllShoppingCart(TradehubDbContext db)
    {
        _db = db;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var claimsIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(claimsIdentity) || !ObjectId.TryParse(claimsIdentity, out var userId))
            return BadRequest("Invalid user id");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.ID == userId);
        if (user == null)
            return BadRequest("User not found");

        var cartIds = user.Cart
            .Select(id => id.ToString())
            .ToList();
        
        var cart = await _db.Listings
            .Where(l => cartIds.Contains(l.ID))
            .ToListAsync();
        
        return Ok(cart);
    }
}