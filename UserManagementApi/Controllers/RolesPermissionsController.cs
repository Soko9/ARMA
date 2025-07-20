using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Constants;
using UserManagementApi.DTOs.RolesPermission;
using UserManagementApi.Models;
using UserManagementApi.Repo;

namespace UserManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesPermissionsController : ControllerBase
    {
        private readonly IRolesPermissionsService _Service;

        public RolesPermissionsController(IRolesPermissionsService Service)
        {
            _Service = Service;
        }

        [HttpGet("get-permissions-by-role-id/{Id:Guid}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            IReadOnlyList<Permission> RolePermssions = await _Service.GetPermissionsByRoleAsync(Id);
            return Ok();
        }

        [HttpPost("upsert-role-permission/{Id:Guid}")]
        public async Task<IActionResult> Upsert(Guid Id, [FromBody] RolesPermissionsDTO Dto)
        {
            bool Upserted = await _Service.UpsertPermissionsForRoleAsync(Id, Dto.PermissionsIds, Dto.LastActionUserId);

            if (!Upserted)
                return NotFound(new { Message = Messages.RPToggleError() });

            return Ok(new { Message = Messages.RPToggleSuccess() });
        }
    }
}
