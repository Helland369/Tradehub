namespace Backend.Services;

public class PasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
    }

    public bool CheckPasswordHash(string password, string hashedPassword) {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
