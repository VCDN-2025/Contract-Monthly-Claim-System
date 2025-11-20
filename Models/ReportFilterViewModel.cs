using CMCS.Models;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class ReportFilterViewModel
    {
        [Display(Name = "Claim ID")]
        public string? ClaimId { get; set; }

        [Display(Name = "Lecturer Name")]
        public string? LecturerName { get; set; }

        [Display(Name = "Status")]
        public ClaimStatus? Status { get; set; }

        [Display(Name = "Min Amount")]
        public double? MinAmount { get; set; }

        [Display(Name = "Max Amount")]
        public double? MaxAmount { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        // Property to hold the filtered results
        public List<ClaimModel> FilteredClaims { get; set; } = new List<ClaimModel>();
    }
}