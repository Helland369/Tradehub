namespace Backend.DTO.Users;

using Backend.Models;

public record LoginUserRequest(string UserName, string Password);

public record LoginResponse(string Token, DateTime ExpiresAtUTC);
