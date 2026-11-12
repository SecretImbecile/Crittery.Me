namespace Crittery.Me.Editor.Areas.CMS.Pages.Photo
{
    using Crittery.Me.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using PhotoModel = Data.Models.Photo;

    public class IndexModel(CritteryMeContext context) : PageModel
    {
        public List<PhotoModel> Photos { get; set; } = [];

        /// <summary>
        /// Load the photo listing.
        /// </summary>
        public async Task OnGetAsync()
        {
            this.Photos = await context.Photos
                .OrderByDescending(photo => photo.CapturedOn)
                .ToListAsync();
        }

        /// <summary>
        /// Delete a photo.
        /// </summary>
        /// <param name="id">ID of the photo to delete.</param>
        public async Task<IActionResult> OnGetDeleteAsync(Guid id)
        {
            int count = await context.Photos
                .Where(photo => photo.Id == id)
                .ExecuteDeleteAsync();

            if (count > 0)
            {
                return this
                    .RedirectToPage("/Photo/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound();
            }
        }
    }
}
