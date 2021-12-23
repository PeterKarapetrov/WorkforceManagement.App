using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.Common;
using WorkforceManagement.Models.DTO.Request.User;
using WorkforceManagement.Models.DTO.Response.User;
using WorkforceManagement.Services.Contracts;

namespace WorkforceManagement.WebApi.Controllers
{
    [ApiController]
    //[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Post(UserCreateRequest inputModel)
        {
            UserResponse userResponse = await _userService.CreateUserAsync(inputModel);

            if (userResponse != null)
            {
                return CreatedAtAction("Get", userResponse);
            }

            return BadRequest();
        }

        [HttpPut("assign-admin/{username}")]
        public async Task<IActionResult> Put(string username)
        {
            if (!CoreValidator.isValid(username))
            {
                return BadRequest("Invalid username!");
            }
            var result = await _userService.AssignUserAsAdminAsync(username);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<UserResponse>>> Get()
        {
            ICollection<UserResponse> usersList = await _userService.GetAllAsync();

            if (usersList.Count() > 0)
            {
                return usersList.ToArray();
            }

            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, UserUpdateRequest inputModel)
        {
            if (!CoreValidator.isValid(id))
            {
                return BadRequest("Invalid Id");
            }

            UserResponse userUpdated = await _userService.UpdateUserAsync(id, inputModel);

            if (userUpdated != null)
            {
                return Ok("User update succeed!");
            }

            return BadRequest("Update Failed!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserResponse>> DeleteUser(string id)
        {
            if (!CoreValidator.isValid(id))
            {
                return BadRequest("Deletion Failed!");
            }
            
            var userDeleted = await _userService.DeleteUserAsync(id);

            if (userDeleted != null)
            {
                return userDeleted;
            }

            return BadRequest("Deletion Failed!");
        }
    }
}
