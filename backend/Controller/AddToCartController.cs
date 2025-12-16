using Backend.DTO.Users;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Backend.Controller;

[ApiController]
[Route("api/add_to_cart")]
public class AddToCartController : ControllerBase
{
    private readonly TradehubDbContext _db;
    private readonly ICurrentUserService _uservice;

    public AddToCartController(TradehubDbContext db, ICurrentUserService uservice)
    {
        _db = db;
        _uservice = uservice;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToCart([FromBody] AddToCart dto, CancellationToken ct)
    {
        if (!_uservice.TryGetUserId(out var userId))
            return Unauthorized("Invalid or missing user id in token");

        if (!ObjectId.TryParse(dto.ItemId, out var itemId))
            return BadRequest("Invalid item id");
        
        var user = _db.Users.FirstOrDefault(u => u.ID == userId);
        if (user == null)
            return NotFound("User not found");
        
        var existiing = user.Cart.FirstOrDefault(u => u.ListingID == itemId);
        if (existiing is null)
        {
            user.Cart.Add(new CartItems
            {
                ListingID = itemId,
                Quantity = dto.Quantity > 0 ? dto.Quantity : 1,
            });
        }
        else
        {
            existiing.Quantity += dto.Quantity > 0 ? dto.Quantity : 1;
        }
        
        //user.Cart.Add(item);
        await _db.SaveChangesAsync(ct);
        return Ok();
    }
}
