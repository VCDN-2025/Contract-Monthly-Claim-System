using CMCS.DataSeeding;
using CMCS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    [Authorize(Roles = DbSeeder.Roles.ProgrammeCoordinator)]
    public class ProgrammeCoOrdinatorController : Controller
    {
        private readonly DataRepository _dataRepository;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Verify(string id)
        {
            var claim = _dataRepository.GetClaimById(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.AwaitingAMApproval;
            claim.PcActionDate = DateTime.Now;
            _dataRepository.UpdateClaim(claim);

            TempData["SuccessMessage"] = $"Claim {id} verified. Moved to Academic Manager Approval.";
            return RedirectToAction(nameof(Index));
        }

        // Reject (POST): Changes the status of a specific claim to 'RejectedByPC' and records the action date.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

                var fileBytes = await System.IO.File.ReadAllBytesAsync(document.EncryptedFilePath);


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