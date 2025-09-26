using Auth.Microservice.Services.Interfaces;
using System.Security.Cryptography;

namespace Auth.Microservice.Services
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;

        public string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(KeySize);

            byte[] combined = new byte[SaltSize + KeySize];
            Array.Copy(salt, 0, combined, 0, SaltSize);
            Array.Copy(hash, 0, combined, SaltSize, KeySize);

            return Convert.ToBase64String(combined);
        }

        public bool VerifyPassword(string password, string storedHash)
        {

            byte[] combined = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[SaltSize];
            byte[] storedKey = new byte[KeySize];

            Array.Copy(combined, 0, salt, 0, SaltSize);
            Array.Copy(combined, SaltSize, storedKey, 0, KeySize);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, storedKey);
        }
    }
}
