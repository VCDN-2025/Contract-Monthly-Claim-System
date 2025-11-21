/*
 * REFERENCE LIST
 * NWONAH, R. Role-Based Access Control (RBAC) in C# and ASP.NET Core [Online]. Available at: https://medium.com/@nwonahr/role-based-access-control-rbac-in-c-and-asp-net-core-the-security-backbone-of-modern-apps-dea1204a0870 
 * MICROSOFT. Data Persistence Design Patterns [Online]. Available at: https://learn.microsoft.com/en-us/archive/msdn-magazine/2009/april/design-patterns-for-data-persistence 
 */
using CMCS.DataSeeding;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CMCS.Controllers
{
    
    [Authorize(Roles = DbSeeder.Roles.ProgrammeCoordinator)]
    public class ProgrammeCoOrdinatorController : Controller
    {
        private readonly DataRepository _dataRepository;
        private const double MAX_HOURS_PER_MONTH = 150.0; // Define the policy limit for automation

        // Constructor: Initializes the controller with the data repository dependency.
        public ProgrammeCoOrdinatorController(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        // Index (GET): Displays a list of claims that are awaiting Programme Co-ordinator (PC) verification.
        public IActionResult Index()
        {
            var claims = _dataRepository.GetAllClaims()
                .Where(c => c.Status == ClaimStatus.AwaitingPCVerification)
                .ToList();

            return View(claims);
        }

        // Verify (POST): Moves a claim from PC Verification to Awaiting AM Approval and records the action date.
        // [In-text reference: Automates claim verification against MAX_HOURS_PER_MONTH policy.]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Verify(string id)
        {
            var claim = _dataRepository.GetClaimById(id);
            if (claim == null) return NotFound();

            
            // Business Rule Check Hours Policy 
          
            if (claim.HoursWorked > MAX_HOURS_PER_MONTH)
            {
                // If the claim violates the policy, automatically reject it and stop verification
                TempData["ErrorMessage"] = $"AUTOMATED REJECTION: Claim {id} violates policy. Claimed hours ({claim.HoursWorked}h) exceed the limit of {MAX_HOURS_PER_MONTH} hours.";

                // Set to RejectedByPC status automatically
                claim.Status = ClaimStatus.RejectedByPC;
                claim.PcActionDate = DateTime.Now;
                _dataRepository.UpdateClaim(claim);

                return RedirectToAction(nameof(Index));
            }
           

            // If the policy check passes:
            claim.Status = ClaimStatus.AwaitingAMApproval; // Move to next stage
            claim.PcActionDate = DateTime.Now;
            _dataRepository.UpdateClaim(claim);

            TempData["SuccessMessage"] = $"Claim {id} verified and meets policy. Moved to Academic Manager Approval.";
            return RedirectToAction(nameof(Index));
        }

        // Reject (POST): Changes the status of a specific claim to 'RejectedByPC' and records the action date.
        public IActionResult Reject(string id)
        {
            var claim = _dataRepository.GetClaimById(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.RejectedByPC;
            claim.PcActionDate = DateTime.Now;
            _dataRepository.UpdateClaim(claim);

            TempData["ErrorMessage"] = $"Claim {id} rejected and set to final status.";
            return RedirectToAction(nameof(Index));
        }

        // DownloadDocument (GET/POST): Retrieves a claim's document file from storage and serves it as a file download.
        public async Task<IActionResult> DownloadDocument(string claimId)
        {
            var claim = _dataRepository.GetClaimById(claimId);
        
            var document = _dataRepository.GetDocumentById(claim?.DocumentId ?? "");

            if (document == null || !System.IO.File.Exists(document.EncryptedFilePath))
            {
                TempData["ErrorMessage"] = "Document not found or the storage path is invalid.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                //Retrieves the document file for download.]
                var fileBytes = await System.IO.File.ReadAllBytesAsync(document.EncryptedFilePath);

                // Determines the correct MIME type based on file extension
                string mimeType = Path.GetExtension(document.OriginalFileName).ToLowerInvariant() switch
                {
                    ".pdf" => "application/pdf",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    _ => "application/octet-stream",
                };

                return File(fileBytes, mimeType, document.OriginalFileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred reading the secure file: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}