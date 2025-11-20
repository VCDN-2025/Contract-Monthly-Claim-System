using CMCS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CMCS.DataSeeding;

namespace CMCS.Controllers
{
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
            // Only validate the fields needed for rate setting (e.g., FullName and ContractHourlyRate)
            if (ModelState.IsValid)
            {
                // Ensure the entity is fully tracked before updating
                _dataRepository.UpdateLecturer(model);

                TempData["SuccessMessage"] = $"Rate for {model.FullName} updated successfully to R{model.ContractHourlyRate:N2}.";
                return RedirectToAction(nameof(ManageRates));
            }
            return View(model);
        }
    }
}