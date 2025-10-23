namespace Backend.DTO.Users;

using Backend.Models;

public record CreateUserRequest(
    string Fname,
    string Lname,
    string Email,
    string UserName,
    string Phone,
    string Password,
    Address Address,
    string? Role
);
