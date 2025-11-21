/*
 * REFERENCE LIST
 * CHATGPT. Login POST Framework [Online]. Available at: https://chatgpt.com/share/6920927b-cc48-8000-9795-a3496f12211d 
 * MICROSOFT. ASP.NET Core Identity [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-10.0&tabs=visual-studio 
 */
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    // Represents the credential fields required for user login
    public class LoginViewModel
    {
        // Stores the user's login email address
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Holds the user's password used for authentication
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
