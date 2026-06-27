namespace ConferenceManager.API.Services.Promoters
{
    using ConferenceManager.API.DTOs.Promoters;
    using ConferenceManager.API.Models;
    using ConferenceManager.API.Data;

    public class PromoterService : IPromoterService
    {
        private readonly AppDbContext _context;

        public PromoterService(AppDbContext context)
        {
            _context = context;
        }

        public bool AssignPromoter(int userId, string referralCode)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var existingCode = _context.PromoterProfiles
                .FirstOrDefault(p => p.ReferralCode == referralCode);

            if(existingCode != null)
            {
                return false;
            }

            user.Role = "Promoter";

            var profile = new PromoterProfile
            {
                UserId = userId,
                ReferralCode = referralCode,
                IsActive = true
            };

            _context.PromoterProfiles.Add(profile);

            _context.SaveChanges();

            return true;
        }
    }
}
