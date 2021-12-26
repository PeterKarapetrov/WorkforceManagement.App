using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WorkforceManagement.Common;
using WorkforceManagement.Data.Entities;
using WorkforceManagement.Models.DTO.Request;
using WorkforceManagement.Models.DTO.Response;
using WorkforceManagement.Services.Contracts;
using WorkforceManagement.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkforceManagement.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TimeOffRequestsController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ITimeOffRequestService _timeOffRequestService;

        public TimeOffRequestsController(IUserService userService, ITimeOffRequestService timeOffRequestService)
        {
            _userService = userService;
            _timeOffRequestService = timeOffRequestService;
        }

        // POST api/<TimeOffRequestsController>
        [HttpPost]
        public async Task<ActionResult<TimeOffRequestResponseDTO>> Post(TimeOffRequestRequestDTO timeOffRequestRequestDTO)
        {
            User currentUser = await _userService.GetCurrentUserAsync(this.User);

            if (currentUser == null)
            {
                return BadRequest();
            }

            TimeOffRequestResponseDTO timeOffRequest = await _timeOffRequestService.CreateTimeOffRequest(timeOffRequestRequestDTO, currentUser);

            if (timeOffRequest != null)
            {
                return CreatedAtAction("Get", new { id = timeOffRequest.TimeOffRequestId}, timeOffRequest);
            }

            return BadRequest("TimeOffRequest Create Failed!");
        }

        // PUT api/<TimeOffRequestsController>/5
        [HttpPut("{timeOffRequestId}")]
        public async Task<ActionResult<TimeOffRequestResponseDTO>> Put(int timeOffRequestId, TimeOffRequestRequestDTO timeOffRequestRequestDT)
        {
            if (!CoreValidator.IsValid(timeOffRequestId))
            {
                return BadRequest("Invalid Id");
            }

            User currentUser = await _userService.GetCurrentUserAsync(this.User);

            if (currentUser == null)
            {
                return BadRequest("Update Failed!");
            }

            TimeOffRequestResponseDTO timeOffRequest = await _timeOffRequestService.UpdateTimeOffRequest(timeOffRequestId, timeOffRequestRequestDT, currentUser);

            if (timeOffRequest != null)
            {
                return Ok();
            }

            return BadRequest("TimeOffRequest Update Failed!");
        }

        // PUT api/<TimeOffRequestsController>/5
        [HttpPut("ApproveRequest/{timeOffRequestApprovalId}/{isApproved}")]
        [ActionName("ApproveRequest")]
        public async Task<ActionResult<TimeOffRequestResponseDTO>> Put(int timeOffRequestApprovalId, bool isApproved)
        {
            if (!CoreValidator.IsValid(timeOffRequestApprovalId))
            {
                return BadRequest("Invalid Id");
            }

            User currentUser = await _userService.GetCurrentUserAsync(this.User);

            if (currentUser == null)
            {
                return BadRequest("Update Failed!");
            }

            TimeOffRequestResponseDTO timeOffRequest = await _timeOffRequestService.ApproveTimeOffRequest(timeOffRequestApprovalId, currentUser, isApproved);

            if (timeOffRequest != null)
            {
                return Ok();
            }

            return BadRequest("TimeOffRequest Update Failed!");
        }

        // DELETE api/<TimeOffRequestsController>/5
        [HttpDelete("{timeOffRequestId}")]
        public async Task<ActionResult<TimeOffRequestResponseDTO>> Delete(int timeOffRequestId)
        {
            if (!CoreValidator.IsValid(timeOffRequestId))
            {
                return BadRequest("Invalid Id");
            }

            User currentUser = await _userService.GetCurrentUserAsync(this.User);

            if (currentUser == null)
            {
                return BadRequest("TimeOffRequest Delete Failed!");
            }

            TimeOffRequestResponseDTO timeOffRequestResponse = await _timeOffRequestService.DeleteTimeOffRequest(timeOffRequestId, currentUser);

            if (timeOffRequestResponse != null)
            {
                return timeOffRequestResponse;
            }

            return BadRequest("TimeOffRequest Delete Failed!");
        }

        // GET api/<TimeOffRequestsController>/5
        [HttpGet("{timeOffRequestId}")]
        public async Task<ActionResult<TimeOffRequestResponseDTO>> Get(int timeOffRequestId)
        {
            if (!CoreValidator.IsValid(timeOffRequestId))
            {
                return BadRequest("Invalid Id");
            }

            User currentUser = await _userService.GetCurrentUserAsync(this.User);

            if (currentUser == null)
            {
                return NotFound();
            }

            TimeOffRequestResponseDTO timeOffRequest = await _timeOffRequestService.GetTimeOffRequest(timeOffRequestId, currentUser);

            if (timeOffRequest != null)
            {
                return timeOffRequest;
            }

            return NotFound();
        }
    }
}
