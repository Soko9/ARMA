using AuthApi.DTOs;
using AuthApi.Models;
using AuthApi.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedHelpers.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly DbSet<User> _Users;
        private readonly DbSet<IpwhiteList> _IPs;
        private readonly ILogService _Log;

        public AuthService(UserManagementDbContext Db, ILogService Log)
        {
            _Users = Db.Users;
            _IPs = Db.IpwhiteLists;
            _Log = Log;
        }

        public async Task<AuthResponse?> LoginAsync(AuthRequest Request, string IP)
        {
            //IpwhiteList? Ip = await _IPs.Where(Ip => Ip.Ip == IP).FirstOrDefaultAsync();
            //if (Ip == null || !Ip.IsActive)
            //{
            //    await _Log.LogAsync("Login", "Failed login Attempt From An Unauthorized IP", "IPWhiteList", Ip!.Ipid, null);
            //    return null;
            //}

            User? UserRecord = await _Users.FirstOrDefaultAsync(U => U.Passcode == Request.Passcode);
            if (UserRecord == null || !PasswordHelper.VerifyPassword(Request.Password, UserRecord.PasswordHash) || !UserRecord.IsActive || UserRecord.IsLocked)
            {
                await _Log.LogAsync("Login", "Failed Login Attempt From An Unauthorized User", "User", UserRecord?.UserId ?? Guid.Empty, null);
                return null;
            }

            string Token = GenerateJWTToken(UserRecord);

            return new AuthResponse()
            {
                Token = Token,
            };
        }

        private string GenerateJWTToken(User UserRecord)
        {
            Claim[] Claims =
            {
                new Claim(ClaimTypes.NameIdentifier, UserRecord.UserId.ToString()),
                new Claim(ClaimTypes.Name, UserRecord.FullName),
                new Claim(ClaimTypes.Role, UserRecord.RoleId.ToString()!),
            };

            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("X2n+avfQZpK5HVVQQK1Hnp3WysAoIXIE8w6zPfwFq4g"));
            SigningCredentials Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: "auth-api",
                audience: "arma-system",
                signingCredentials: Credentials,
                claims: Claims,
                expires: DateTime.Now.AddDays(30)
            );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
