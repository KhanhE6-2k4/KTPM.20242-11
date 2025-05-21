using System.Data.Common;
using MediaStore.Data;
using MediaStore.Exceptions;
using MediaStore.Helpers;
using MediaStore.Models;
using MediaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MediaStore.Controllers
{
    public class CartController : Controller
    {
        private readonly AimsContext db;
        public CartController(AimsContext context)
        {
            db = context;
        }


        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
        {
            return View(Cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int id, int qty = 1)
        {
            if (qty < 0)
            {
                return Json(new { success = false, message = "Invalid quantity!" });
            }
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.Id == id);
            var product = db.Media.SingleOrDefault(p => p.MediaId == id);
            if (product == null)
            {
                TempData["Message"] = "Can't find this product";
                return Redirect("/404");
            }
            if (item == null)
            {
                if (qty > product.TotalQuantity)
                {
                    return Json(new { success = false, message = "Not enough stock available. You can add a maximum of " + (product.TotalQuantity) + " item(s) of this product." });
                }
                item = new CartItem
                {
                    Id = product.MediaId,
                    ImageUrl = product.ImageUrl,
                    Title = product.Title,
                    Price = product.Price,
                    Weight = product.Weight,
                    Qty = qty,
                    TotalQuantity = product.TotalQuantity,
                    IsRushOrderSupported = (bool)product.RushOrderSupported
                };
                cart.Add(item);
            }
            else
            {

                if (item.Qty + qty > product.TotalQuantity)
                {
                    return Json(new { success = false, message = "Not enough stock available. You can add a maximum of " + (product.TotalQuantity - (item.Qty)) + " item(s) of this product." });
                }

                item.Qty += qty;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, cart);
            // return RedirectToAction("Index");
            return Json(new { success = true, cartItemCount = cart.Count() });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.Id == id);
            if (item == null)
            {
                return Json(new { success = false, message = "Item not found." });
            }
            cart.Remove(item);
            HttpContext.Session.Set(MySetting.CART_KEY, cart);
            return Json(new { success = true });
        }

        [HttpPost]
        [HttpPost]
        public IActionResult UpdateProductQty([FromBody] UpdateQuantityRequest req)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.Id == req.Id);
            var product = db.Media.FirstOrDefault(p => p.MediaId == req.Id);
            if (item == null || product == null)
                return Json(new { success = false, message = "Item not found." });

            if (req.Action == "plus" && item.Qty < product.TotalQuantity)
                item.Qty++;
            else if (req.Action == "minus" && item.Qty > 1)
                item.Qty--;
            else if (req.Action == "set")
            {
                var newQty = Math.Clamp(req.Qty, 1, product.TotalQuantity);
                item.Qty = newQty;
            }

            HttpContext.Session.Set(MySetting.CART_KEY, cart);
            return Json(new { success = true, newQty = item.Qty, newAmount = item.Amount });
        }

        public IActionResult PopUp()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CartCount()
        {
            return Json(new { count = Cart.Count });
        }

        [HttpGet]
        public IActionResult AmountCount()
        {
            return Json(new { amount = Cart.Sum(p => p.Amount) });

        }
        public IActionResult Checkout()
        {
            var cart = Cart;
            try
            {
                if (cart == null || !cart.Any())
                {
                    throw new EmptyCartWhenCheckoutException("Cannot checkout because cart is empty!");
                }
                return RedirectToAction("DeliveryInfo", "PlaceOrder");

            }
            catch (CartException ex)
            {
                TempData["CartError"] = ex.Message;
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}