using CMCS.DataSeeding;
using CMCS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CMCS.Controllers
{
    //Implements Role-Based Access Control, restricting access to the HR role.
    [Authorize(Roles = DbSeeder.Roles.HR)]
    public class HRController : Controller
    {
        private readonly DataRepository _dataRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

       //Initializes the controller with the data repository, user manager, and role manager dependencies.]
        public HRController(DataRepository dataRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dataRepository = dataRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: HR/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: HR/ManageRates (Displays all lecturers for rate setting)
        public IActionResult ManageRates()
        {
            var lecturers = _dataRepository.GetAllLecturers();
            return View(lecturers);
        }

        // GET: HR/EditRate/id (View to edit a single lecturer's rate)
        public IActionResult EditRate(string id)
        {
            var lecturer = _dataRepository.GetLecturerById(id);
            if (lecturer == null) return NotFound();
            return View(lecturer);
        }

        // POST: HR/EditRate (Handles rate update)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRate(LecturerModel model)
        {
            if (ModelState.IsValid)
            {
                _dataRepository.UpdateLecturer(model);

                TempData["SuccessMessage"] = $"Rate for {model.FullName} updated successfully to R{model.ContractHourlyRate:N2}.";
                return RedirectToAction(nameof(ManageRates));
            }
            return View(model);
        }

        // GET: HR/CreateUser
        public IActionResult CreateUser()
        {
            // Get all role names for the dropdown list (Lecturer, PC, AM, etc.)
            // NOTE: DbSeeder.Roles contains the role constants
            ViewBag.Roles = new SelectList(_roleManager.Roles.Select(r => r.Name).ToList());
            return View(new UserAdminViewModel());
        }

        // POST: HR/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };

                // 1. Create the user
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // 2. Assign the selected role (PC, AM, Lecturer, etc.)
                    await _userManager.AddToRoleAsync(user, model.RoleName);

                    TempData["SuccessMessage"] = $"User {model.Email} created and assigned the '{model.RoleName}' role successfully. Link contract details via Lecturer Info.";

                    return RedirectToAction(nameof(Index));
                }

                // If creation failed, add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Re-fetch ViewBag data if validation fails
            ViewBag.Roles = new SelectList(_roleManager.Roles.Select(r => r.Name).ToList());
            return View(model);
        }

        // GET: HR/Reports (Initial view for the report generator)
        public IActionResult Reports()
    {
        // Display all claims initially when no filters are applied
        var model = new ReportFilterViewModel
        {
            FilteredClaims = _dataRepository.GetAllClaims().ToList()
        };
        return View(model);
    }

    // POST: HR/Reports (Handles form submission and applies filters)
    [HttpPost]
    [ValidateAntiForgeryToken]
   // Constructs a complex LINQ query to filter claims data based on user input(Part 3 Requirement).]
    public IActionResult Reports(ReportFilterViewModel model)
    {
        var claimsQuery = _dataRepository.GetAllClaims().AsQueryable();

        // Filter by Claim ID
        if (!string.IsNullOrEmpty(model.ClaimId))
        {
            claimsQuery = claimsQuery.Where(c => c.ClaimId.Contains(model.ClaimId));
        }

        // Filter by Status
        if (model.Status.HasValue)
        {
            claimsQuery = claimsQuery.Where(c => c.Status == model.Status.Value);
        }

        // Filter by Amount Range
        if (model.MinAmount.HasValue)
        {
            claimsQuery = claimsQuery.Where(c => c.ClaimAmount >= model.MinAmount.Value);
        }
        if (model.MaxAmount.HasValue)
        {
            claimsQuery = claimsQuery.Where(c => c.ClaimAmount <= model.MaxAmount.Value);
        }

        // Filter by Date Range
        if (model.StartDate.HasValue)
        {
            claimsQuery = claimsQuery.Where(c => c.SubmissionDate >= model.StartDate.Value);
        }
        // Use AddDays(1) to include the entire EndDate
        if (model.EndDate.HasValue)
        {
            claimsQuery = claimsQuery.Where(c => c.SubmissionDate < model.EndDate.Value.AddDays(1));
        }

        // Execute the query
        model.FilteredClaims = claimsQuery.ToList();

        return View(model);
    }
}
}