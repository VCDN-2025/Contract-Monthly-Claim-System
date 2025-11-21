/*
 * REFERENCE LIST
 * NWONAH, R. Role-Based Access Control (RBAC) in C# and ASP.NET Core [Online]. Available at: https://medium.com/@nwonahr/role-based-access-control-rbac-in-c-and-asp-net-core-the-security-backbone-of-modern-apps-dea1204a0870 
 * MICROSOFT. ASP.NET Core Identity [Online]. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-10.0&tabs=visual-studio
 */
using CMCS.DataSeeding;
using CMCS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using System.Linq;

namespace CMCS.Controllers
{
    [Authorize(Roles = DbSeeder.Roles.HR)]
    public class LecturerInformationController : Controller
    {
        private readonly DataRepository _dataRepository;
        private readonly UserManager<IdentityUser> _userManager;

        //Initializes the controller with the data repository and user manager dependencies.
        public LecturerInformationController(DataRepository dataRepository, UserManager<IdentityUser> userManager) //Inject UserManager
        {
            _dataRepository = dataRepository;
            _userManager = userManager; // Assign UserManager
        }

        // Index (GET): Fetches and displays a list of all lecturer records.
        public IActionResult Index()
        {
            // Fetches all lecturer data from the database.
            var lecturers = _dataRepository.GetAllLecturers();
            return View(lecturers);
        }

        // Create (GET): Displays the form to create a new lecturer record.
        //unassigned Identity users for linking to a new lecturer profile.
        public IActionResult Create()
        {
            //Get all existing Identity Users
            var allUsers = _userManager.Users.ToList();

            //Get IDs of users already linked to a LecturerModel
            var linkedUserIds = _dataRepository.GetAllLecturers().Select(l => l.IdentityUserId).ToList();

            //Filter for users not yet linked
            var unassignedUsers = allUsers.Where(u => !linkedUserIds.Contains(u.Id))
                                          .OrderBy(u => u.Email)
                                          .ToList();

            //Pass the list of unassigned users to the view for the dropdown
            ViewBag.UnassignedUsers = new SelectList(unassignedUsers, "Id", "Email");

            return View(new LecturerModel());
        }

        // Create (POST): Submits the new lecturer data and adds the record to the repository.
        // Creates the Lecturer profile and links it to the selected Identity user.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LecturerModel model)
        {
            if (ModelState.IsValid)
            {
                _dataRepository.AddLecturer(model);
                //The IdentityUserId is now captured from the form and saved in the repository
                TempData["SuccessMessage"] = $"Lecturer {model.FullName} added and linked successfully!";
                return RedirectToAction(nameof(Index));
            }

            //If validation fails, refresh the ViewBag data for the dropdown before returning the view
            return Create();
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
                //Persists the updated lecturer data to the database.
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
            // Deletes the lecturer record from the database.
            _dataRepository.DeleteLecturer(id);
            TempData["SuccessMessage"] = $"Lecturer ID {id} deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}