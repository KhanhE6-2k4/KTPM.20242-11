using MediaStore.Data;
using MediaStore.Helpers;
using MediaStore.Services;
using MediaStore.Services.Payment;
using MediaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace MediaStore.Subsystem.VnPay
{
    public class VnPayController : Controller
    {
        private readonly IVnPayService _vnPayService;

        public VnPayController(IVnPayService vnPayService)
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
        // public IActionResult PaymentCallBack()
        // {
        //     var response = _vnPayService.PaymentExecute(Request.Query);
        //     if (response == null || response.VnPayResponseCode != "00")
        //     {
        //         TempData["Message"] = $"Loi thanh toan VnPay: {response.VnPayResponseCode}";
        //         return RedirectToAction("PaymentFail", "PayOrder");
        //     }

        //     // Luu don hang vao Database
        //     TempData["Message"] = $"Thanh toan VnPay thanh cong";
        //     return RedirectToAction("PaymentSuccess", "PayOrder");
        // }
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null)
            {
                TempData["Message"] = $"Unknown error.";
                return RedirectToAction("PaymentResult", "PayOrder");
            }
            var paymentTransaction = new PaymentTransaction
            {
                PaymentTime = ParseVnpDate(response.PayDate),
                PaymentAmount = response.Amount,
                Content = response.OrderDescription,
                BankTransactionId = response.BankCode,
                CardType = response.CardType
            };
            HttpContext.Session.Set<PaymentTransaction>(MySetting.TRANSACTION_KEY, paymentTransaction);
            var responseCode = response.VnPayResponseCode;
            TempData["Message"] = Message.GetMessage(responseCode);
            TempData["Success"] = responseCode == "00" ? "yes" : "no"; 
            return RedirectToAction("PaymentResult", "PayOrder");
        }
        private DateTime ParseVnpDate(string vnpDateString)
        {
            return DateTime.ParseExact(vnpDateString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
        }

    }
}