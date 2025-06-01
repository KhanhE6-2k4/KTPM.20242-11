using MediaStore.Data;
using MediaStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductManagementController : Controller
    {
        private readonly AimsContext _context;

        public ProductManagementController(AimsContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductManagement
        public async Task<IActionResult> Index(string searchString, string typeNameFilter, int page = 1)
        {
            int pageSize = 10;
            var mediaQuery = _context.Media.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                mediaQuery = mediaQuery.Where(m => m.Title.Contains(searchString));
            }
            
            var allMediaWithIncludes = await mediaQuery
                                        .Include(m => m.Book)
                                        .Include(m => m.Dvd)
                                        .Include(m => m.CdAndLp)
                                        .OrderByDescending(m => m.MediaId)
                                        .ToListAsync();

            IEnumerable<Media> filteredMedia = allMediaWithIncludes;

            if (!string.IsNullOrEmpty(typeNameFilter) && typeNameFilter != "All")
            {
                switch (typeNameFilter)
                {
                    case "Book":
                        filteredMedia = allMediaWithIncludes.Where(m => m.Book != null);
                        break;
                    case "DVD":
                        filteredMedia = allMediaWithIncludes.Where(m => m.Dvd != null);
                        break;
                    case "CD":
                        filteredMedia = allMediaWithIncludes.Where(m => m.CdAndLp != null && m.CdAndLp.IsCd);
                        break;
                    case "LP":
                        filteredMedia = allMediaWithIncludes.Where(m => m.CdAndLp != null && !m.CdAndLp.IsCd);
                        break;
                }
            }

            var totalItems = filteredMedia.Count();
            var pagedMedia = filteredMedia
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList(); 
            
            var productViewModels = pagedMedia.Select(p => new ProductViewModel
            {
                MediaId = p.MediaId,
                Title = p.Title,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Description = p.Description,
                Type = GetTypeName(p), 
                TotalQuantity = p.TotalQuantity
            }).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.SearchString = searchString;
            ViewBag.TypeNameFilter = typeNameFilter;
            ViewBag.TotalProduct = totalItems;
            ViewBag.MediaTypes = new List<string> { "All", "Book", "DVD", "CD", "LP" };

            return View(productViewModels);
        }
        
        private static string GetTypeName(Media media)
        {
            if (media.Book != null) return "Book";
            if (media.Dvd != null) return "DVD";
            if (media.CdAndLp != null) return media.CdAndLp.IsCd ? "CD" : "LP";
            return "Media"; 
        }

        // GET: Admin/ProductManagement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var media = await _context.Media
                .Include(m => m.Book)
                .Include(m => m.Dvd)
                .Include(m => m.CdAndLp)
                .FirstOrDefaultAsync(m => m.MediaId == id);
            
            if (media == null) return NotFound();
            
            var productDetail = new AdminCreateProductViewModel 
            {
                MediaType = GetTypeName(media),
                Title = media.Title,
                Price = media.Price,
                TotalQuantity = media.TotalQuantity,
                Weight = media.Weight,
                RushOrderSupported = media.RushOrderSupported ?? false,
                ImageUrl = media.ImageUrl,
                Barcode = media.Barcode,
                Description = media.Description,
                ProductDimension = media.ProductDimension,
                ImportDate = media.ImportDate
            };

            if (media.Book != null) {
                productDetail.Authors = media.Book.Authors; productDetail.CoverType = media.Book.CoverType; productDetail.Publisher = media.Book.Publisher; productDetail.PublicationDate = media.Book.PublicationDate; productDetail.Pages = media.Book.Pages; productDetail.BookLanguage = media.Book.Language; productDetail.BookGenre = media.Book.Genre;
            } else if (media.Dvd != null) {
                productDetail.DvdType = media.Dvd.DvdType; productDetail.Director = media.Dvd.Director; productDetail.Runtime = media.Dvd.Runtime; productDetail.Studio = media.Dvd.Studio; productDetail.DvdLanguage = media.Dvd.Language; productDetail.Subtitles = media.Dvd.Subtitles; productDetail.ReleasedDate = media.Dvd.ReleasedDate; productDetail.DvdGenre = media.Dvd.Genre;
            } else if (media.CdAndLp != null) {
                productDetail.Artists = media.CdAndLp.Artists; productDetail.RecordLabel = media.CdAndLp.RecordLabel; productDetail.TrackList = media.CdAndLp.TrackList; productDetail.CdLpGenre = media.CdAndLp.Genre; productDetail.CdLpReleaseDate = media.CdAndLp.ReleaseDate;
            }
            ViewBag.ProductId = id; 
            return View(productDetail); 
        }

        // GET: Admin/ProductManagement/Create
        public IActionResult Create()
        {
            var model = new AdminCreateProductViewModel();
            ViewBag.MediaTypes = new List<string> { "Book", "DVD", "CD", "LP" };
            return View(model);
        }

        // POST: Admin/ProductManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCreateProductViewModel viewModel)
        {
            ViewBag.MediaTypes = new List<string> { "Book", "DVD", "CD", "LP" };

            if (ModelState.IsValid)
            {
                var media = new Media
                {
                    // QUAN TRỌNG: KHÔNG gán MediaId ở đây.
                    Title = viewModel.Title,
                    Price = viewModel.Price,
                    TotalQuantity = viewModel.TotalQuantity,
                    Weight = viewModel.Weight,
                    RushOrderSupported = viewModel.RushOrderSupported,
                    ImageUrl = viewModel.ImageUrl,
                    Barcode = viewModel.Barcode,
                    Description = viewModel.Description,
                    ProductDimension = viewModel.ProductDimension,
                    ImportDate = viewModel.ImportDate
                };
                _context.Media.Add(media);
                
                using var dbTransaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.SaveChangesAsync(); 

                    switch (viewModel.MediaType)
                    {
                        case "Book":
                            if (string.IsNullOrEmpty(viewModel.Authors) || string.IsNullOrEmpty(viewModel.CoverType) || string.IsNullOrEmpty(viewModel.Publisher) || viewModel.PublicationDate == null)
                            { ModelState.AddModelError("", "Thông tin bắt buộc cho Sách còn thiếu."); await dbTransaction.RollbackAsync(); return View(viewModel); }
                            var book = new Book { MediaId = media.MediaId, Authors = viewModel.Authors!, CoverType = viewModel.CoverType!, Publisher = viewModel.Publisher!, PublicationDate = viewModel.PublicationDate.Value, Pages = viewModel.Pages, Language = viewModel.BookLanguage, Genre = viewModel.BookGenre };
                            _context.Books.Add(book);
                            break;
                        case "DVD":
                             if (string.IsNullOrEmpty(viewModel.DvdType) || string.IsNullOrEmpty(viewModel.Director) || viewModel.Runtime == null || string.IsNullOrEmpty(viewModel.Studio) || string.IsNullOrEmpty(viewModel.DvdLanguage) || string.IsNullOrEmpty(viewModel.Subtitles))
                            { ModelState.AddModelError("", "Thông tin bắt buộc cho DVD còn thiếu."); await dbTransaction.RollbackAsync(); return View(viewModel); }
                            var dvd = new Dvd { MediaId = media.MediaId, DvdType = viewModel.DvdType!, Director = viewModel.Director!, Runtime = viewModel.Runtime.Value, Studio = viewModel.Studio!, Language = viewModel.DvdLanguage!, Subtitles = viewModel.Subtitles!, ReleasedDate = viewModel.ReleasedDate, Genre = viewModel.DvdGenre };
                            _context.Dvds.Add(dvd);
                            break;
                        case "CD":
                        case "LP":
                            if (string.IsNullOrEmpty(viewModel.Artists) || string.IsNullOrEmpty(viewModel.RecordLabel) || string.IsNullOrEmpty(viewModel.TrackList) || string.IsNullOrEmpty(viewModel.CdLpGenre))
                            { ModelState.AddModelError("", "Thông tin bắt buộc cho CD/LP còn thiếu."); await dbTransaction.RollbackAsync(); return View(viewModel); }
                            var cdLp = new CdAndLp { MediaId = media.MediaId, IsCd = viewModel.MediaType == "CD", Artists = viewModel.Artists!, RecordLabel = viewModel.RecordLabel!, TrackList = viewModel.TrackList!, Genre = viewModel.CdLpGenre!, ReleaseDate = viewModel.CdLpReleaseDate };
                            _context.CdAndLps.Add(cdLp);
                            break;
                        default:
                            ModelState.AddModelError("MediaType", "Loại media không hợp lệ.");
                            await dbTransaction.RollbackAsync(); 
                            return View(viewModel);
                    }
                    await _context.SaveChangesAsync(); 
                    
                    await dbTransaction.CommitAsync(); 
                    TempData["SuccessMessage"] = $"Sản phẩm '{media.Title}' ({viewModel.MediaType}) đã được tạo thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await dbTransaction.RollbackAsync(); 
                    ModelState.AddModelError("", "Lỗi khi lưu sản phẩm: " + (ex.InnerException?.Message ?? ex.Message));
                    return View(viewModel);
                }
            }
            return View(viewModel);
        }

        // GET: Admin/ProductManagement/Edit/5
        // Action này nhận id để hiển thị form Edit
        [HttpGet] // Phân biệt rõ ràng đây là action cho GET request
        public async Task<IActionResult> Edit(int? id) // Tham số là int? id
        {
            ViewBag.MediaTypes = new List<string> { "Book", "DVD", "CD", "LP" };
            if (id == null) return NotFound();

            var media = await _context.Media
                                .Include(m => m.Book)
                                .Include(m => m.Dvd)
                                .Include(m => m.CdAndLp)
                                .FirstOrDefaultAsync(m => m.MediaId == id.Value);
            
            if (media == null) return NotFound();

            var viewModel = new AdminCreateProductViewModel
            {
                Title = media.Title, Price = media.Price, TotalQuantity = media.TotalQuantity, Weight = media.Weight,
                RushOrderSupported = media.RushOrderSupported ?? false, ImageUrl = media.ImageUrl, Barcode = media.Barcode,
                Description = media.Description, ProductDimension = media.ProductDimension, ImportDate = media.ImportDate
            };
            ViewBag.ProductId = id.Value; // Truyền ProductId cho View để dùng trong form action

            if (media.Book != null) {
                viewModel.MediaType = "Book"; viewModel.Authors = media.Book.Authors; viewModel.CoverType = media.Book.CoverType; viewModel.Publisher = media.Book.Publisher; viewModel.PublicationDate = media.Book.PublicationDate; viewModel.Pages = media.Book.Pages; viewModel.BookLanguage = media.Book.Language; viewModel.BookGenre = media.Book.Genre;
            } else if (media.Dvd != null) {
                viewModel.MediaType = "DVD"; viewModel.DvdType = media.Dvd.DvdType; viewModel.Director = media.Dvd.Director; viewModel.Runtime = media.Dvd.Runtime; viewModel.Studio = media.Dvd.Studio; viewModel.DvdLanguage = media.Dvd.Language; viewModel.Subtitles = media.Dvd.Subtitles; viewModel.ReleasedDate = media.Dvd.ReleasedDate; viewModel.DvdGenre = media.Dvd.Genre;
            } else if (media.CdAndLp != null) {
                viewModel.MediaType = media.CdAndLp.IsCd ? "CD" : "LP"; viewModel.Artists = media.CdAndLp.Artists; viewModel.RecordLabel = media.CdAndLp.RecordLabel; viewModel.TrackList = media.CdAndLp.TrackList; viewModel.CdLpGenre = media.CdAndLp.Genre; viewModel.CdLpReleaseDate = media.CdAndLp.ReleaseDate;
            } else {
                 viewModel.MediaType = ""; 
            }
            
            return View(viewModel); // Trả về View Edit.cshtml với viewModel đã có dữ liệu
        }

        // POST: Admin/ProductManagement/Edit/5
        // Action này nhận id từ route và viewModel từ form body để xử lý việc lưu
        [HttpPost] // Phân biệt rõ ràng đây là action cho POST request
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminCreateProductViewModel viewModel) // Tham số là int id và AdminCreateProductViewModel viewModel
        {
            ViewBag.MediaTypes = new List<string> { "Book", "DVD", "CD", "LP" };
            ViewBag.ProductId = id; 

            if (!await _context.Media.AnyAsync(m => m.MediaId == id)) 
            {
                return NotFound(); 
            }

            if (ModelState.IsValid)
            {
                var mediaToUpdate = await _context.Media
                                        .Include(m => m.Book)
                                        .Include(m => m.Dvd)
                                        .Include(m => m.CdAndLp)
                                        .FirstOrDefaultAsync(m => m.MediaId == id);
                if (mediaToUpdate == null) return NotFound();

                mediaToUpdate.Title = viewModel.Title; mediaToUpdate.Price = viewModel.Price; mediaToUpdate.TotalQuantity = viewModel.TotalQuantity;
                mediaToUpdate.Weight = viewModel.Weight; mediaToUpdate.RushOrderSupported = viewModel.RushOrderSupported; mediaToUpdate.ImageUrl = viewModel.ImageUrl;
                mediaToUpdate.Barcode = viewModel.Barcode; mediaToUpdate.Description = viewModel.Description; mediaToUpdate.ProductDimension = viewModel.ProductDimension;
                mediaToUpdate.ImportDate = viewModel.ImportDate;
                
                // Xóa loại con cũ nếu MediaType thay đổi
                if (mediaToUpdate.Book != null && viewModel.MediaType != "Book") { _context.Books.Remove(mediaToUpdate.Book); mediaToUpdate.Book = null; }
                if (mediaToUpdate.Dvd != null && viewModel.MediaType != "DVD") { _context.Dvds.Remove(mediaToUpdate.Dvd); mediaToUpdate.Dvd = null; }
                if (mediaToUpdate.CdAndLp != null && (viewModel.MediaType != "CD" && viewModel.MediaType != "LP" ||
                    (viewModel.MediaType == "CD" && !mediaToUpdate.CdAndLp.IsCd) || 
                    (viewModel.MediaType == "LP" && mediaToUpdate.CdAndLp.IsCd)))
                {
                    _context.CdAndLps.Remove(mediaToUpdate.CdAndLp);
                    mediaToUpdate.CdAndLp = null;
                }
                
                try
                {
                    switch (viewModel.MediaType)
                    {
                        case "Book":
                            if (string.IsNullOrEmpty(viewModel.Authors) /*...*/) { /*lỗi*/ return View(viewModel); }
                            if (mediaToUpdate.Book == null) mediaToUpdate.Book = new Book { MediaId = id };
                            mediaToUpdate.Book.Authors = viewModel.Authors!; mediaToUpdate.Book.CoverType = viewModel.CoverType!; mediaToUpdate.Book.Publisher = viewModel.Publisher!; mediaToUpdate.Book.PublicationDate = viewModel.PublicationDate.Value; mediaToUpdate.Book.Pages = viewModel.Pages; mediaToUpdate.Book.Language = viewModel.BookLanguage; mediaToUpdate.Book.Genre = viewModel.BookGenre;
                            break;
                        case "DVD":
                            if (string.IsNullOrEmpty(viewModel.DvdType) /*...*/) { /*lỗi*/ return View(viewModel); }
                            if (mediaToUpdate.Dvd == null) mediaToUpdate.Dvd = new Dvd { MediaId = id };
                            mediaToUpdate.Dvd.DvdType = viewModel.DvdType!; mediaToUpdate.Dvd.Director = viewModel.Director!; mediaToUpdate.Dvd.Runtime = viewModel.Runtime.Value; mediaToUpdate.Dvd.Studio = viewModel.Studio!; mediaToUpdate.Dvd.Language = viewModel.DvdLanguage!; mediaToUpdate.Dvd.Subtitles = viewModel.Subtitles!; mediaToUpdate.Dvd.ReleasedDate = viewModel.ReleasedDate; mediaToUpdate.Dvd.Genre = viewModel.DvdGenre;
                            break;
                        case "CD":
                        case "LP":
                             if (string.IsNullOrEmpty(viewModel.Artists) /*...*/) { /*lỗi*/ return View(viewModel); }
                            if (mediaToUpdate.CdAndLp == null) mediaToUpdate.CdAndLp = new CdAndLp { MediaId = id };
                            mediaToUpdate.CdAndLp.IsCd = viewModel.MediaType == "CD";
                            mediaToUpdate.CdAndLp.Artists = viewModel.Artists!; mediaToUpdate.CdAndLp.RecordLabel = viewModel.RecordLabel!; mediaToUpdate.CdAndLp.TrackList = viewModel.TrackList!; mediaToUpdate.CdAndLp.Genre = viewModel.CdLpGenre!; mediaToUpdate.CdAndLp.ReleaseDate = viewModel.CdLpReleaseDate;
                            break;
                        default:
                            ModelState.AddModelError("MediaType", "Loại media không hợp lệ.");
                            return View(viewModel);
                    }
                    
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Sản phẩm '{mediaToUpdate.Title}' đã được cập nhật thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Media.Any(e => e.MediaId == id)) { return NotFound(); } else { throw; }
                }
                catch (Exception ex)
                {
                     ModelState.AddModelError("", "Lỗi khi cập nhật chi tiết sản phẩm: " + (ex.InnerException?.Message ?? ex.Message));
                     return View(viewModel);
                }
            }
            return View(viewModel);
        }

        // GET: Admin/ProductManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var media = await _context.Media.FirstOrDefaultAsync(m => m.MediaId == id);
            if (media == null) return NotFound();
            return View(media);
        }

        // POST: Admin/ProductManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var media = await _context.Media.FindAsync(id);
            if (media != null)
            {
                _context.Media.Remove(media); 
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sản phẩm đã được xóa thành công.";
            } else {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm để xóa.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}