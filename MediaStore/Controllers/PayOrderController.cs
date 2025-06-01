using System.Data.Common;
using System.Threading.Tasks;
using MediaStore.Data;
using MediaStore.Helpers;
using MediaStore.Services;
using MediaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MediaStore.Controllers
{
    public class PayOrderController : Controller
    {

        private readonly AimsContext db;

        public PayOrderController(AimsContext context)
        {
            db = context;
        }
        public IActionResult Invoice()
        {
            var order = HttpContext.Session.Get<Order>(MySetting.ORDER_KEY);
            var cart = order.cart; // Lay cart tu trong order ra
            var deliveryInfo = order.deliveryInfo;
            InvoiceViewModel invoice;
            if (deliveryInfo.IsRushOrder)
            {
                List<CartItem> regularItem = new List<CartItem>();
                int regularSubTotal = 0;
                List<CartItem> rushItem = new List<CartItem>();
                int rushSubTotal = 0;
                foreach (var item in cart)
                {
                    if (item.IsRushOrderSupported)
                    {
                        rushItem.Add(item);
                        rushSubTotal += item.Amount;
                    }
                    else
                    {
                        regularItem.Add(item);
                        regularSubTotal += item.Amount;
                    }
                }
                invoice = new InvoiceViewModel
                {
                    name = deliveryInfo.Name,
                    phone = deliveryInfo.Phone,
                    address = deliveryInfo.Address + "-" + deliveryInfo.Province,
                    email = deliveryInfo.Email,
                    hasRushOrder = true,
                    regularItem = regularItem,
                    regularSubTotal = regularSubTotal,
                    regularShippingFee = order.regularShippingFee,
                    rushItem = rushItem,
                    rushSubTotal = rushSubTotal,
                    rushShippingFee = order.rushShippingFee
                };
                HttpContext.Session.Set<InvoiceViewModel>(MySetting.INVOICE_KEY, invoice);
                return RedirectToAction("RushInvoice", "PayOrder");
            }
            invoice = new InvoiceViewModel
            {
                name = deliveryInfo.Name,
                phone = deliveryInfo.Phone,
                address = $"{deliveryInfo.Address} - {deliveryInfo.Province}",
                email = deliveryInfo.Email,
                hasRushOrder = false,
                regularItem = cart,
                regularSubTotal = cart.Sum(item => item.Amount),
                regularShippingFee = order.regularShippingFee
            };
            HttpContext.Session.Set<InvoiceViewModel>(MySetting.INVOICE_KEY, invoice);
            return View(invoice);
        }

        public IActionResult RushInvoice()
        {
            InvoiceViewModel invoice = HttpContext.Session.Get<InvoiceViewModel>(MySetting.INVOICE_KEY);
            return View(invoice);
        }


        public IActionResult PaymentSuccess()
        {
            return View();
        }
        public IActionResult PaymentFail()
        {
            return View();
        }
        public async Task<IActionResult> PaymentResult()
        {
            var msg = TempData.Peek("Message")?.ToString();
            var success = TempData.Peek("Success")?.ToString();

            // Giữ lại cả Message và Success cho lần redirect kế tiếp
            TempData.Keep("Message");
            TempData.Keep("Success");
            if (success == "no")
            {
                return RedirectToAction("PaymentFail");
            }
            TempData["Message"] = msg + " Please check your email for order's information";
            var paymentTransaction = HttpContext.Session.Get<PaymentTransaction>(MySetting.TRANSACTION_KEY);
            if (paymentTransaction != null)
            {
                var invoice = HttpContext.Session.Get<InvoiceViewModel>(MySetting.INVOICE_KEY);
                db.PaymentTransactions.Add(paymentTransaction);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("PaymentSuccess");
        }

    }
}