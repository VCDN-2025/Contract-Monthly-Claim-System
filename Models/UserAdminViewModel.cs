using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class UserAdminViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email (Username)")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Role Assignment")]
        public string RoleName { get; set; } = string.Empty;
    }
}