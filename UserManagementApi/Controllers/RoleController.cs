using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Constants;
using UserManagementApi.DTOs.Role;
using UserManagementApi.Models;
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
            Role? RoleRecord = await _Service.GetByIDAsync(Id);

            if (RoleRecord == null)
                return NotFound(new { Message = Messages.NotFound("Role") });

            return Ok(RoleRecord);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            IReadOnlyList<Role> Roles = await _Service.GetAllAsync();
            return Ok(Roles);
        }

        [HttpGet("get-all-visible")]
        public async Task<IActionResult> GetAllVisible()
        {
            IReadOnlyList<Role> Roles = await _Service.GetAllVisibleAsync();
            return Ok(Roles);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RoleDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool Created = await _Service.CreateAsync(Dto);

            if (!Created)
                return StatusCode(500, new { Message = Messages.RCreateError() });

            return Ok(new { Message = Messages.RCreateSuccess() });
        }

        [HttpPut("update/{Id:Guid}")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] RoleDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool Updated = await _Service.UpdateAsync(Id, Dto);

            if (!Updated)
                return NotFound(new { Message = Messages.RUpdateError() });

            return Ok(new { Message = Messages.RUpdateSuccess() });
        }

        [HttpPatch("{Id:Guid}/toggle-visibility")]
        public async Task<IActionResult> ToggleVisibility(Guid Id, [FromBody] Guid ActionId)
        {
            bool Toggled = await _Service.ToggleVisibility(Id, ActionId);

            if (!Toggled)
                return NotFound(new { Message = Messages.RToggleError() });

            return Ok(new { Message = Messages.RToggleSuccess() });
        }

        [HttpDelete("delete/{Id:Guid}")]
        public async Task<IActionResult> Delete(Guid Id, [FromBody] Guid ActionId)
        {
            bool Deleted = await _Service.DeleteAsync(Id, ActionId);

            if (!Deleted)
                return NotFound(new { Message = Messages.RDeleteError() });

            return Ok(new { Message = Messages.RDeleteSuccess() });
        }
    }
}
