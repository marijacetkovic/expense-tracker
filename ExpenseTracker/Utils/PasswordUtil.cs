using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ExpenseTracker.Utils
{
    
    public static class PasswordUtil{
        
        // Generates a random salt and hashes the password using PBKDF2
        public static (string Hash, string Salt) HashPassword(string password)
        {
            // Generate a 128-bit salt
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            // Derive a 256-bit subkey (PBKDF2)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Return both hash and salt as base64 strings
            return (hashed, Convert.ToBase64String(salt));
        }

        // Verifies a password against a stored hash and salt
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            string hashToCheck = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 256 / 8));

            return hashToCheck == storedHash;
        }
    }

}
