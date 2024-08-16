using System.ComponentModel.DataAnnotations;

namespace AirplaneTickets.Models.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Maximum 100 characters allowed.")]
        [EmailAddress(ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
