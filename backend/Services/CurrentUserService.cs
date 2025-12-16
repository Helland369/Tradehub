using System.Security.Claims;
using MongoDB.Bson;

namespace Backend.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;

    public CurrentUserService(IHttpContextAccessor http)
    {
        _http = http;
    }

    public bool TryGetUserId(out ObjectId userId)
    {
        userId = default;

        var user = _http.HttpContext?.User;
        if (user == null)
            return false;

        var claimValue = user.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? user.FindFirst("sub")?.Value;

        return ObjectId.TryParse(claimValue, out userId);
    }

    public ObjectId GetUserId()
    {
        if (!TryGetUserId(out var userId))
            throw new UnauthorizedAccessException("Invalid or missing user Id in token!");

        return userId;
    }
}                             
