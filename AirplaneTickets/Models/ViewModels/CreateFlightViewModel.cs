using AirplaneTickets.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirplaneTickets.Models.ViewModels
{
    public class CreateFlightViewModel
    {
        [Display(Name = "Departure")]
        [Required(ErrorMessage = "Departure airport is required.")]
        public Airport Departure { get; set; }

        [Display(Name = "Destination")]
        [Required(ErrorMessage = "Destination airport is required.")]
        public Airport Destination { get; set; }

        [Display(Name = "Departure date")]
        [Required(ErrorMessage = "Departure date is required.")]
        [DataType(DataType.Date)]
        public DateOnly DepartureDate { get; set; }

        [Display(Name = "Number of connections")]
        [Required(ErrorMessage = "Number of connections is required.")]
        [Range(0, 100, ErrorMessage = "Number of connections must be between 0 and 100.")]
        public int Connections { get; set; }

        [Display(Name = "Total number of seats")]
        [Required(ErrorMessage = "Total number of seats is required.")]
        [Range(1, 1000, ErrorMessage = "Total number of seats must be between 1 and 1000.")]
        public int TotalSeats { get; set; }
    }
}
