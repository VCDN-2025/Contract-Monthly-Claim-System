using CMCS.DataSeeding;
using CMCS.Models;
using CMCS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    [Authorize(Roles = DbSeeder.Roles.Lecturer)]
    public class ClaimStatusController : Controller
    {
        private readonly DataRepository _dataRepository;
        private readonly FileUploadService _fileUploadService;

        // Constructor: Initializes the controller with the data repository and file upload service dependencies.
        public ClaimStatusController(DataRepository dataRepository, FileUploadService fileUploadService)
        {
            _dataRepository = dataRepository;
            _fileUploadService = fileUploadService;
        }

        // Index (GET): Displays a list of all claims for the current lecturer ("LIC-101").
        public IActionResult Index()
        {
            var claims = _dataRepository.GetAllClaims().Where(c => c.LecturerId == "LIC-101").ToList();
            return View(claims);
        }

        // Create (GET): Displays the form for a lecturer to create a new claim.
        public IActionResult Create()
        {
            return View(new ClaimInputViewModel());
        }

        // Create (POST): Submits a new claim, handles document upload, and saves the claim and document records.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClaimInputViewModel model)
        {
            if (ModelState.IsValid)
            {

                var (success, message, documentModel) = await _fileUploadService.ProcessUploadAsync(model.SupportingDocument, model.ClaimId);

                if (!success)
                {
                    ModelState.AddModelError("SupportingDocument", message);
                    return View(model);
                }

                if (documentModel == null)
                {

                    TempData["ErrorMessage"] = "A critical error occurred while creating the document record.";
                    return RedirectToAction(nameof(Index));
                }

                var claim = new ClaimModel
                {
                    ClaimId = model.ClaimId,
                    LecturerId = "LIC-101",
                    SubmissionDate = DateTime.Now,
                    HoursWorked = model.HoursWorked,
                    HourlyRate = model.HourlyRate,
                    AdditionalNotes = model.AdditionalNotes,
                    DocumentId = documentModel!.DocumentId,
                    Status = ClaimStatus.AwaitingPCVerification
                };

                _dataRepository.AddDocument(documentModel!);
                _dataRepository.AddClaim(claim);

                TempData["SuccessMessage"] = $"Claim {claim.ClaimId} submitted successfully and awaiting Programme Co-ordinator verification.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}