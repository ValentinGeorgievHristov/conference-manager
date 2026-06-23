namespace ConferenceManager.API.DTOs.Conferences
{
    public class UpdateConferenceDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
