using AirplaneTickets.Models.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AirplaneTickets.Models.Entities
{
    public class Flight
    {
        [Key]
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "Departure")]
        [Required(ErrorMessage = "Departure airport is required.")]
        public Airport Departure { get; set; }

        [Display(Name = "Destination")]
        [Required(ErrorMessage = "Destination airport is required.")]
        public Airport Destination { get; set; }

        [Display(Name = "Departure date")]
        [Required(ErrorMessage = "Departure date is required.")]
        public DateOnly DepartureDate { get; set; }

        [Display(Name = "Number of connections")]
        [Required(ErrorMessage = "Number of connections is required.")]
        [Range(0, 100, ErrorMessage = "Number of connections must be between 0 and 100.")]
        public int Connections { get; set; }

        [Display(Name = "Total number of seats")]
        [Required(ErrorMessage = "Total number of seats is required.")]
        [Range(1, 1000, ErrorMessage = "Total number of seats must be between 1 and 1000.")]
        public int TotalSeats { get; set; }

        [Display(Name = "Number of seats available")]
        [Required(ErrorMessage = "Number of available seats is required.")]
        [Range(0, 1000, ErrorMessage = "Number of available seats must be between 0 and 1000.")]
        public int AvailableSeats { get; set; }

        [Display(Name = "Canceled")]
        [DefaultValue(false)]
        public bool IsCanceled { get; set; }
    }
}
