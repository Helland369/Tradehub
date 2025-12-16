using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controller;

[ApiController]
[Route("api/buy_shopping_cart")]
public class BuyShoppingCartController : ControllerBase
{
    private readonly TradehubDbContext _db;
    private readonly ICurrentUserService _uservice;

    public BuyShoppingCartController(TradehubDbContext db, ICurrentUserService uservice)
    {
        _db = db;
        _uservice = uservice;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> BuyShoppingCart(CancellationToken ct)
    {
        if (_uservice.TryGetUserId(out var userId))
            return Unauthorized("Invalid or missing user id in token");

        var user = _db.Users.FirstOrDefault(u => u.ID == userId);
        if (user == null)
            return NotFound("user not found");

        var listingIdstr = user.Cart.Select(c => c.ListingID.ToString()).ToList();
        var listings = _db.Listings.Where(l => listingIdstr.Contains(l.ID)).ToList();

        var listingsDict = listings.ToDictionary(l => l.ID, l => l);

        double totPrice = 0;

        foreach (var c in user.Cart)
        {
            var idStr = c.ListingID.ToString();
            if (!listingsDict.TryGetValue(idStr, out var listing))
                continue;

            if (listing.BuyPrice == null)
                continue;

            totPrice += listing.BuyPrice.Value * c.Quantity;
        }

        if (totPrice > user.Points)
            return BadRequest("Insufficient funds in wallet!");

        user.Points -= (int)totPrice;

        foreach (var c in user.Cart)
            user.Purchases.Add(c.ListingID);

        user.Cart.Clear();

        await _db.SaveChangesAsync(ct);

        return Ok(new
        {
            message = $"Purchuce complete! Total spent {totPrice} points."
        });
    }
}
