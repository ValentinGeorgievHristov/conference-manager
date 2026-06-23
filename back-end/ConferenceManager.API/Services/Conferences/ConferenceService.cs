using ConferenceManager.API.Data;
using ConferenceManager.API.DTOs.Conferences;
using ConferenceManager.API.Models;

namespace ConferenceManager.API.Services.Conferences
{
    public class ConferenceService : IConferenceService
    {
        private readonly AppDbContext _context;
        // private readonly IConfiguration _configuration;

        public ConferenceService(AppDbContext context)
        {
            _context = context;
        }

        public void CreateConference(CreateConferenceDto dto, int userId)
        {
            var conference = new Conference
            {
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date.ToUniversalTime(),
                UserId = userId
            };

            _context.Conferences.Add(conference);
            _context.SaveChanges();
        }

        public List<ConferenceDto> GetUserConferences(int userId)
        {
            return _context.Conferences
                .Where(c => c.UserId == userId)
                .Select(c => new ConferenceDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Date = c.Date
                })
                .ToList();
        }

        public List<ConferenceDto> GetAllConferences()
        {
            return _context.Conferences
                .Select(c => new ConferenceDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Date = c.Date
                })
                .ToList();
        }

        public bool UpdateConference(int conferenceId, UpdateConferenceDto dto, int currentUserId, bool isAdmin)
        {
            var conference = _context.Conferences.FirstOrDefault(c => c.Id == conferenceId);

            if (conference == null)
            {
                return false;
            }

            if (conference.UserId != currentUserId && !isAdmin)
            {
                return false;
            }

            conference.Title = dto.Title;
            conference.Description = dto.Description;
            conference.Date = dto.Date.ToUniversalTime();

            _context.SaveChanges();

            return true;
        }

        public string DeleteConference(int conferenceId, int currentUserId, bool isAdmin)
        {
            var conference = _context.Conferences.FirstOrDefault(c => c.Id == conferenceId);

            if (conference == null)
            {
                return "NotFound";
            }

            if(conference.UserId != currentUserId && !isAdmin)
            {
                return "Forbidden";
            };

            _context.Conferences.Remove(conference);

            _context.SaveChanges();

            return "Deleted";
        }
    }
}
