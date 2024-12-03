using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Model.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShopingCartController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShopingCartModel shopingCartModel { get; set; }
        public ShopingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            string? userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shopingCartModel = new()
            {
                ShopingCartList = _unitOfWork.shopingCart.GetAll(d => d.ApplicationUserId == userId,
                                    includeProperties: "Product"),
                OrderHeader = new()

            };
            foreach (var item in shopingCartModel.ShopingCartList)
            {
                item.Price = GetPriceBasedOnQuantity(item);
                shopingCartModel.OrderHeader.OrderTotal += (item.Price * item.Count);
            }

            return View(shopingCartModel);
        }
        public IActionResult Plus(int cartId)
        {
            ShopingCart cart = _unitOfWork.shopingCart.Get(d => d.Id == cartId);
            if (cart != null)
            {
                cart.Count += 1;
                _unitOfWork.shopingCart.Update(cart);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Minus(int cartId)
        {
            ShopingCart cart = _unitOfWork.shopingCart.Get(d => d.Id == cartId);
            if (cart != null)
            {
                if (cart.Count <= 1)
                {
                    _unitOfWork.shopingCart.Remove(cart);
                }
                else
                {
                    cart.Count -= 1;
                    _unitOfWork.shopingCart.Update(cart);
                }
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Remove(int cartId)
        {
            ShopingCart cart = _unitOfWork.shopingCart.Get(d => d.Id == cartId);
            if (cart != null)
            {
                _unitOfWork.shopingCart.Remove(cart);
                _unitOfWork.Save();
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.shopingCart.GetAll(d=>d.ApplicationUserId==userId).Count());
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult OrderSummary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shopingCartModel = new()
            {
                ShopingCartList = _unitOfWork.shopingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };

            shopingCartModel.OrderHeader.ApplicationUser = _unitOfWork.applicationUser.Get(u => u.Id == userId);

            shopingCartModel.OrderHeader.Name = shopingCartModel.OrderHeader.ApplicationUser.Name;
            shopingCartModel.OrderHeader.PhoneNumber = shopingCartModel.OrderHeader.ApplicationUser.PhoneNumber;
            shopingCartModel.OrderHeader.StreetAddress = shopingCartModel.OrderHeader.ApplicationUser.StreetAddress;
            shopingCartModel.OrderHeader.City = shopingCartModel.OrderHeader.ApplicationUser.City;
            shopingCartModel.OrderHeader.State = shopingCartModel.OrderHeader.ApplicationUser.State;
            shopingCartModel.OrderHeader.PostalCode = shopingCartModel.OrderHeader.ApplicationUser.PostalCode;



            foreach (var cart in shopingCartModel.ShopingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                shopingCartModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(shopingCartModel);
        }

        [HttpPost]
        [ActionName("OrderSummary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shopingCartModel.ShopingCartList = _unitOfWork.shopingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");

            shopingCartModel.OrderHeader.OrderDate = System.DateTime.Now;
            shopingCartModel.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.applicationUser.Get(u => u.Id == userId);


            foreach (var cart in shopingCartModel.ShopingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                shopingCartModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            shopingCartModel.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            shopingCartModel.OrderHeader.OrderStatus = SD.StatusPending;
            _unitOfWork.orderHeader.Add(shopingCartModel.OrderHeader);
            _unitOfWork.Save();
            foreach (var cart in shopingCartModel.ShopingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = shopingCartModel.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.orderDetails.Add(orderDetail);
                _unitOfWork.Save();
            }
            //it is a regular customer account and we need to capture payment
            //stripe logic
            var domain = "https://localhost:44340/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"customer/ShipingCart/OrderConfirmation?id={shopingCartModel.OrderHeader.Id}",
                CancelUrl = domain + "customer/ShipingCart/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in shopingCartModel.ShopingCartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.orderHeader.UpdateStripePaymentID(shopingCartModel.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }
        public IActionResult OrderConfirmation(int id)
        {

            return View(id);
        }
        public double GetPriceBasedOnQuantity(ShopingCart shopingCart)
        {
            if (shopingCart.Count <= 50)
            {
                return shopingCart.Product.Price50;
            }
            else if (shopingCart.Count <= 100)
            {
                return shopingCart.Product.Price100;
            }
            else
            {
                return shopingCart.Product.Price;
            }
        }
    }
}
