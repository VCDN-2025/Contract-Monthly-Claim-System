using CMCS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CMCS.DataSeeding;
using System.Linq;

namespace CMCS.Controllers
{
    //Implements Role-Based Access Control, restricting access to the HR role
    [Authorize(Roles = DbSeeder.Roles.HR)]
    public class HRController : Controller
    {
        private readonly DataRepository _dataRepository;

        public HRController(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
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
            // Only validate the fields needed for rate setting (Data Annotations on model will handle the rest)
            if (ModelState.IsValid)
            {
                //Persists  the updated lecturer rate to the database
                _dataRepository.UpdateLecturer(model);

                TempData["SuccessMessage"] = $"Rate for {model.FullName} updated successfully to R{model.ContractHourlyRate:N2}.";
                return RedirectToAction(nameof(ManageRates));
            }
            return View(model);
        }

        // GET: HR/Reports (Initial view for the report generator)
        // Implements filtering for claims reports using multiple criteria
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
        public IActionResult Reports(ReportFilterViewModel model)
        {
            //Constructs a LINQ query to filter claims data based on user input
            var claimsQuery = _dataRepository.GetAllClaims().AsQueryable();

            //Filter by Claim ID
            if (!string.IsNullOrEmpty(model.ClaimId))
            {
                claimsQuery = claimsQuery.Where(c => c.ClaimId.Contains(model.ClaimId));
            }

            //Filter by Status
            if (model.Status.HasValue)
            {
                claimsQuery = claimsQuery.Where(c => c.Status == model.Status.Value);
            }

            //Filter by Amount Range
            if (model.MinAmount.HasValue)
            {
                claimsQuery = claimsQuery.Where(c => c.ClaimAmount >= model.MinAmount.Value);
            }
            if (model.MaxAmount.HasValue)
            {
                claimsQuery = claimsQuery.Where(c => c.ClaimAmount <= model.MaxAmount.Value);
            }

            //Filter by Date Range
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