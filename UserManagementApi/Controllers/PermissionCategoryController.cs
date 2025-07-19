using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Constants;
using UserManagementApi.DTOs.PermissionCategory;
using UserManagementApi.Repo;

namespace UserManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionCategoryController : ControllerBase
    {
        private readonly IPermissionCategoryService _Service;

        public PermissionCategoryController(IPermissionCategoryService Service)
        {
            _Service = Service;
        }

        [HttpGet("get-by-id/{Id:Guid}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var Category = await _Service.GetByIDAsync(Id);

            if (Category == null)
                return NotFound(new { Message = Messages.NotFound("Permission Category") });

            return Ok(Category);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var Categories = await _Service.GetAllAsync();
            return Ok(Categories);
        }

        [HttpGet("get-all-visible")]
        public async Task<IActionResult> GetAllVisible()
        {
            var Categories = await _Service.GetAllVisibleAsync();
            return Ok(Categories);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PermissionCategoryDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Created = await _Service.CreateAsync(Dto);

            if (!Created)
                return StatusCode(500, new { Message = Messages.PCCreateError() });

            return Ok(new { Massage = Messages.PCCreateSuccess() });
        }

        [HttpPut("update/{Id:Guid}")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] PermissionCategoryDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Updated = await _Service.UpdateAsync(Id, Dto);

            if (!Updated)
                return NotFound(new { Message = Messages.PCUpdateError() });

            return Ok(new { Message = Messages.PCUpdateSuccess() });
        }

        [HttpPatch("{Id:Guid}/toggle-visibility")]
        public async Task<IActionResult> ToggleVisibility(Guid Id, [FromBody] Guid ActionId)
        {
            var Toggled = await _Service.ToggleVisibility(Id, ActionId);

            if (!Toggled)
                return NotFound(new { Message = Messages.PCToggleError() });

            return Ok(new { Message = Messages.PCToggleSuccess() });
        }

        [HttpDelete("delete/{Id:Guid}")]
        public async Task<IActionResult> Delete(Guid Id, [FromBody] Guid ActionId)
        {
            var Deleted = await _Service.DeleteAsync(Id, ActionId);

            if (!Deleted)
                return NotFound(new { Message = Messages.PCDeleteError() });

            return Ok(new { Message = Messages.PCDeleteSuccess() });
        }
    }
}
