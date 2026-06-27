namespace ConferenceManager.API.Models
{
    using ConferenceManager.API.Models.Enums;

    public class Registration
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int ConferenceId { get; set; }

        public Conference Conference { get; set; } = null!;

        public DateTime RegistrationDate { get; set; }

        public bool IsConfirmed { get; set; }

        public int? ConfirmedByAdminId { get; set; }

        public DateTime? ConfirmedAt { get; set; }

        public int? PromoterId { get; set; }

        public RegistrationSourceType SourceType { get; set; }

        public int? PromoterProfileId { get; set; }

        public PromoterProfile? PromoterProfile { get; set; }
             
    }
}
