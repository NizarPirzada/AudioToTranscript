using Authenticity.CourtSide.Core.Models;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Authenticity.CourtSide.Core.Domains.Implementation
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32; // 256 bit
        private HashingOptions Options { get; }

        public PasswordHasher(IOptions<HashingOptions> options)
        {
            Options = options.Value;
        }

        public string Hash(string password)
        {

            using (var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Options.Iterations))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{Options.Iterations}.{salt}.{key}";
            }
        }

        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.', (char)3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected password format");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            using (var algorithm = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                byte[] keyToCheck = algorithm.GetBytes(KeySize);
                bool verified = keyToCheck.SequenceEqual(key);

                return verified;
            }
        }
    }
}
