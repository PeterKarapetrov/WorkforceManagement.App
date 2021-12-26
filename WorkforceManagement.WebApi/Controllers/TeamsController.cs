using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagement.Common;
using WorkforceManagement.Models.DTO.Request.Team;
using WorkforceManagement.Models.DTO.Response.Team;
using WorkforceManagement.Services.Contracts;
using WorkforceManagement.Services.Interfaces;

namespace WorkforceManagement.WebApi.Controllers
{
    [ApiController]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route("api/[controller]")]

    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamResponse>>> Get()
        {
            var teamsResponseList = await _teamService.GetAllTeams();

            if (teamsResponseList.Count() > 0)
            {
                return teamsResponseList.ToArray();
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamResponse>> Get(int id)
        {
            if (!CoreValidator.IsValid(id))
            {
                return BadRequest("Invalid Id");
            }

            var teamResponse = await _teamService.GetById(id);

            if (teamResponse != null)
            {
                return teamResponse;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post(TeamRequest teamRequest)
        {
            TeamResponse createTeam = await _teamService.CreateTeam(teamRequest);

            if (createTeam != null)
            {
                return CreatedAtAction("Get", "Teams", new { id = createTeam.Id }, teamRequest);
            }

            return BadRequest("Team already exist.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TeamRequest teamRequest)
        {
            if (CoreValidator.IsValid(id))
            {
                BadRequest("Ivalid Id");
            }

            var updatedTeam = await _teamService.UpdateTeam(teamRequest, id);

            if (updatedTeam != null)
            {
                return Ok("Team updated.");
            }

            return BadRequest("Update Failed");
        }

        [HttpPut("teamId={teamId}&userId={userId}&action={action}")]
        public async Task<IActionResult> Put(int teamId, string userId, string action)
        {
            if (!CoreValidator.IsValid(teamId) && !CoreValidator.isValid(userId))
            {
                return BadRequest("Invalid team or user Id");
            }

            //TODO fix this validation
            //if (action == GlobalConstants.AddMemberToTeam || action == GlobalConstants.RemoveMemberFromTeam)
            //{
            //    return BadRequest("Invalid action");
            //}

            if (action == GlobalConstants.AddMemberToTeam)
            {
                if (await _teamService.AssignMemberToTeam(teamId, userId))
                {
                    return Ok("Memeber assigned to Team");
                }
            }
            else if (action == GlobalConstants.RemoveMemberFromTeam)
            {
                if (await _teamService.RemoveMemberFromTeam(teamId, userId))
                {
                    return Ok("Memeber removed from Team");
                }
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TeamResponse>> Delete(int id)
        {
            if (CoreValidator.IsValid(id))
            {
                BadRequest("Ivalid Id");
            }

            var teamDeleted = await _teamService.DeleteTeam(id);

            if (teamDeleted != null)
            {
                return teamDeleted;
            }

            return BadRequest("Deletion Faild!");
        }
    }
}
