using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Constants;
using UserManagementApi.DTOs.User;
using UserManagementApi.Models;
using UserManagementApi.Repo;

namespace UserManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _Service;

        public UserController(IUserService Service)
        {
            _Service = Service;
        }

        [HttpGet("get-by-id/{Id:Guid}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            User? UserRecord = await _Service.GetByIDAsync(Id);

            if (UserRecord == null)
                return NotFound(new { Message = Messages.NotFound("User") });

            return Ok(UserRecord);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            IReadOnlyList<User> Users = await _Service.GetAllAsync();
            return Ok(Users);
        }

        [HttpGet("get-all-visible")]
        public async Task<IActionResult> GetAllActive()
        {
            IReadOnlyList<User> Users = await _Service.GetAllActiveAsync();
            return Ok(Users);
        }

        [HttpGet("get-all-locked")]
        public async Task<IActionResult> GetAllLocked()
        {
            IReadOnlyList<User> Users = await _Service.GetAllLocedAsync();
            return Ok(Users);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool Created = await _Service.CreateAsync(Dto);

            if (!Created)
                return StatusCode(500, new { Message = Messages.UCreateError() });

            return Ok(new { Message = Messages.UCreateSuccess() });
        }

        [HttpPut("update/{Id:Guid}")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] UserDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool Updated = await _Service.UpdateAsync(Id, Dto);

            if (!Updated)
                return NotFound(new { Message = Messages.UUpdateError() });

            return Ok(new { Message = Messages.UUpdateSuccess() });
        }

        [HttpPatch("{Id:Guid}/toggle-activity")]
        public async Task<IActionResult> ToggleActivity(Guid Id, [FromBody] Guid ActionId)
        {
            bool Toggled = await _Service.ToggleActivity(Id, ActionId);

            if (!Toggled)
                return NotFound(new { Message = Messages.UToggleError() });

            return Ok(new { Message = Messages.UToggleSuccess() });
        }

        [HttpPatch("{Id:Guid}/reset-passcode")]
        public async Task<IActionResult> ResetPasscode(Guid Id, [FromBody] Guid ActionId)
        {
            bool Toggled = await _Service.ResetPasscodeAsync(Id, ActionId);

            if (!Toggled)
                return NotFound(new { Message = Messages.UResetPasscodeError() });

            return Ok(new { Message = Messages.UResetPasscodeSuccess() });
        }

        [HttpPatch("{Id:Guid}/reset-password")]
        public async Task<IActionResult> ResetPassword(Guid Id, [FromBody] UserDTO Dto)
        {
            bool Toggled = await _Service.ResetPasswordAsync(Id, Dto);

            if (!Toggled)
                return NotFound(new { Message = Messages.UResetPasswordError() });

            return Ok(new { Message = Messages.UResetPasswordSuccess() });
        }

        [HttpDelete("delete/{Id:Guid}")]
        public async Task<IActionResult> Delete(Guid Id, [FromBody] Guid ActionId)
        {
            bool Deleted = await _Service.DeleteAsync(Id, ActionId);

            if (!Deleted)
                return NotFound(new { Message = Messages.UDeleteError() });

            return Ok(new { Message = Messages.UDeleteSuccess() });
        }
    }
}
