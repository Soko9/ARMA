using System.Security.Cryptography;
using System.Text;

namespace UserManagementApi.Helpers
{
    public class PasswordHelper
    {
        public static (string Hash, string Salt) HashPassword(string Password)
        {
            byte[] SaltBytes = new byte[16];
            using RandomNumberGenerator RandomNumber = RandomNumberGenerator.Create();
            RandomNumber.GetBytes(SaltBytes);
            string Salt = Convert.ToBase64String(SaltBytes);

            using SHA256 Sha = SHA256.Create();
            byte[] CombinedBytes = Encoding.UTF8.GetBytes(Password + Salt);
            byte[] HashBytes = Sha.ComputeHash(CombinedBytes);
            string Hash = Convert.ToBase64String(HashBytes);

            return (Hash, Salt);
        }

        public static bool VerifyPassword(string Password, string StoredHash, string StoredSalt)
        {
            using SHA256 Sha = SHA256.Create();
            byte[] CombinedBytes = Encoding.UTF8.GetBytes(Password + StoredSalt);
            byte[] HashBytes = Sha.ComputeHash(CombinedBytes);
            string Hash = Convert.ToBase64String(HashBytes);

            return Hash == StoredHash;
        }

        public static string GenerateRandomPasscode(int Length = 5)
        {
            StringBuilder Passcode = new StringBuilder(Length);
            Random Random = new Random();
            for (int i = 0; i < Length; i++)
            {
                Passcode.Append(Random.Next(0, 10));
            }
            return Passcode.ToString();
        }
    }
}
