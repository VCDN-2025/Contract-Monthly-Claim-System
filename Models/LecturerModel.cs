/*
 * REFERENCE LIST
 * MICROSOFT. ASP.NET Core Identity [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-10.0&tabs=visual-studio 
 * MICROSOFT. Data Persistence Design Patterns [Online]. Available at: https://learn.microsoft.com/en-us/archive/msdn-magazine/2009/april/design-patterns-for-data-persistence 
 */
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{

    // Represents the core information about a contracted Lecturer.

    public class LecturerModel
    {
        // Property: Unique system-generated identifier for the lecturer.
        [Key]
        [Required]
        [Display(Name = "Lecturer ID")]
        public string LecturerId { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        // Foreign Key to the Identity User
        [Required]
        [StringLength(450)]
        public string IdentityUserId { get; set; } = string.Empty;

        // Property: The full name of the lecturer, required and limited in length.
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        // FIX: Remove [Required] and make nullable. The email is managed by the Identity User linked above.
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string? Email { get; set; } // Set as nullable string

        // Property: The contractually agreed hourly rate, required and within a set range.
        [Required(ErrorMessage = "Hourly Rate is required.")]
        [Range(100.0, 1500.0, ErrorMessage = "Rate must be between R100.00 and R1500.00.")]
        [Display(Name = "Contract Hourly Rate (R)")]
        public double ContractHourlyRate { get; set; }

        // Property: The start date of the lecturer's contract, required and formatted as a date.
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Contract Start Date")]
        public DateTime ContractStartDate { get; set; } = DateTime.Now;

        //The end date of the lecturer's contract, required and formatted as a date.
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Contract End Date")]
        public DateTime ContractEndDate { get; set; } = DateTime.Now.AddYears(1);

        // Property: Boolean flag indicating if the lecturer's contract is currently active.
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }
}