using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    /// <summary>
    /// Represents a Lecturer's Claim for work done, holding all necessary data and status information.
    /// </summary>
    public class ClaimModel
    {
        //System Identification Fields
        // Property: Unique identifier for the claim, auto-generated upon creation.
        [Required]
        public string ClaimId { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

        // Property: The unique ID of the Lecturer who submitted the claim.
        [Required]
        public string LecturerId { get; set; } = "LIC-101";

        // Property: The date the claim was submitted, defaulted to the creation time.
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Submission Date")]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        //Lecturer Input Fields
        // Property: The total number of hours worked, required and constrained by a range.
        [Required(ErrorMessage = "Hours worked is required.")]
        [Range(1.0, 300.0, ErrorMessage = "Hours must be between 1 and 300.")]
        [Display(Name = "Total Hours Worked")]
        public double HoursWorked { get; set; }

        // Property: The agreed hourly rate (R), required and constrained by a range.
        [Required(ErrorMessage = "Hourly rate is required.")]
        [Range(100.0, 1000.0, ErrorMessage = "Rate must be between R100.00 and R1000.00.")]
        [Display(Name = "Agreed Hourly Rate (R)")]
        public double HourlyRate { get; set; }

        // Property: Optional field for additional notes related to the claim.
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        [Display(Name = "Additional Notes")]
        [DataType(DataType.MultilineText)]
        public string? AdditionalNotes { get; set; }

        //Financial & Document Fields
        // Property: Calculated read-only property for the total claim amount (Hours * Rate).
        [Display(Name = "Claim Amount")]
        [DataType(DataType.Currency)]
        public double ClaimAmount => HoursWorked * HourlyRate;

        // Property: Foreign key linking this claim to the associated DocumentModel record.
        public string DocumentId { get; set; } = string.Empty;


        //Status Tracking Fields 
        // Property: The current status of the claim in the workflow, mandatory.
        [Required]
        public ClaimStatus Status { get; set; } = ClaimStatus.PendingSubmission;

        // Property: The date the Programme Co-ordinator took action on the claim, if any.
        [Display(Name = "Programme Co-ordinator Action Date")]
        public DateTime? PcActionDate { get; set; }

        // Property: The date the Academic Manager took action on the claim, if any.
        [Display(Name = "Academic Manager Action Date")]
        public DateTime? AmActionDate { get; set; }
    }
}