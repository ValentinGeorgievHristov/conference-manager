namespace ConferenceManager.API.Models
{
    public class PromoterProfile
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ReferralCode { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public User User { get; set; } = null!;
    }
}
