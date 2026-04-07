using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        try
        {
            var listing = await _db.Listings.ToListAsync();

            return Ok(listing);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest();
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetListingsById(string id)
    {
        try
        {
            var listing = await _db.Listings.FindAsync(id);
            return Ok(listing);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest();
        }
    }
}
