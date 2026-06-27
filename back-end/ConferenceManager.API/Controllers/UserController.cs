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

        public UserController(IUserService userService)
        {
            _userService = userService;
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




    }
}

