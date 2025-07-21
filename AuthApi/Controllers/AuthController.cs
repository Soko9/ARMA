using AuthApi.DTOs;
using AuthApi.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _AuthService;

        public AuthController(IAuthService AuthService)
        {
            _AuthService = AuthService;
        }

        [HttpPost]
        [EnableRateLimiting("LoginPolicy")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthRequest Request)
        {
            if (Request.Passcode == null || Request.Password == null)
            {
                return BadRequest("Invalid Login Request");
            }

            string? Ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(Ip))
            {
                return BadRequest("IP Address Not Found");
            }

            AuthResponse? Response = await _AuthService.LoginAsync(Request, Ip);
            if (Response == null || Response.Token == null)
            {
                return BadRequest("Invalid Credentials or Access Denied");
            }

            return Ok(Response);
        }
    }
}
