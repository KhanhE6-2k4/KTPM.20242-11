using MediaStore.Controllers;
using MediaStore.Data;
using MediaStore.Exceptions;
using MediaStore.Helpers;
using MediaStore.Services;
using MediaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediaStore.Controllers
{
    public class PlaceOrderController : Controller
    {
        private readonly AimsContext db;
        private readonly IShippingService _shippingService;

        public PlaceOrderController(AimsContext context, IShippingService shippingService)
        {
            db = context;
            _shippingService = shippingService;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DeliveryInfo()
        {
            ViewBag.ProvinceList = new SelectList(GetVietnamProvinces());
            return View();
        }
        [HttpPost]
        public IActionResult DeliveryInfo(DeliveryForm deliveryform)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProvinceList = new SelectList(GetVietnamProvinces());
                return View(deliveryform);
            }
            HttpContext.Session.Set<DeliveryForm>(MySetting.DELIVERY_KEY, deliveryform);
            if (deliveryform.IsRushOrder)
            {
                return RedirectToAction("RushOrder", "PlaceRushOrder");
            }
            return RedirectToAction("UpdateOrderInfo", "PlaceOrder");
        }

        public IActionResult UpdateOrderInfo()
        {
            var myCart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY);
            var MyDeliveryInfo = HttpContext.Session.Get<DeliveryForm>(MySetting.DELIVERY_KEY);
            var MyRushOrderInfo = HttpContext.Session.Get<RushOrderForm>(MySetting.RUSH_ORDER_KEY);
            var order = new Order
            {
                cart = myCart,
                deliveryInfo = MyDeliveryInfo,
                rushOrderInfo = MyRushOrderInfo,
                regularShippingFee = _shippingService.CalculateRegularFee(MyDeliveryInfo, myCart),
                rushShippingFee = _shippingService.CalculateRushFee(MyDeliveryInfo, myCart)
            };
            HttpContext.Session.Set<Order>(MySetting.ORDER_KEY, order);
            return RedirectToAction("Invoice", "PayOrder");
        }
        private List<string> GetVietnamProvinces()
        {
            return new List<string>
        {
            "An Giang", "Bà Rịa - Vũng Tàu", "Bắc Giang", "Bắc Kạn", "Bạc Liêu",
            "Bắc Ninh", "Bến Tre", "Bình Định", "Bình Dương", "Bình Phước", "Bình Thuận",
            "Cà Mau", "Cần Thơ", "Cao Bằng", "Đà Nẵng", "Đắk Lắk", "Đắk Nông",
            "Điện Biên", "Đồng Nai", "Đồng Tháp", "Gia Lai", "Hà Giang", "Hà Nam",
            "Hà Nội", "Hà Tĩnh", "Hải Dương", "Hải Phòng", "Hậu Giang", "Hòa Bình",
            "Hưng Yên", "Khánh Hòa", "Kiên Giang", "Kon Tum", "Lai Châu", "Lâm Đồng",
            "Lạng Sơn", "Lào Cai", "Long An", "Nam Định", "Nghệ An", "Ninh Bình",
            "Ninh Thuận", "Phú Thọ", "Phú Yên", "Quảng Bình", "Quảng Nam", "Quảng Ngãi",
            "Quảng Ninh", "Quảng Trị", "Sóc Trăng", "Sơn La", "Tây Ninh", "Thái Bình",
            "Thái Nguyên", "Thanh Hóa", "Thừa Thiên Huế", "Tiền Giang", "TP. Hồ Chí Minh",
            "Trà Vinh", "Tuyên Quang", "Vĩnh Long", "Vĩnh Phúc", "Yên Bái"
        };
        }

    }
}