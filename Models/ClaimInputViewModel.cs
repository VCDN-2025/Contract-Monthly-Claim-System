using CMCS.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) for the Claim creation view/form input.
    /// </summary>
    public class ClaimInputViewModel
    {
        // Property: Unique system-generated ID for the claim (used for document linking).
        public string ClaimId { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

        // Property: Total hours worked by the lecturer, subject to validation.
        [Required(ErrorMessage = "Hours worked is required.")]
        [Range(1.0, 300.0, ErrorMessage = "Hours must be between 1 and 300.")]
        [Display(Name = "Total Hours Worked")]
        public double HoursWorked { get; set; }

        // Property: The agreed-upon hourly rate (in Rands), subject to validation.
        [Required(ErrorMessage = "Hourly rate is required.")]
        [Range(100.0, 1000.0, ErrorMessage = "Rate must be between R100.00 and R1000.00.")]
        [Display(Name = "Agreed Hourly Rate (R)")]
        public double HourlyRate { get; set; }

        // Property: Optional field for any additional notes, limited to 500 characters.
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        [Display(Name = "Additional Notes")]
        [DataType(DataType.MultilineText)]
        public string? AdditionalNotes { get; set; }

        // Property: The file uploaded by the lecturer (e.g., timesheet), which is required.
        [Required(ErrorMessage = "A supporting document is required.")]
        [Display(Name = "Supporting Document (.pdf, .docx, .xlsx)")]
        public IFormFile? SupportingDocument { get; set; }
    }
}