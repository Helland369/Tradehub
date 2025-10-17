namespace Backend.Services;

public class PasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 16);
    }

    public bool ChechPasswordHash(string password, string hashedPassword) {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
