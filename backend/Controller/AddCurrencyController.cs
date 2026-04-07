using Backend.DTO.Users;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/addcurrency")]
public class AddCurrencyController : ControllerBase
{
    private readonly TradehubDbContext _db;
    private readonly ICurrentUserService _uservice;

    public AddCurrencyController(TradehubDbContext db, ICurrentUserService uservice)
    {
        _db = db;
        _uservice = uservice;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddCurrency([FromBody] AddCurrencyRequest req, CancellationToken ct)
    {
        try
        {
            if (!_uservice.TryGetUserId(out var userId))
                return Unauthorized("Invalid or missing user id in token");

            var user = _db.Users.FirstOrDefault(u => u.ID == userId);
            if (user == null)
                return NotFound("user not found");

            user.Points += req.amount;
            _ = await _db.SaveChangesAsync(ct);
            return Ok(new
            {
                message = "Successfully added currency",
                points = user.Points,
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest();
        }
    }
}
