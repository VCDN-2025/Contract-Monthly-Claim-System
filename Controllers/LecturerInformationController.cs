using CMCS.DataSeeding;
using CMCS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    [Authorize(Roles = DbSeeder.Roles.HR)]
    public class LecturerInformationController : Controller
    {
        private readonly DataRepository _dataRepository;

        // Constructor: Initializes the controller with the data repository dependency.
        public LecturerInformationController(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        // Index (GET): Fetches and displays a list of all lecturer records.
        public IActionResult Index()
        {
            // Fetch all lecturer data from the repository
            var lecturers = _dataRepository.GetAllLecturers();
            return View(lecturers);
        }

        // Create (GET): Displays the form to create a new lecturer record.
        public IActionResult Create()
        {
            return View(new LecturerModel());
        }

        // Create (POST): Submits the new lecturer data and adds the record to the repository.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LecturerModel model)
        {
            if (ModelState.IsValid)
            {
                _dataRepository.AddLecturer(model);
                TempData["SuccessMessage"] = $"Lecturer {model.FullName} added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Edit (GET): Fetches and displays the edit form for a specific lecturer ID.
        public IActionResult Edit(string id)
        {
            var lecturer = _dataRepository.GetLecturerById(id);
            if (lecturer == null) return NotFound();
            return View(lecturer);
        }

        // Edit (POST): Submits updated lecturer data and saves the changes to the repository.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LecturerModel model)
        {
            if (ModelState.IsValid)
            {
                _dataRepository.UpdateLecturer(model);
                TempData["SuccessMessage"] = $"Lecturer {model.FullName} updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Details (GET): Fetches and displays the details of a specific lecturer ID.
        public IActionResult Details(string id)
        {
            var lecturer = _dataRepository.GetLecturerById(id);
            if (lecturer == null) return NotFound();
            return View(lecturer);
        }

        // Delete (GET): Displays the confirmation page for deleting a specific lecturer record.
        public IActionResult Delete(string id)
        {
            var lecturer = _dataRepository.GetLecturerById(id);
            if (lecturer == null) return NotFound();
            return View(lecturer);
        }

        // DeleteConfirmed (POST): Finalizes the deletion of a specific lecturer record from the repository.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _dataRepository.DeleteLecturer(id);
            TempData["SuccessMessage"] = $"Lecturer ID {id} deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}