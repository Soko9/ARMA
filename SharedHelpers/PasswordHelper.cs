using System.Text;

namespace SharedHelpers.Helpers
{
    public class PasswordHelper
    {
        public static string HashPassword(string Password)
            => BCrypt.Net.BCrypt.HashPassword(Password);

        public static bool VerifyPassword(string Password, string StoredHash)
            => BCrypt.Net.BCrypt.Verify(Password, StoredHash);

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
