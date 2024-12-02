using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        internal readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.category.GetAll().ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name and DisplayOrder should not be same");
            }
            if (category.Name != null && category.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "test is invalid");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Record created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _unitOfWork.category.Get(d => d.Id == id);
            if (category == null) { return NotFound(); }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return Edit(category.Id);
            }
            _unitOfWork.category.Update(category);
            _unitOfWork.Save();
            TempData["success"] = "Record updated successfully";
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _unitOfWork.category.Get(d => d.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _unitOfWork.category.Get(d => d.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.category.Remove(category);
                _unitOfWork.Save();
                TempData["success"] = "Record deleted successfully";
            }
            return RedirectToAction("Index");
        }

    }
}
