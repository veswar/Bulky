using System.Diagnostics;
using System.Security.Claims;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.product.GetAll();
            return View(products);
        }
        [Authorize]
        public IActionResult Details(int id)
        {
            ShopingCart shopingCart = new()
            {
                Product = _unitOfWork.product.Get(d => d.Id == id),
                Count = 1,
                ProductId = id
            };
            return View(shopingCart);
        }
        [HttpPost]
        public IActionResult Details(ShopingCart cart)
        {
            cart.Id = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            cart.ApplicationUserId = userId;
            cart.ProductId = cart.ProductId;
            var cartItem = _unitOfWork.shopingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == cart.ProductId);
            if (cartItem != null)
            {
                cartItem.Count = cartItem.Count + cart.Count;
                _unitOfWork.shopingCart.Update(cartItem);
            }
            else
            {
                _unitOfWork.shopingCart.Add(cart);
            }
            _unitOfWork.Save();
            cart.Product = new Product();
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
