namespace ConferenceManager.API.DTOs.Registrations
{
    public class AdminRegistrationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ConferenceId { get; set; }
        public string ConferenceTitle { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
