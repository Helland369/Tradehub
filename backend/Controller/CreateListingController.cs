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
    private readonly IWebHostEnvironment _env;
    
    public CreateListingController(TradehubDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }
    
    [HttpPost]
    [Authorize]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public async Task<ActionResult<IEnumerable<Listing>>> Listing([FromForm]Listing req, CancellationToken ct)
    {
        var claimsIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(claimsIdentity) || !ObjectId.TryParse(claimsIdentity, out var userId))
            return Unauthorized("Invalid or missing user id in token");

        var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var targetDirectory = Path.Combine(webRoot, "uploads", "listings");
        Directory.CreateDirectory(targetDirectory);
        
        var savedImageUrls = new List<string>();
        if (req.Images.Count > 0)
        {
            foreach (var image in req.Images)
            {
                if (image.Length <= 0) continue;
                
                var extension = Path.GetExtension(image.FileName);
                var allowed = new[] {".jpg", ".jpeg", ".png", ".webp", ".gif"};
                if (!allowed.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    return BadRequest("Invalid image extension");

                var fileName = $"{ObjectId.GenerateNewId()}{extension}";
                var filePath = Path.Combine(targetDirectory, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream, ct);
                }
                
                var relativeFilePath = Path.Combine("uploads", fileName);
                savedImageUrls.Add(relativeFilePath);
            }
        }
        
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
            Bids = new List<Bid>(),
            IsAuction = req.IsAuction || true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            EndTime = req.EndTime,
            Watchers = new List<ObjectId>(),
            Location = req.Location,
        };

        await _db.Listings.AddAsync(listing, ct);
        await _db.SaveChangesAsync(ct);
        return Ok(listing);
    }
}