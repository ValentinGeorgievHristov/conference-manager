namespace ConferenceManager.API.Models
{
    public class Conference
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public List<Registration> Registrations { get; set; } = new();
        public string? ImageUrl { get; set; }
    }
}
