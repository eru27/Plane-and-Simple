using AirplaneTickets.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirplaneTickets.Models.ViewModels
{
    public class SearchFlightsViewModel
    {
        [Display(Name = "Departure")]
        [Required(ErrorMessage = "Departure airport is required.")]
        public Airport Departure { get; set; }

        [Display(Name = "Destination")]
        [Required(ErrorMessage = "Destination airport is required.")]
        public Airport Destination { get; set; }

        [Display(Name = "Connecting flights")]
        public bool ConnectingFlights { get; set; }

    }
}
