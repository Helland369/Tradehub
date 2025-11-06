using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Backend.Controller;

[ApiController]
[Route("/api/listings")]
public class GetListingsController : ControllerBase
{
    private readonly TradehubDbContext _db;
    
    public GetListingsController(TradehubDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetListings()
    {
        var listing = await _db.Listings.ToListAsync();
        
        return Ok(listing);
    }
    
    [HttpGet("{id}")]
    public async  Task<IActionResult> GetListingsById(string id)
    {
        var listing = await _db.Listings.FindAsync(id);
        return Ok(listing);
    }
}