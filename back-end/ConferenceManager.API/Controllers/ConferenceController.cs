using ConferenceManager.API.DTOs.Conferences;
using ConferenceManager.API.Services.Conferences;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConferenceManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConferenceController : ControllerBase
    {
        private readonly IConferenceService _conferenceService;

        public ConferenceController(IConferenceService conferenceService)
        {
            _conferenceService = conferenceService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateConference(CreateConferenceDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            _conferenceService.CreateConference(dto, int.Parse(userId));

            return Ok("Conference created");
        }

        [Authorize]
        [HttpGet("my")]
        public IActionResult GetMyConferences()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var conferences = _conferenceService
                .GetUserConferences(int.Parse(userId));

            return Ok(conferences);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public IActionResult GetAllConferences()
        {
            var conferences = _conferenceService.GetAllConferences();

            return Ok(conferences);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateConference(int id, UpdateConferenceDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(userIdClaim == null)
            {
                return Unauthorized();
            }

            var currentUserId = int.Parse(userIdClaim);

            var isAdmin = User.IsInRole("Admin");

            var updated =
                _conferenceService.UpdateConference(
                    id,
                    dto,
                    currentUserId,
                    isAdmin);

            if(!updated)
            {
                return Forbid();
            }

            return Ok("Conference updated");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteConference(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var currentUserId = int.Parse(userIdClaim);

            var isAdmin = User.IsInRole("Admin");

            var result  = _conferenceService.DeleteConference(
                id, currentUserId, isAdmin);

            if (result == "NotFound")
            {
                return NotFound("Conference not found.");
            }

            if (result == "Forbidden")
            {
                return Forbid();
            }

            return Ok("Conference deleted");
        }
}
}
