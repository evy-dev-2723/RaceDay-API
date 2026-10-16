namespace RaceDay.API.DTOs
{
    public class EventDto
    {
        public int EventID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Venue { get; set; }
        public decimal DistanceKm { get; set; }
        public string? RouteMapURL { get; set; }
        public string OrganiserName { get; set; } = string.Empty;
        public List<EventCategoryDto> Categories { get; set; } = new();
    }

    public class EventCategoryDto
    {
        public int EventCategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal EntryFee { get; set; }
        public int MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; }
    }

    public class CreateEventDto
    {
        public string EventName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Venue { get; set; }
        public decimal DistanceKm { get; set; }
        public string? RouteMapURL { get; set; }
    }

    public class EnrolmentDto
    {
        public int EnrolmentID { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string ParticipantName { get; set; } = string.Empty;
        public DateTime EnrolmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public int? BibNumber { get; set; }
    }

    public class CreateEnrolmentDto
    {
        public int EventID { get; set; }
        public int CategoryID { get; set; }
    }

    public class ResultDto
    {
        public int ResultID { get; set; }
        public int EnrolmentID { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public TimeSpan FinishTime { get; set; }
        public int? Position { get; set; }
        public int? OverallRank { get; set; }
        public int? CategoryRank { get; set; }
        public decimal? PacePerKm { get; set; }
        public bool Verified { get; set; }
    }

    public class CreateResultDto
    {
        public int EnrolmentID { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int? Position { get; set; }
        public int? OverallRank { get; set; }
        public int? CategoryRank { get; set; }
        public decimal? PacePerKm { get; set; }
    }
}
