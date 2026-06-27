namespace ConferenceManager.API.DTOs.Promoters
{
    public class AssignPromoterDto
    {
        public int UserId { get; set; }
        public string ReferralCode { get; set; } = string.Empty;
    }
}
