namespace ConferenceManager.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public List<Registration> Registrations { get; set; } = new();

        public PromoterProfile? PromoterProfile { get; set; }

        public string? ProfileImageUrl { get; set; }

    }
}
