
namespace ConferenceManager.API.Services.Conferences
{
    using ConferenceManager.API.DTOs.Conferences;
    using ConferenceManager.API.Models;

    public interface IConferenceService
    {
        void CreateConference(CreateConferenceDto dto, int userId);
        List<ConferenceDto> GetUserConferences(int userId);

        List<ConferenceDto> GetAllConferences();

        bool UpdateConference(int conferenceId, UpdateConferenceDto dto, int currentUserId, bool isAdmin);
        string DeleteConference(int conferenceId, int currentUserId, bool isAdmin);
        ConferenceStatsDto GetConferenceStats(int conferenceId);
        bool UpdateConferenceImage(int conferenceId, string imageUrl, int currentUserId, bool isAdmin);

        Conference? GetConferenceById(int id);
    }
}
