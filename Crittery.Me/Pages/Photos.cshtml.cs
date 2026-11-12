
namespace Crittery.Me.Pages
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the photos page;
    /// </summary>
    public class PhotosModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets or sets the photos to display.
        /// </summary>
        public List<Photo> Photos { get; set; } = [];

        /// <summary>
        /// Load the photos page.
        /// </summary>
        public async Task OnGetAsync()
        {
            this.Photos = await context.Photos
                .OrderByDescending(photo => photo.CapturedOn)
                .ToListAsync();

            foreach(Photo photo in this.Photos)
            {
                photo.ImageFullSize.Files = photo.ImageFullSize.Files
                    .OrderBy(file => file.MediaType.EndsWith("webp", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                photo.ImageThumbnail.Files = photo.ImageThumbnail.Files
                    .OrderBy(file => file.MediaType.EndsWith("webp", StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
    }
}
