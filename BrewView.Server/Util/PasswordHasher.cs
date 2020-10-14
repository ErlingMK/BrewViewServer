using System;
using System.Security.Cryptography;
using System.Text;
using BrewView.Contracts.User;
using BrewView.DatabaseModels.User;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BrewView.Server.Util
{
    internal class PasswordHasher
    {
        internal (string, string) Hash(string password)
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return (Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8)), Convert.ToBase64String(salt));
        }

        public static bool VerifyPassword(CredentialsModel credentialsModel, User user)
        {
            var passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(credentialsModel.Password, Convert.FromBase64String(user.Salt),
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));

            return passwordHash == user.PasswordHash;
        }
    }
}