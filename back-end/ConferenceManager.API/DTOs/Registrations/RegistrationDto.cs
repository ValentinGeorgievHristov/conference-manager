namespace ConferenceManager.API.DTOs.Registrations
{
    public class RegistrationDto
    {
        public int Id { get; set; }
        public int ConferenceId { get; set; }
        public String ConferenceTitle { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public bool IsConfirmed { get; set; }

    }
}
