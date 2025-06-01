using MediaStore.Data;
using MediaStore.Services.Email; // Đảm bảo using đúng namespace của EmailService của bạn
// using MediaStore.ViewModels; // Bỏ using này nếu không tạo ViewModel riêng cho Order
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System; // Cần cho Exception, Math.Ceiling
using System.Collections.Generic; // Cần cho List<string>
using System.Linq;
using System.Threading.Tasks;

namespace MediaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderManagementController : Controller
    {
        private readonly AimsContext _context;
        private readonly EmailService _emailService; // <<--- SỬA Ở ĐÂY: Đổi từ IEmailService sang EmailService

        // Sửa constructor để nhận EmailService
        public OrderManagementController(AimsContext context, EmailService emailService) 
        {
            _context = context;
            _emailService = emailService; // Gán EmailService đã inject
        }

        // GET: Admin/OrderManagement
        public async Task<IActionResult> Index(string customerName, string orderStatus, int page = 1)
        {
            int pageSize = 10;
            var query = _context.OrderInfos
                                    .Include(o => o.Delivery)
                                    .OrderByDescending(o => o.OrderId)
                                    .AsQueryable();

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(o => o.Delivery != null && o.Delivery.Name.Contains(customerName));
            }

            if (!string.IsNullOrEmpty(orderStatus) && orderStatus != "All")
            {
                query = query.Where(o => o.Status == orderStatus);
            }

            var totalItems = await query.CountAsync();
            var orders = await query
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CustomerNameFilter = customerName;
            ViewBag.OrderStatusFilter = orderStatus;
            ViewBag.TotalOrders = totalItems;
            ViewBag.PossibleStatuses = new List<string> { "All", "Pending Confirmation", "Processing", "Shipped", "Completed", "Cancelled", "Rejected" };

            return View(orders);
        }

        // GET: Admin/OrderManagement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderInfo = await _context.OrderInfos
                .Include(o => o.Delivery) 
                .Include(o => o.OrderMedia) 
                    .ThenInclude(om => om.Media) 
                .Include(o => o.Invoices) 
                    .ThenInclude(i => i.Transaction) 
                .Include(o => o.RushOrderInfos) 
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (orderInfo == null)
            {
                return NotFound();
            }
            
            ViewBag.PossibleStatuses = new List<string> { "Pending Confirmation", "Processing", "Shipped", "Completed", "Cancelled", "Rejected" };

            return View(orderInfo);
        }

        // POST: Admin/OrderManagement/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId, string newStatus)
        {
            if (string.IsNullOrEmpty(newStatus))
            {
                TempData["ErrorMessage"] = "Trạng thái mới không được để trống.";
                return RedirectToAction(nameof(Details), new { id = orderId });
            }

            var orderInfo = await _context.OrderInfos.Include(o => o.Delivery).FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (orderInfo == null)
            {
                return NotFound();
            }

            string oldStatus = orderInfo.Status; // Lưu lại trạng thái cũ để so sánh nếu cần
            orderInfo.Status = newStatus;
            try
            {
                _context.Update(orderInfo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Trạng thái của đơn hàng #{orderId} đã được cập nhật thành '{newStatus}'.";

                // Tùy chọn: Gửi email thông báo cho khách hàng về việc cập nhật trạng thái
                // Nếu bạn muốn gửi email ở đây, bạn cần một phương thức trong EmailService.cs
                // ví dụ: SendOrderStatusUpdateEmailAsync(string toEmail, string customerName, string orderCode, string newStatus)
                // Và đảm bảo EmailService có phương thức đó.
                if (orderInfo.Delivery != null && !string.IsNullOrEmpty(orderInfo.Delivery.Email))
                {
                    // Ví dụ nếu bạn tạo phương thức SendOrderStatusUpdateEmail trong EmailService:
                    // await _emailService.SendOrderStatusUpdateEmail(
                    //      orderInfo.Delivery.Email, 
                    //      orderInfo.OrderId.ToString(), // orderCode
                    //      orderInfo.Delivery.Name, 
                    //      newStatus
                    // );
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.OrderInfos.Any(e => e.OrderId == orderId)) { return NotFound(); } else { throw; }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi cập nhật trạng thái đơn hàng: " + ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id = orderId });
        }
    }
}