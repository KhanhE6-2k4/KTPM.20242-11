using MediaStore.Data;
using MediaStore.Helpers;
using MediaStore.Services;
using MediaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MediaStore.Controllers
{
    public class PayOrderController : Controller
    {
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

    }
}