namespace ConferenceManager.API.Services.Promoters
{
    using ConferenceManager.API.DTOs.Promoters;

    public interface IPromoterService
    {
        bool AssignPromoter(int userId, string referralCode);
   
    }
}
