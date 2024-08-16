using AirplaneTickets.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AirplaneTickets.Models.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserAccount
    {
        [Key]
        [Display(Name = "Id")]
        public Guid Id { get; set; }

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
        public string Email { get; set; }

        [Display(Name = "Password Hash")]
        [Required(ErrorMessage = "Password is required.")]
        public string PasswordHash { get; set; }

    }
}
