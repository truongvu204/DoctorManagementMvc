using Microsoft.AspNetCore.Mvc;
using DoctorManagementMvc.Models;
using DoctorManagementMvc.Services;
using DoctorManagementMvc.Models;
using DoctorManagementMvc.Services;

namespace DoctorManagementMvc.Controllers
{
    public class DoctorsController : Controller
    {
        private static readonly DoctorRepository _repo = new DoctorRepository();

       
        public IActionResult Index(string? q)
        {
            var data = string.IsNullOrWhiteSpace(q) ? _repo.All() : _repo.Search(q);
            ViewBag.Query = q;
            return View(data);
        }

     
        public IActionResult Create() => View();

        
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Doctor doctor)
        {
            if (!ModelState.IsValid) return View(doctor);

            if (!_repo.Add(doctor, out var error))
            {
                ModelState.AddModelError("", error);
                return View(doctor);
            }
            TempData["msg"] = "Doctor added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(string id)
        {
            var doc = _repo.GetByCode(id);
            if (doc == null)
            {
                TempData["msg"] = "Doctor code does not exist.";
                return RedirectToAction(nameof(Index)); 
            }
            return View(doc);
        }

     
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Doctor doctor)
        {
            if (!ModelState.IsValid) return View(doctor);

            if (!_repo.Update(doctor, out var error))
            {
                ModelState.AddModelError("", error);
                return View(doctor);
            }
            TempData["msg"] = "Doctor updated successfully.";
            return RedirectToAction(nameof(Index));
        }
 
        public IActionResult Delete(string id)
        {
            var doc = _repo.GetByCode(id);
            if (doc == null)
            {
                TempData["msg"] = "Doctor code does not exist.";
                return RedirectToAction(nameof(Index));
            }
            return View(doc);
        }

        
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            if (!_repo.Delete(id, out var error))
                TempData["msg"] = error;
            else
                TempData["msg"] = "Doctor deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
