using Microsoft.AspNetCore.Mvc;
using UserManagementApi.DTOs.PermissionCategory;
using UserManagementApi.Repo;

namespace UserManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionCategoryController : ControllerBase
    {
        private readonly IPermissionCategoryService _service;

        public PermissionCategoryController(IPermissionCategoryService service)
        {
            _service = service;
        }

        [HttpGet("get-by-id/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _service.GetByIDAsync(id);

            if (category == null)
                return NotFound(new { Message = $"Permission Category not found." });

            return Ok(category);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("get-all-visible")]
        public async Task<IActionResult> GetAllVisible()
        {
            var categories = await _service.GetAllVisibleAsync();
            return Ok(categories);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PermissionCategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);

            if (!created)
                return StatusCode(500, new { Message = "An error occurred while creating the permission category." });

            return CreatedAtAction(nameof(GetById), new { id = dto.PermissionCategoryId }, dto);
        }

        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PermissionCategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.PermissionCategoryId == null || dto.PermissionCategoryId != id)
                return BadRequest(new { Message = "ID mismatch between route and body." });

            var updated = await _service.UpdateAsync(dto);

            if (!updated)
                return NotFound(new { Message = $"Permission Category not found or could not be updated." });

            return NoContent();
        }

        [HttpPatch("{id:guid}/toggle-visibility")]
        public async Task<IActionResult> ToggleVisibility(Guid id, [FromBody] Guid actionUserId)
        {
            var toggled = await _service.ToggleVisibility(id, actionUserId);

            if (!toggled)
                return NotFound(new { Message = $"Permission Category not found or could not be toggled." });

            return NoContent();
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, Guid actionUserId)
        {
            var deleted = await _service.DeleteAsync(id, actionUserId);

            if (!deleted)
                return NotFound(new { Message = $"Permission Category not found or could not be deleted." });

            return NoContent();
        }
    }
}
