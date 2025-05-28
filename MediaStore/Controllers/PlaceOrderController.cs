using MediaStore.Controllers;
using MediaStore.Data;
using MediaStore.Exceptions;
using MediaStore.Helpers;
using MediaStore.Services;
using MediaStore.Services.Email;
using MediaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language;

namespace MediaStore.Controllers
{
    public class PlaceOrderController : Controller
    {
        private readonly AimsContext db;
        private readonly IShippingService _shippingService;

        private readonly EmailService _emailService;

        public PlaceOrderController(AimsContext context, IShippingService shippingService, EmailService emailService)
        {
            db = context;
            _shippingService = shippingService;
            _emailService = emailService;

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

        public async Task<IActionResult> ProcessAfterPay(string success)
        {
            if (success == "true")
            {

                var orderSession = HttpContext.Session.Get<Order>(MySetting.ORDER_KEY);
                var invoiceSession = HttpContext.Session.Get<InvoiceViewModel>(MySetting.INVOICE_KEY);
                var transactionSession = HttpContext.Session.Get<PaymentTransaction>(MySetting.TRANSACTION_KEY);


                var cart = orderSession.cart;
                var deliveryInfo = orderSession.deliveryInfo;
                var rushOrderInfo = orderSession.rushOrderInfo;
                var regularShippingFee = orderSession.regularShippingFee;
                var rushShippingFee = orderSession.rushShippingFee;

                var delivery = new DeliveryInfo
                {
                    Name = deliveryInfo.Name,

                    Phone = deliveryInfo.Phone,

                    Email = deliveryInfo.Email,

                    Province = deliveryInfo.Province,

                    Address = deliveryInfo.Address,

                    Message = deliveryInfo.Message
                };
                var order = new OrderInfo
                {
                    ShippingFees = regularShippingFee + (invoiceSession.hasRushOrder ? rushShippingFee : 0),
                    Subtotal = cart.Sum(item => item.Amount),
                    Status = "Pending",
                    Delivery = delivery
                };

                if (rushOrderInfo != null)
                {
                    var rush = new RushOrderInfo
                    {
                        DeliveryTime = rushOrderInfo.DeliveryTime,
                        Instruction = rushOrderInfo.Instruction == null ? "No" : rushOrderInfo.Instruction,
                        Order = order
                    };
                    order.RushOrderInfos.Add(rush);
                }

                var regularItem = invoiceSession.regularItem;
                var rushItem = invoiceSession.rushItem;
                OrderMedia orderMedia;
                if (regularItem != null)
                {
                    foreach (var item in regularItem)
                    {
                        orderMedia = new OrderMedia
                        {
                            MediaId = item.Id,
                            Quantity = item.Qty,
                            OrderType = 0
                        };
                        order.OrderMedia.Add(orderMedia);
                    }
                }
                if (rushItem != null)
                {
                    foreach (var item in rushItem)
                    {
                        orderMedia = new OrderMedia
                        {
                            MediaId = item.Id,
                            Quantity = item.Qty,
                            OrderType = 1
                        };
                        order.OrderMedia.Add(orderMedia);
                    }
                }

                var invoice = new Invoice
                {
                    TotalAmount = invoiceSession.TotalPrice,

                    Transaction = transactionSession,
                    Order = order
                };

                db.Invoices.Add(invoice);
                await db.SaveChangesAsync();

                // Dọn session
                // HttpContext.Session.Remove(MySetting.ORDER_KEY);
                // HttpContext.Session.Remove(MySetting.INVOICE_KEY);
                // HttpContext.Session.Remove(MySetting.TRANSACTION_KEY);
                // HttpContext.Session.Remove(MySetting.CART_KEY);
   
                HttpContext.Session.Clear();
                // Gui mail thanh cong den kh
                await _emailService.SendOrderConfirmationEmail(invoiceSession.email, order.OrderId.ToString(), invoiceSession.name);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}