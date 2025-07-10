using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Constants;
using UserManagementApi.DTOs.Role;
using UserManagementApi.Repo;

namespace UserManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _Service;

        public RoleController(IRoleService Service)
        {
            _Service = Service;
        }

        [HttpGet("get-by-id/{Id:Guid}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var Role = await _Service.GetByIDAsync(Id);

            if (Role == null)
                return NotFound(new { Message = Messages.NotFound("Role") });

            return Ok(Role);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var Roles = await _Service.GetAllAsync();
            return Ok(Roles);
        }

        [HttpGet("get-all-visible")]
        public async Task<IActionResult> GetAllVisible()
        {
            var Roles = await _Service.GetAllVisibleAsync();
            return Ok(Roles);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RoleDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Created = await _Service.CreateAsync(Dto);

            if (!Created)
                return StatusCode(500, new { Message = Messages.RCreateError() });

            return Ok(new { Message = Messages.RCreateSuccess() });
        }

        [HttpPut("update/{Id:Guid}")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] RoleDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Dto.RoleId == null || Dto.RoleId != Id)
                return BadRequest(new { Message = Messages.Mismatch() });

            var Updated = await _Service.UpdateAsync(Dto);

            if (!Updated)
                return NotFound(new { Message = Messages.RUpdateError() });

            return Ok(new { Message = Messages.RUpdateSuccess() });
        }

        [HttpPatch("{Id:Guid}/toggle-visibility")]
        public async Task<IActionResult> ToggleVisibility(Guid Id, [FromBody] Guid ActionId)
        {
            var Toggled = await _Service.ToggleVisibility(Id, ActionId);

            if (!Toggled)
                return NotFound(new { Message = Messages.RToggleError() });

            return Ok(new { Message = Messages.RToggleSuccess() });
        }

        [HttpDelete("delete/{Id:Guid}")]
        public async Task<IActionResult> Delete(Guid Id, Guid ActionId)
        {
            var Deleted = await _Service.DeleteAsync(Id, ActionId);

            if (!Deleted)
                return NotFound(new { Message = Messages.RDeleteError() });

            return Ok(new { Message = Messages.RDeleteSuccess() });
        }
    }
}
