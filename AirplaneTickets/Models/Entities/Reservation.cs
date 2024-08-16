using AirplaneTickets.Models.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirplaneTickets.Models.Entities
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public Guid UserId { get; set; }

        public UserAccount User { get; set; }

        [ForeignKey("FlightId")]
        [Required]
        public Guid FlightId { get; set; }

        public Flight Flight { get; set; }

        [Display(Name = "Number of seats")]
        [Required(ErrorMessage = "Number of seats is required.")]
        [Range(1, 1000, ErrorMessage = "Number of seats must be between 1 and 1000.")]
        public int NumberOfSeats { get; set; }

        [Display(Name = "Reservation status")]
        [Required(ErrorMessage = "Reservation status is required.")]
        [DefaultValue(ReservationStatus.Pending)]
        public ReservationStatus Status { get; set; }
    }
}
