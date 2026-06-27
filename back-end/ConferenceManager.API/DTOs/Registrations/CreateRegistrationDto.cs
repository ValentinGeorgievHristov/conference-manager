namespace ConferenceManager.API.DTOs.Registrations
{
    public class CreateRegistrationDto
    {
        public int ConferenceId { get; set; }
        
        public string? ReferralCode { get; set; }
    }
}
