using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Models;

namespace UserManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly UserManagementDbContext _Db;

        public HealthController(UserManagementDbContext Db)
        {
            _Db = Db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                bool CanConnect = await _Db.Database.CanConnectAsync();
                return Ok(new
                {
                    status = CanConnect ? "Healthy" : "Degraded",
                    database = CanConnect ? "Connected" : "Disconnected",
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "Unhealthy",
                    error = ex.Message,
                    timestamp = DateTime.Now
                });
            }
        }
    }
}
