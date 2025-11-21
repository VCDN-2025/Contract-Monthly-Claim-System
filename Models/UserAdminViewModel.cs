using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    // Represents the input fields required for creating or managing a user account
    public class UserAdminViewModel
    {
        // Stores the user's email address used as the username
        [Required]
        [EmailAddress]
        [Display(Name = "Email (Username)")]
        public string Email { get; set; } = string.Empty;

        // Holds the account password assigned to the user
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        // Defines the role assigned to the user within the system
        [Required]
        [Display(Name = "Role Assignment")]
        public string RoleName { get; set; } = string.Empty;
    }
}
