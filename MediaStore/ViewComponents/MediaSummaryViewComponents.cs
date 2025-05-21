
using MediaStore.Data;
using MediaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaStore.ViewComponents
{
    public class MediaSummaryViewComponent : ViewComponent
    {
        private readonly AimsContext db;

        public MediaSummaryViewComponent(AimsContext context)
        {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bookCount = await db.Books.CountAsync();
            var dvdCount = await db.Dvds.CountAsync();
            var cdCount = await db.CdAndLps.CountAsync(c => c.IsCd);
            var lpCount = await db.CdAndLps.CountAsync(c => !c.IsCd);
            var mediaCount = bookCount + dvdCount + cdCount + lpCount;

            var mediaCounts = new List<MediaSummaryItem> {
                new MediaSummaryItem {TypeName = "Media", Count = mediaCount},
                new MediaSummaryItem {TypeName = "Book", Count = bookCount},
                new MediaSummaryItem {TypeName = "DVD", Count = dvdCount},
                new MediaSummaryItem {TypeName = "CD", Count = cdCount},
                new MediaSummaryItem {TypeName = "LP", Count = lpCount}
            };

            return View(mediaCounts);
        }



    }
}
