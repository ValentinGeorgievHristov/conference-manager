namespace ConferenceManager.API.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System.Security.Claims;

    using ConferenceManager.API.DTOs.Registrations;
    using ConferenceManager.API.Services.Registrations;

    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {

        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Register(CreateRegistrationDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim);

            var result = _registrationService.RegisterUser(userId, dto);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("my")]
        public IActionResult MyRegistrations()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim);

            var result = _registrationService.GetUserRegistrations(userId);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _registrationService.GetAllRegistrations();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/confirm")]
        public IActionResult ConfirmRegistration(int id, ConfirmRegistrationDto dto)
        {
            var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(adminIdClaim == null)
            {
                return Unauthorized();
            }

            var adminId = int.Parse(adminIdClaim);

            var result = _registrationService.ConfirmRegistration(id, dto.IsConfirmed, adminId);

            if (!result)
            {
                return Problem("Registration not found");
            }

            return Ok("Registration updated");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-registrations")]
        public IActionResult GetAll([FromQuery] string? status)
        {
            var result = _registrationService.GetAllRegistrations(status);

            return Ok(result);
        }
      
    }
}
