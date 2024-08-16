using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AirplaneTickets.Models.ViewModels
{
    public class CreateReservationViewModel
    {
        [Required]
        [Display(Name = "User Id")]
        public Guid UserId { get; set; }

        [Required]
        [Display(Name = "Flight Id")]
        public Guid FlightId { get; set; }

        [Display(Name = "Number of seats")]
        [Required(ErrorMessage = "Number of seats is required.")]
        [Range(1, 1000, ErrorMessage = "Number of seats must be between 1 and 1000.")]
        public int NumberOfSeats { get; set; }
    }
}
