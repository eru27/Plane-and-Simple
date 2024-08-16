using AirplaneTickets.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AirplaneTickets.Models.ViewModels
{
    public class CreateAccountViewModel
    {
        [Display(Name = "Account type")]
        [Required(ErrorMessage = "Type of the account is required.")]
        public UserAccountType Type { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum 50 characters allowed.")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum 50 characters allowed.")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Maximum 100 characters allowed.")]
        [EmailAddress(ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
