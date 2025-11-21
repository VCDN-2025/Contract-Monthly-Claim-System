/*
 * REFERENCE LIST
 * NWONAH, R. Role-Based Access Control (RBAC) in C# and ASP.NET Core [Online]. Available at: https://medium.com/@nwonahr/role-based-access-control-rbac-in-c-and-asp-net-core-the-security-backbone-of-modern-apps-dea1204a0870 
 * MICROSOFT. ASP.NET Core Identity [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-10.0&tabs=visual-studio 
 * NWONAH, R. Working with Sessions and Cookies in ASP.NET Core [Online]. Available at: https://medium.com/@nwonahr/working-with-sessions-and-cookies-in-asp-net-core-013b24037d91 
 * MICROSOFT. Calculated Fields (Client-Side Auto-Calculation) [Online]. Available at: https://learn.microsoft.com/en-us/power-apps/maker/data-platform/define-calculated-fields 
 */
using CMCS.DataSeeding;
using CMCS.Models;
using CMCS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    [Authorize(Roles = DbSeeder.Roles.Lecturer)]
    public class ClaimStatusController : Controller
    {
        private readonly DataRepository _dataRepository;
        private readonly FileUploadService _fileUploadService;
        private readonly UserManager<IdentityUser> _userManager;

        // Constructor: Initializes the controller with the data repository and file upload service dependencies.
        public ClaimStatusController(
            DataRepository dataRepository,
            FileUploadService fileUploadService,
            UserManager<IdentityUser> userManager) // ADDITION: Inject user manager
        {
            _dataRepository = dataRepository;
            _fileUploadService = fileUploadService;
            _userManager = userManager; // ADDITION: Assign user manager
        }

        // Index (GET): Displays a list of all claims for the current lecturer ("LIC-101").
        public IActionResult Index()
        {
            // FIX: Retrieve claims based on logged-in user ID
            var currentUserId = _userManager.GetUserId(User);
            var claims = _dataRepository.GetAllClaims().Where(c => c.IdentityUserId == currentUserId).ToList();
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
                // Get logged-in user and lecturer profile (Rate Automation)
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                var lecturerProfile = _dataRepository.GetAllLecturers()
                    .FirstOrDefault(l => l.IdentityUserId == user.Id);

                if (lecturerProfile == null)
                {
                    TempData["ErrorMessage"] = "Error: Your lecturer profile is not linked or has not been set up by HR.";
                    return RedirectToAction(nameof(Index));
                }

                // rate validation
                if (model.HourlyRate != lecturerProfile.ContractHourlyRate)
                {
                    ModelState.AddModelError(nameof(model.HourlyRate),
                        $"Automation Rule Violated: Claim rate must match the official rate (R{lecturerProfile.ContractHourlyRate:N2}) set by HR. Your rate input has been ignored.");

                    return View(model);
                }

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

                //  Use IdentityUserId and HR-set rate
                var claim = new ClaimModel
                {
                    ClaimId = model.ClaimId,
                    LecturerId = lecturerProfile.LecturerId, // CMCS Lecturer ID
                    IdentityUserId = user.Id, // ASP.NET Identity User ID
                    SubmissionDate = DateTime.Now,
                    HoursWorked = model.HoursWorked,

                    // Use the official HR-set rate
                    HourlyRate = lecturerProfile.ContractHourlyRate,

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
        public IActionResult Details(string id)
        {
            var claim = _dataRepository.GetClaimById(id);
            var currentUserId = _userManager.GetUserId(User);

            if (claim == null || claim.IdentityUserId != currentUserId) 
            {
                return NotFound();
            }

            return View(claim);
        }
    }
}