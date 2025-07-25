﻿using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Constants;
using UserManagementApi.DTOs.Permission;
using UserManagementApi.Models;
using UserManagementApi.Repo;

namespace UserManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _Service;

        public PermissionController(IPermissionService Service)
        {
            _Service = Service;
        }

        [HttpGet("get-by-id/{Id:Guid}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            Permission? PermissionRecord = await _Service.GetByIDAsync(Id);

            if (PermissionRecord == null)
                return NotFound(new { Message = Messages.NotFound("Permission") });

            return Ok(PermissionRecord);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            IReadOnlyList<Permission> Permissions = await _Service.GetAllAsync();
            return Ok(Permissions);
        }

        [HttpGet("get-all-visible")]
        public async Task<IActionResult> GetAllVisible()
        {
            var Permissions = await _Service.GetAllVisibleAsync();
            return Ok(Permissions);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PermissionDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool Created = await _Service.CreateAsync(Dto);

            if (!Created)
                return StatusCode(500, new { Message = Messages.PCreateError() });

            return Ok(new { Message = Messages.PCreateSuccess() });
        }

        [HttpPut("update/{Id:Guid}")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] PermissionDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool Updated = await _Service.UpdateAsync(Id, Dto);

            if (!Updated)
                return NotFound(new { Message = Messages.PUpdateError() });

            return Ok(new { Message = Messages.PUpdateSuccess() });
        }

        [HttpPatch("{Id:Guid}/toggle-visibility")]
        public async Task<IActionResult> ToggleVisibility(Guid Id, [FromBody] Guid ActionId)
        {
            bool Toggled = await _Service.ToggleVisibility(Id, ActionId);

            if (!Toggled)
                return NotFound(new { Message = Messages.PToggleError() });

            return Ok(new { Message = Messages.PToggleSuccess() });
        }

        [HttpDelete("delete/{Id:Guid}")]
        public async Task<IActionResult> Delete(Guid Id, [FromBody] Guid ActionId)
        {
            bool Deleted = await _Service.DeleteAsync(Id, ActionId);

            if (!Deleted)
                return NotFound(new { Message = Messages.PDeleteError() });

            return Ok(new { Message = Messages.PDeleteSuccess() });
        }
    }
}
