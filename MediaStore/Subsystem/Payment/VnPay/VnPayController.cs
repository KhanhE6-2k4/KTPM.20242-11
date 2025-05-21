using MediaStore.Helpers;
using MediaStore.Services;
using MediaStore.Services.Payment;
using MediaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MediaStore.Subsystem.VnPay
{
    public class VnPayController : Controller
    {
        private readonly VnPayService _vnPayService;

        public VnPayController(VnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        VnPaymentRequestModel createRequest(InvoiceViewModel invoice)
        {
            return new VnPaymentRequestModel
            {
                Amount = invoice.TotalPrice,
                CreateDate = DateTime.Now,
                Description = "Make a payment for the order",
                FullName = invoice.name,
                OrderId = new Random().Next(1000, 10000)
            };
        }

        public IActionResult Pay()
        {
            var invoice = HttpContext.Session.Get<InvoiceViewModel>(MySetting.INVOICE_KEY);
            var vnPaymentRequest = createRequest(invoice);
            return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPaymentRequest));
        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }
        public IActionResult PaymentFail()
        {
            return View();
        }
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Loi thanh toan VnPay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            // Luu don hang vao Database
            TempData["Message"] = $"Thanh toan VnPay thanh cong";
            return RedirectToAction("PaymentSuccess");
        }

    }
}