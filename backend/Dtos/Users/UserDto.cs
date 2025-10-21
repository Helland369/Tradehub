
public record UserDto(
    string ID,
    string Fname,
    string Lname,
    string? Email,
    string Role,
    DateTime CreatedAt
);
