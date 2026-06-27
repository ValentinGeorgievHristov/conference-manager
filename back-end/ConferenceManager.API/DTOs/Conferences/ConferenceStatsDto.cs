namespace ConferenceManager.API.DTOs.Conferences
{
    public class ConferenceStatsDto
    {
        public int ConferenceId { get; set; }
        public int Total { get; set; }
        public int Confirmed { get; set; }
        public int Unconfirmed { get; set; }
    }
}
