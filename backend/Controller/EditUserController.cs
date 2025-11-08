using System.Security.Claims;
using Backend.DTO.Users;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Backend.Controller;

[ApiController]
[Route("/api/edituser")]
[Authorize]
public class EditUserController : ControllerBase
{
    private readonly TradehubDbContext  _db;
    private readonly PasswordHasher _hasher;
    
    public EditUserController(TradehubDbContext db, PasswordHasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }
    
    [HttpPatch]
    public async Task<IActionResult> EditUser([FromBody]EditUser req, CancellationToken ct)
    {
        var claimsIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(claimsIdentity) || !ObjectId.TryParse(claimsIdentity, out var userId))
            return Unauthorized("Invalid or missing user id in token");
            
        var user = await _db.Users.FirstOrDefaultAsync(u => u.ID == userId, ct);
        if (user == null) return NotFound("User not found");
        
        if (!string.IsNullOrWhiteSpace(req.Fname)) user.Fname = req.Fname;
        if (!string.IsNullOrWhiteSpace(req.Lname)) user.Lname = req.Lname;
        if (!string.IsNullOrWhiteSpace(req.Email)) user.Email = req.Email;
        if (!string.IsNullOrWhiteSpace(req.UserName)) user.UserName = req.UserName;
        if (!string.IsNullOrWhiteSpace(req.Phone))  user.Phone = req.Phone;
        if (!string.IsNullOrWhiteSpace(req.Password)) user.PasswordHash = _hasher.HashPassword(req.Password);

        if (req.Address != null)
        {
            user.Address ??= new Address();
            if (!string.IsNullOrWhiteSpace(req.Address.Street)) user.Address.Street = req.Address.Street;
            if (!string.IsNullOrWhiteSpace(req.Address.City)) user.Address.City = req.Address.City;
            if (!string.IsNullOrWhiteSpace(req.Address.Country)) user.Address.Country = req.Address.Country;
            if (req.Address.Zip != null) user.Address.Zip = req.Address.Zip;
        }

        await _db.SaveChangesAsync(ct);
        
        return Ok( new
        {
            user.ID,
            user.Fname,
            user.Lname,
            user.Email,
            user.UserName,
            user.Phone,
            user.Address
        });
    }
}