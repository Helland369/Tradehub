using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controller;

[ApiController]
[Route("api/get_all_shopping_cart")]
public class GetAllShoppingCart : ControllerBase
{
    private readonly TradehubDbContext _db;
    private readonly ICurrentUserService _uservice;

    public GetAllShoppingCart(TradehubDbContext db, ICurrentUserService uservice)
    {
        _db = db;
        _uservice = uservice;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        if (!_uservice.TryGetUserId(out var userId))
            return Unauthorized("Ivalid or missing user id in token");

        var user = await _db.Users.FirstOrDefaultAsync(u => u.ID == userId);
        if (user == null)
            return BadRequest("User not found");
    
        if (user.Cart == null ||  user.Cart.Count == 0)
            return Ok(new List<object>());
        
        var listingIds = user.Cart
            .Select(c => c.ListingID.ToString())
            .ToList();

        var listings = await _db.Listings
            .Where(l => listingIds.Contains(l.ID))
            .ToListAsync<Listing>();
        
        var listingDict = listings.ToDictionary(l => l.ID , l => l);
    
        var response = new List<object>();
        
        foreach (var cartItem in user.Cart)
        {
            var listing = listings.FirstOrDefault(l => l.ID == cartItem.ListingID.ToString());
            if (listing == null)
                continue;
            
            response.Add(new
            {
                id = listing.ID,
                quantity = cartItem.Quantity,
                title = listing.Title,
                buyPrice = listing.BuyPrice,
            });
        }
        
        return Ok(response);
    }
}
