using MediaStore.Data;
using MediaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MediaStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly AimsContext db;
        private const int PageSize = 6;
        public ProductController(AimsContext context)
        {
            db = context;
        }

        private static string GetTypeName(Media media)
        {
            return media switch
            {
                Book => "Book",
                Dvd => "DVD",
                CdAndLp m => m.IsCd ? "CD" : "LP",
                _ => "Media"
            };
        }
        // public IActionResult Index(string? TypeName)
        // {
        //     var productList = db.Media.ToList(); // Join cac bang neu su dung TPT
        //     if (!string.IsNullOrEmpty(TypeName) && TypeName != "Media")
        //     {
        //         productList = productList.Where(p=>
        //             (TypeName == "Book" && p is Book) ||
        //             (TypeName == "DVD" && p is Dvd) ||
        //             (TypeName == "CD" && p is CdAndLp p1 && p1.IsCd) ||
        //             (TypeName == "LP" && p is CdAndLp p2 && !p2.IsCd)
        //         ).ToList();

        //     }
        //     var result = productList.Select(p => new ProductViewModel
        //     {
        //         MediaId = p.MediaId,
        //         Title = p.Title,
        //         Price = p.Price,
        //         ImageUrl = p.ImageUrl,
        //         Description = p.Description,
        //         Type = GetTypeName(p)
        //     });
        //     return View(result);
        // }

        public IActionResult Index(string? query, string? TypeName, int page = 1)
        {
            var products = db.Media.ToList(); // ToList() trước để có thể dùng 'is Book'

            // Lọc theo loại trong bộ nhớ
            if (!string.IsNullOrEmpty(TypeName) && TypeName != "Media")
            {
                products = products.Where(p =>
                    (TypeName == "Book" && p is Book) ||
                    (TypeName == "DVD" && p is Dvd) ||
                    (TypeName == "CD" && p is CdAndLp cd && cd.IsCd) ||
                    (TypeName == "LP" && p is CdAndLp lp && !lp.IsCd)
                ).ToList();
            }

            // Lọc theo tiêu đề
            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => p.Title.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Tổng số sản phẩm và phân trang
            var totalItems = products.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            var pagedProducts = products
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var result = pagedProducts.Select(p => new ProductViewModel
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
            ViewBag.TotalPages = totalPages;
            ViewBag.Query = query;
            ViewBag.TypeName = TypeName;
            ViewBag.TotalProduct = products.Count();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_productList", result);
            }

            return View(result);
        }

        // public IActionResult Search(string query)
        // {
        //     // if (string.IsNullOrWhiteSpace(query))
        //     // {
        //     //     return PartialView("_SearchResult", new List<ProductViewModel>());
        //     // }
        //     var result = db.Media
        //         .Where(p => p.Title.Contains(query))
        //         .Select(p => new ProductViewModel
        //         {
        //             MediaId = p.MediaId,
        //             Title = p.Title,
        //             Price = p.Price,
        //             ImageUrl = p.ImageUrl,
        //             Description = p.Description,
        //             Type = GetTypeName(p)
        //         })
        //         .ToList();
        //     return PartialView("_SearchResult", result);
        // }

        // public IActionResult Search(string? query)
        // {
        //     // Normalize query: trim and check for null/whitespace
        //     query = query?.Trim();
        //     if (string.IsNullOrWhiteSpace(query))
        //     {
        //         return PartialView("_SearchResult", new List<ProductViewModel>());
        //     }

        //     try
        //     {
        //         // Perform case-insensitive search and limit results
        //         var result = db.Media
        //             .Where(p => p.Title != null && p.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
        //             .Take(100) // Limit results to prevent performance issues
        //             .Select(p => new ProductViewModel
        //             {
        //                 MediaId = p.MediaId,
        //                 Title = p.Title ?? string.Empty, // Handle null Title
        //                 Price = p.Price, // Ensure Price is non-null or handle accordingly
        //                 ImageUrl = p.ImageUrl ?? string.Empty, // Handle null ImageUrl
        //                 Description = p.Description ?? string.Empty, // Handle null Description
        //                 Type = GetTypeName(p) // Ensure GetTypeName handles null cases
        //             })
        //             .ToList();

        //         return PartialView("_SearchResult", result);
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the error (use a logging framework like Serilog or ILogger)
        //         Console.WriteLine($"Search error: {ex.Message}");
        //         return StatusCode(500, "An error occurred while searching. Please try again.");
        //     }
        // }
        public IActionResult Detail(int id, string type)
        {
            object data = null;
            ProductViewModel result;
            switch (type)
            {
                case "Book":
                    data = db.Books.FirstOrDefault(b => b.MediaId == id);
                    result = new BookViewModel((Book)data);
                    return View("ProductDetail/bookDetail", result);
                case "DVD":
                    data = db.Dvds.FirstOrDefault(b => b.MediaId == id);
                    result = new DvdViewModel((Dvd)data);
                    return View("ProductDetail/dvdDetail", result);
                case "CD":
                    data = db.CdAndLps.FirstOrDefault(b => b.MediaId == id && b.IsCd);
                    result = new CdViewModel((CdAndLp)data);
                    return View("ProductDetail/cdDetail", result);
                case "LP":
                    data = db.CdAndLps.FirstOrDefault(b => b.MediaId == id && !b.IsCd);
                    result = new LpViewModel((CdAndLp)data);
                    return View("ProductDetail/lpDetail", result);
                default:
                    TempData["Message"] = "Unknown product type";
                    return Redirect("/404");
            }
            if (data == null)
            {
                TempData["Message"] = "Can't find this product information";
                return Redirect("/404");
            }
        }
    }
}
