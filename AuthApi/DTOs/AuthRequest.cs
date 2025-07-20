namespace AuthApi.DTOs
{
    public class AuthRequest
    {
        public required string Passcode { get; set; }
        public required string Password { get; set; }
    }
}
