using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Constants;
using UserManagementApi.DTOs.RolesPermission;
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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RolesPermissionsDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Created = await _Service.CreateAsync(Dto);

            if (!Created)
                return StatusCode(500, new { Message = Messages.RPCreateError() });

            return Ok(new { Message = Messages.RPCreateSuccess() });
        }

        [HttpPatch("{Id:Guid}/toggle-activity")]
        public async Task<IActionResult> ToggleVisibility(Guid Id, [FromBody] Guid ActionId)
        {
            var Toggled = await _Service.ToggleActivity(Id, ActionId);

            if (!Toggled)
                return NotFound(new { Message = Messages.RPToggleError() });

            return Ok(new { Message = Messages.RPToggleSuccess() });
        }

        [HttpDelete("delete/{Id:Guid}")]
        public async Task<IActionResult> Delete(Guid Id, [FromBody] Guid ActionId)
        {
            var Deleted = await _Service.DeleteAsync(Id, ActionId);

            if (!Deleted)
                return NotFound(new { Message = Messages.RPDeleteError() });

            return Ok(new { Message = Messages.RPDeleteSuccess() });
        }
    }
}
