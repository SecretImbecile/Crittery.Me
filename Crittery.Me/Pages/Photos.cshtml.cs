// <copyright file="Photos.cshtml.cs" company="Jack Kelly">
// Copyright © 2026 Jack Kelly.
//
// The crittery.me website is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the
// Free Software Foundation, either version 3 of the License, or
// (at your option)any later version.
//
// The crittery.me source code is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
// more details.
//
// You should have received a copy of the GNU General Public License along with
// The crittery.me source code. If not, see [https://www.gnu.org/licenses/].
// </copyright>

namespace Crittery.Me.Pages
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the photos page.
    /// </summary>
    public class PhotosModel(CritteryMeContext context) : PageModel
    {
        /// <summary>
        /// Gets the photos to display.
        /// </summary>
        public List<Photo> Photos { get; private set; } = [];

        /// <summary>
        /// Gets the currently viewed photo, if any.
        /// </summary>
        public Photo? PhotoCurrent { get; private set; }

        /// <summary>
        /// Gets the previous photo to navigate to, if any.
        /// </summary>
        public Photo? PhotoPrevious { get; private set; }

        /// <summary>
        /// Gets the next photo to navigate to, if any.
        /// </summary>
        public Photo? PhotoNext { get; private set; }

        /// <summary>
        /// Load the photos page.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the page request.
        /// Results in a page view.
        /// </returns>
        public Task<IActionResult> OnGetAsync(string? stub)
        {
            if (string.IsNullOrWhiteSpace(stub))
            {
                return this.OnGetHomeAsync();
            }
            else
            {
                return this.OnGetViewAsync(stub);
            }
        }

        /// <summary>
        /// Load the photos page with the default view.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the page request.
        /// Results in a page view.
        /// </returns>
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
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the page request.
        /// Results in a page view.
        /// </returns>
        public async Task<IActionResult> OnGetViewAsync(string stub)
        {
            this.PhotoCurrent = await context.Photos
                .Where(photo => EF.Functions.Like(photo.Stub, stub))
                .SingleOrDefaultAsync();

            if (this.PhotoCurrent != null)
            {
                this.Photos = await context.Photos
                        .OrderByDescending(photo => photo.CapturedOn)
                        .ToListAsync();

                this.PhotoPrevious = this.Photos
                        .Where(photo => photo.CapturedOn > this.PhotoCurrent.CapturedOn)
                        .OrderBy(photo => photo.CapturedOn)
                        .FirstOrDefault();

                this.PhotoNext = this.Photos
                    .Where(photo => photo.CapturedOn < this.PhotoCurrent.CapturedOn)
                    .OrderByDescending(photo => photo.CapturedOn)
                    .FirstOrDefault();

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
            else
            {
                return this
                    .NotFound();
            }
        }
    }
}
