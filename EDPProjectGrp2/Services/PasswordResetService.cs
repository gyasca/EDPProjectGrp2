using System;
using System.Security.Cryptography;
using System.Text;

public class PasswordResetService
{
    public string GenerateToken()
    {
        // Generate a random token
        byte[] tokenData = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenData);
        }
        return Convert.ToBase64String(tokenData);
    }
}
