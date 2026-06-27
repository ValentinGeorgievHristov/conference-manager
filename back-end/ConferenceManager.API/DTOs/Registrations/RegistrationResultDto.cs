namespace ConferenceManager.API.DTOs.Registrations
{
    public class RegistrationResultDto
    {
        public bool Success { get; set; }

        public bool HasPromoter { get; set; }

        public string? PromoterName { get; set; }

        public string? Warning { get; set; }
       
    }
}
