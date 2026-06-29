namespace ConferenceManager.API.Controllers
{
    using ConferenceManager.API.DTOs.Conferences;
    using ConferenceManager.API.Services.Conferences;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;

    [ApiController]
    [Route("api/[controller]")]
    public class ConferenceController : ControllerBase
    {
        private readonly IConferenceService _conferenceService;
        private readonly IWebHostEnvironment _env;

        public ConferenceController(IConferenceService conferenceService,
            IWebHostEnvironment env)
        {
            _conferenceService = conferenceService;
            _env = env;
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

            if (userIdClaim == null)
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

            if (!updated)
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

            var result = _conferenceService.DeleteConference(
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

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/stats")]
        public IActionResult GetStats(int id)
        {
            var result = _conferenceService.GetConferenceStats(id);

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}/image")]
        public async Task<IActionResult> UpdateConferenceImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var isAdmin = User.IsInRole("Admin");

            var conference = _conferenceService.GetConferenceById(id);

            if (conference == null)
            {
                return NotFound("Conference not found");
            }

            var isOwner = conference.UserId == userId;

            if (!isOwner && !isAdmin)
            {
                return Forbid();
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"/images/{fileName}";

            var result = _conferenceService.UpdateConferenceImage(
                id,
                url,
                userId,
                isAdmin);

            if (!result)
            {
                return Forbid();
            }

            return Ok(new
            {
                url
            });
        }
    }
}
