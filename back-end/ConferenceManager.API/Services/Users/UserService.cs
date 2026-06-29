namespace ConferenceManager.API.Services.Users
{
    using ConferenceManager.API.Data;
    using ConferenceManager.API.Models;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using ConferenceManager.API.DTOs.Users;

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<UserDto> GetAllUsers()
        {
            return _context.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                })
                .ToList();
        }

        public void CreateUser(CreateUserDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
            {
                throw new Exception("Email already exists.");
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public string Login(LoginDto dto)
        {
            var user = _context.Users
              .FirstOrDefault(u => u.Email == dto.Email);


            if (user == null)
            {
                return "USER NOT FOUND";
            }          

            if (!BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash))
            {
                return "";
            }

            // Secret Key.
            var key = _configuration["Jwt:Key"];

            // SecurityKey
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));

            // SigningCredentials
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
             {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
             };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
                );

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return true;
        }

        public bool UpdateUser(int id, CreateUserDto dto)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return false;
            }
     
            user.Username = dto.Username;
            user.Email = dto.Email;

            user.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(dto.Password);

            _context.SaveChanges();

            return true;
        }

        public bool UpdateUserImage(int userId, string imageUrl)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            user.ProfileImageUrl = imageUrl;

            _context.SaveChanges();

            return true;
        }
    }
}
