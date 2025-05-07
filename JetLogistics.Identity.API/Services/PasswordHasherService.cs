using Microsoft.AspNetCore.Identity;

namespace JetLogistics.Identity.API.Services
{
    public static class PasswordHasherService
    {
        private static readonly PasswordHasher<string> _hasher = new();

        public static string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return _hasher.VerifyHashedPassword(null!, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
        }
    }
}
