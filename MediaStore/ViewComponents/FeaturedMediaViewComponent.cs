using System.ComponentModel;
using MediaStore.Data;
using MediaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaStore.ViewComponents
{
    public class FeaturedMediaViewComponent : ViewComponent
    {
        private readonly AimsContext db;
        public FeaturedMediaViewComponent(AimsContext context)
        {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topPriceProduct = await db.Media
            .OrderByDescending(item => item.Price)
            .Take(3)
            .ToListAsync();
            var result = topPriceProduct.Select(item => new ProductViewModel
            {
                MediaId = item.MediaId,
                Title = item.Title,
                ImageUrl = item.ImageUrl,
                Price = item.Price,
                Description = item.Description,
                TotalQuantity = item.TotalQuantity
            });
            return View(result);
        } 
    }
}