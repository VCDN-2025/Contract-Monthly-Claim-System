/*
 * REFERENCE LIST
 * NWONAH, R. Role-Based Access Control (RBAC) in C# and ASP.NET Core [Online]. Available at: https://medium.com/@nwonahr/role-based-access-control-rbac-in-c-and-asp-net-core-the-security-backbone-of-modern-apps-dea1204a0870 
 * MICROSOFT. Data Persistence Design Patterns [Online]. Available at: https://learn.microsoft.com/en-us/archive/msdn-magazine/2009/april/design-patterns-for-data-persistence 
 */
using CMCS.DataSeeding;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace CMCS.Controllers
{
    [Authorize(Roles = DbSeeder.Roles.AcademicManager)]
    public class AcademicManagerController : Controller
    {
        private readonly DataRepository _dataRepository;

        // Constructor: Initializes the controller with the data repository dependency.
        public AcademicManagerController(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        // Index (GET): Displays a list of claims that are awaiting Academic Manager (AM) approval.
        public IActionResult Index()
        {
            var claims = _dataRepository.GetAllClaims()
                .Where(c => c.Status == ClaimStatus.AwaitingAMApproval)
                .ToList();

            return View(claims);
        }

        // Approve (POST): Changes the status of a specific claim to 'Approved' and records the action date.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(string id)
        {
            var claim = _dataRepository.GetClaimById(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Approved;
            claim.AmActionDate = DateTime.Now;
            _dataRepository.UpdateClaim(claim);

            TempData["SuccessMessage"] = $"Claim {id} APPROVED. Payment authorized.";
            return RedirectToAction(nameof(Index));
        }

        // Reject (POST): Changes the status of a specific claim to 'RejectedByAM' and records the action date.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(string id)
        {
            var claim = _dataRepository.GetClaimById(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.RejectedByAM; // Final rejection
            claim.AmActionDate = DateTime.Now;
            _dataRepository.UpdateClaim(claim);

            TempData["ErrorMessage"] = $"Claim {id} REJECTED. Status is final.";
            return RedirectToAction(nameof(Index));
        }

    }
}