using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        internal readonly IUnitOfWork _unitOfWork;
        internal readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.product.GetAll().ToList();

            return View(products);
        }
        public IActionResult Upsert(int? id)
        {
            ProductModel product = new()
            {
                Categories = _unitOfWork.category.GetAll()
              .Select(d => new SelectListItem
              {
                  Text = d.Name,
                  Value = d.Id.ToString()
              }).ToList()
            };
            if (id != null)
            {
                product.Product = _unitOfWork.product.Get(d => d.Id == id);
            }
            else
            {
                product.Product = new Product();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult Upsert(ProductModel productModel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (productModel.Product.Id == 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (wwwRootPath != null && file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePath = Path.Combine(wwwRootPath, @"images\product");
                        using (var fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        productModel.Product.ImageUrl = Path.Combine(filePath, fileName);
                    }

                    _unitOfWork.product.Add(productModel.Product);
                }
                else
                {
                    _unitOfWork.product.update(productModel.Product);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(productModel);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ProductModel productModel = new()
            {
                Product = _unitOfWork.product.Get(d => d.Id == id),
                Categories = _unitOfWork.category.GetAll()
              .Select(d => new SelectListItem
              {
                  Text = d.Name,
                  Value = d.Id.ToString()
              }).ToList()

            };
            if (productModel.Product == null)
            {
                return NotFound();
            }
            return View(productModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ProductModel productModel = new()
            {
                Product = _unitOfWork.product.Get(d => d.Id == id),
                Categories = _unitOfWork.category.GetAll()
              .Select(d => new SelectListItem
              {
                  Text = d.Name,
                  Value = d.Id.ToString()
              }).ToList()

            };
            if (productModel.Product == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.product.Remove(productModel.Product);
                _unitOfWork.Save();
                TempData["success"] = "Record deleted successfully";
            }
            //return RedirectToAction("Index");
            return Json(new { success = true, message = "Record deleted successfully" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.product.GetAll().ToList();

            return Json(new { data = products });
        }
    }
}
