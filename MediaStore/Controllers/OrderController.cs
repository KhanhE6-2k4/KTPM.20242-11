using MediaStore.Controllers;
using MediaStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MediaStore.ViewModels;

namespace MediaStore.Controllers
{
    [Authorize(Roles = "Customer")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class OrderController : Controller
    {
        private readonly AimsContext db;
        public OrderController(AimsContext context)
        {
            db = context;
        }

        public IActionResult Index(string keyword)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);

            var ordersQuery = db.OrderInfos
                .Where(o => o.Delivery.Email == email)
                .Include(o => o.Delivery)
                .Include(o => o.OrderMedia).ThenInclude(om => om.Media)
                .Include(o => o.RushOrderInfos)
                .Include(o => o.Invoices).ThenInclude(i => i.Transaction)
                .AsQueryable();

            // Nếu có từ khóa, lọc theo tên người nhận, địa chỉ, trạng thái, hoặc tiêu đề sản phẩm
            if (!string.IsNullOrEmpty(keyword))
            {
                ordersQuery = ordersQuery.Where(o =>
                    // o.Delivery.Name.Contains(keyword) ||
                    // o.Delivery.Address.Contains(keyword) ||
                    // o.Status.Contains(keyword) ||
                    // o.OrderMedia.Any(m => m.Media.Title.Contains(keyword)) || 
                    o.OrderId.ToString().Contains(keyword)
                );
            }

            var orders = ordersQuery
                .Select(o => new
                {
                    OrderId = o.OrderId,
                    CustomerName = o.Delivery.Name,
                    Address = o.Delivery.Address,
                    Subtotal = o.Subtotal,
                    VAT = o.Subtotal * 0.10,
                    ShippingFees = o.ShippingFees,
                    Total = o.Invoices.FirstOrDefault().TotalAmount,
                    Status = o.Status,
                    MediaItems = o.OrderMedia.Select(m => new
                    {
                        m.Media.Title,
                        m.Quantity,
                        m.OrderType
                    }),
                    RushDelivery = o.RushOrderInfos.Select(r => new
                    {
                        r.DeliveryTime,
                        r.Instruction
                    }),
                    BankCode = o.Invoices.FirstOrDefault().Transaction.BankTransactionId,
                    CardType = o.Invoices.FirstOrDefault().Transaction.CardType
                })
                .ToList<dynamic>();

            ViewBag.Keyword = keyword;
            return View(orders);
        }

        [HttpPost]
        public IActionResult UpdateStatus([FromBody] OrderStatusUpdateModel model)
        {
            var order = db.OrderInfos.FirstOrDefault(o => o.OrderId == model.OrderId);
            if (order == null)
                return NotFound();
            order.Status = model.NewStatus;
            db.SaveChanges();
            return Ok();
        }
       

    }
}