namespace ConferenceManager.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using System.Security.Claims;

    using ConferenceManager.API.Services.Users;
    using ConferenceManager.API.DTOs.Users;

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;

        public UserController(
               IUserService userService,
               IWebHostEnvironment env)
        {
            _userService = userService;
            _env = env;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAllUsers());
        }


        [HttpPost]
        public IActionResult Create(CreateUserDto dto)
        {

            try
            {
                _userService.CreateUser(dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var result = _userService.Login(dto);

            //if(!result)
            //{
            //    return BadRequest("Invalid email or password.");
            //}

            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult Prifile()
        {
            //return Ok("You are authenticanted");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok(new
            {
                UserId = userId,
                Email = email
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);

            if (!result)
            {
                return NotFound("User not found");
            }

            return Ok("User deleted");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateUder(int id, CreateUserDto dto)
        {
            var result = _userService.UpdateUser(id, dto);

            if (!result)
            {
                return NotFound("User not found.");
            }

            return Ok("User updated.");
        }

        [Authorize]
        [HttpPut("me/profile-image")]
        public async Task<IActionResult> UpdateMyProfileImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
       
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"/images/{fileName}";

            var result = _userService.UpdateUserImage(userId, url);

            if (!result)
                return NotFound("User not found");

            return Ok(new { url });
        }

    }
}

