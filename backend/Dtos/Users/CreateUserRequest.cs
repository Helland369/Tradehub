using System.ComponentModel.DataAnnotations;

public record CreateUserRequest(
    [Required, MaxLength(80)] string Fname,
    [Required, MaxLength(80)] string Lname,
    [EmailAddress] string? Email,
    [Required, MinLength(8)] string Password
    );
