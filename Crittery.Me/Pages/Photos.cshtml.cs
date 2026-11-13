namespace Crittery.Me.Pages
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
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
        /// Gets or sets the currently viewed photo, if any.
        /// </summary>
        public Photo? CurrentPhoto { get; set; }

        /// <summary>
        /// Load the photos page.
        /// </summary>
        public Task<IActionResult> OnGetAsync(string? stub)
        {
            if (string.IsNullOrWhiteSpace(stub))
            {
                return OnGetHomeAsync();
            }
            else
            {
                return OnGetViewAsync(stub, false);
            }
        }

        /// <summary>
        /// Load the photos page with the default view.
        /// </summary>
        public async Task<IActionResult> OnGetHomeAsync()
        {
            this.Photos = await context.Photos
                .OrderByDescending(photo => photo.CapturedOn)
                .ToListAsync();

            foreach (Photo photo in this.Photos)
            {
                photo.ImageFullSize.Files = photo.ImageFullSize.Files
                    .OrderBy(file => file.MediaType.EndsWith("webp", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                photo.ImageThumbnail.Files = photo.ImageThumbnail.Files
                    .OrderBy(file => file.MediaType.EndsWith("webp", StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return this
                .Page();
        }

        /// <summary>
        /// Load the photos page with a specific image opened.
        /// </summary>
        /// <param name="stub">The URL stub of the image to view.</param>
        /// <param name="partial">If <see langword="true"/>, returns a partial view.</param>
        public async Task<IActionResult> OnGetViewAsync(string stub, bool partial)
        {
            this.CurrentPhoto = await context.Photos
                .Where(photo => EF.Functions.Like(photo.Stub, stub))
                .SingleOrDefaultAsync();

            if (this.CurrentPhoto != null)
            {
                if (partial)
                {
                    return this
                        .Partial("Partials/_Photo", this.CurrentPhoto);
                }
                else
                {
                    this.Photos = await context.Photos
                        .OrderByDescending(photo => photo.CapturedOn)
                        .ToListAsync();

                    foreach (Photo photo in this.Photos)
                    {
                        photo.ImageFullSize.Files = photo.ImageFullSize.Files
                            .OrderBy(file => file.MediaType.EndsWith("webp", StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        photo.ImageThumbnail.Files = photo.ImageThumbnail.Files
                            .OrderBy(file => file.MediaType.EndsWith("webp", StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    }

                    return this
                        .Page();
                }
            }
            else
            {
                return this
                    .NotFound();
            }
        }
    }
}
