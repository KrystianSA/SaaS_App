using System.Security.Cryptography;
using System.Text;

namespace SaaS_App.Application.Helpers
{
    public class TokenUtils
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random Random = new Random();
        public static string GenerateToken(int length)
        {
            return GenerateToken(Alphabet, length);
        }

        public static string GenerateToken(string characters, int length)
        {
            return new string(Enumerable
              .Range(0, length)
              .Select(num => characters[Random.Next() % characters.Length])
              .ToArray());
        }

        public static string GenerateHash(string text)
        {
            using (var hash = SHA256.Create())
            {
                return Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(text)));
            }
        }
    }
}
