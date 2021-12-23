using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpPut]
        [Route("assign-admin")]
        public async Task<IActionResult> AssignUserAsAdmin(string username)
        {
            var result = await _userService.AssignUserAsAdmin(username);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> Get()
        {
            List<UserResponse> result = await _userService.GetAllAsync();
            return result;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateRequest inputModel)
        {
            UserResponse result = await _userService.UpdateUserAsync(inputModel);
            if (result != null)
            {
                return StatusCode(204, result);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            bool result = await _userService.DeleteUserAsync(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
