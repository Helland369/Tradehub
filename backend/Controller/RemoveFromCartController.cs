using System.Security.Claims;
using Backend.DTO.Users;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Backend.Controller;

[ApiController]
[Route("api/remove_from_cart")]
public class RemoveFromCartController : ControllerBase
{
    private readonly TradehubDbContext _db;

    public RemoveFromCartController(TradehubDbContext db)
    {
        _db = db;
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> RemoveFromCart([FromBody] RemoveFromCart dto, CancellationToken ct)
    {
        var claimsIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(claimsIdentity) || !ObjectId.TryParse(claimsIdentity, out var userId))
            return Unauthorized("Invalid user id");

        if (!ObjectId.TryParse(dto.ItemId, out var itmeId))
            return BadRequest("Invalid item id");
        
        var user = _db.Users.FirstOrDefault(u => u.ID == userId);
        if (user == null)
            return BadRequest("User not found");

        var cartItem = user.Cart.FirstOrDefault(c => c.ListingID == itmeId);
        if (cartItem == null)
            return NotFound("Cart item not found");
        
        var removeQty = dto.Quantity <=  0 ? 1 : dto.Quantity;

        if (cartItem.Quantity > removeQty)
        {
            user.Cart.Remove(cartItem);
        }
        else
        {
            cartItem.Quantity -= removeQty;
        }
        
        await _db.SaveChangesAsync(ct);
        return Ok();
    }
}