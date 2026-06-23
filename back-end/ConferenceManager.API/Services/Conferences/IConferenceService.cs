using ConferenceManager.API.DTOs.Conferences;
using ConferenceManager.API.Models;

namespace ConferenceManager.API.Services.Conferences
{
    public interface IConferenceService
    {
        void CreateConference(CreateConferenceDto dto, int userId);
        List<ConferenceDto> GetUserConferences(int userId);

        List<ConferenceDto> GetAllConferences();

        bool UpdateConference(
            int conferenceId, 
            UpdateConferenceDto dto,
            int currentUserId,
            bool isAdmin);

        string DeleteConference(int conferenceId, int currentUserId, bool isAdmin);
    }
}
