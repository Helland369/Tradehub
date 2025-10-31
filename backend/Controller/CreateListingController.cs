using System.Security.Claims;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Backend.Controller;

[Route("/api/create_listing")]
[ApiController]
public class CreateListingController : ControllerBase
{
    private readonly TradehubDbContext _db;
    
    public CreateListingController(TradehubDbContext db)
    {
        _db = db;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Listing>>> Listing([FromBody]Listing req, CancellationToken ct)
    {
        var claimsIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException();

        if (string.IsNullOrWhiteSpace(claimsIdentity) || !ObjectId.TryParse(claimsIdentity, out var userId))
            return Unauthorized("Invalid or missing user id in token");
        
        var listing = new Listing
        {
            ID = ObjectId.GenerateNewId(),
            UserID = userId,
            Title = req.Title,
            Description = req.Description,
            Category = req.Category,
            Condition = req.Condition,
            Images = req.Images,
            Status = req.Status,
            BuyPrice = req.BuyPrice,
            CurrentBid = req.CurrentBid,
            SellerID = req.SellerID,
            Bids = req.Bids,
            IsAuction = req.IsAuction || true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = req.UpdatedAt,
            EndTime = req.EndTime,
            // need status...
            Watchers = req.Watchers,
            Location = req.Location,
        };

        await _db.Listings.AddAsync(listing);
        await _db.SaveChangesAsync(ct);
        return Ok(listing);
    }
}