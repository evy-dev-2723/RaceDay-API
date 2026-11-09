using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models
{
    public class EventViewModel
    {
        public int EventID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Venue { get; set; } = string.Empty;
        public decimal DistanceKm { get; set; }
        public string OrganiserName { get; set; } = string.Empty;
    }

    public class CreateEventViewModel
    {
        [Required(ErrorMessage = "Event name is required")]
        public string EventName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;

        public string? Venue { get; set; }

        [Required(ErrorMessage = "Distance is required")]
        public decimal DistanceKm { get; set; }
    }
}