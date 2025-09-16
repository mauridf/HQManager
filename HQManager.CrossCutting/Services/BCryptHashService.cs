using BC = BCrypt.Net.BCrypt;

namespace HQManager.CrossCutting.Services;

public class BCryptHashService : IHashService
{
    public string HashPassword(string password)
    {
        return BC.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BC.Verify(password, hashedPassword);
    }
}