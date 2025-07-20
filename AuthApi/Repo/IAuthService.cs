using AuthApi.DTOs;

namespace AuthApi.Repo
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(AuthRequest Request, string IP);
    }
}
