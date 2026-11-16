// <copyright file="New.cshtml.cs" company="Jack Kelly">
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

namespace Crittery.Me.Editor.Areas.CMS.Pages.Photo
{
    using System.Net.Mime;
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Crittery.Me.Editor.Areas.CMS.Classes;
    using ExifLibrary;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// Page model for the new photo form.
    /// </summary>
    public class NewModel(
        CritteryMeContext context,
        IWebHostEnvironment hostEnvironment) : PageModel
    {
        /// <summary>
        /// Gets a list of URLS for known license names.
        /// </summary>
        public static Dictionary<string, string> KnownLicenseUrls => new()
        {
            { "CC-BY", "https://creativecommons.org/licenses/by/4.0" },
            { "CC-BY-NC", "https://creativecommons.org/licenses/by-nc/4.0" },
            { "CC-BY-NC-ND", "https://creativecommons.org/licenses/by-nc-nd/4.0" },
            { "CC-BY-NC-SA", "https://creativecommons.org/licenses/by-nc-sa/4.0" },
            { "CC-BY-ND", "https://creativecommons.org/licenses/by-nd/4.0" },
            { "CC-BY-SA", "https://creativecommons.org/licenses/by-sa/4.0" },
        };

        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public Photo FormData { get; set; } = null!;

        /// <summary>
        /// Load the new photo form.
        /// </summary>
        public async void OnGetAsync()
        {
            this.FormData = new()
            {
                Stub = string.Empty,
                CapturedOn = default,
                Title = string.Empty,
                ImageFullSize = new()
                {
                    Files =
                    [
                        new()
                        {
                            Source = string.Empty,
                            MediaType = MediaTypeNames.Image.Jpeg,
                        },
                        new()
                        {
                            Source = string.Empty,
                            MediaType = MediaTypeNames.Image.Webp,
                        },
                    ],
                    Alt = string.Empty,
                },
                ImageThumbnail = new()
                {
                    Files =
                    [
                        new()
                        {
                            Source = string.Empty,
                            MediaType = MediaTypeNames.Image.Jpeg,
                        },
                        new()
                        {
                            Source = string.Empty,
                            MediaType = MediaTypeNames.Image.Webp,
                        },
                    ],
                    Alt = string.Empty,
                },
                Metadata = new(),
            };
        }

        /// <summary>
        /// Fetch the metadata for an iamge file.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnGetMetadataAsync(string src)
        {
            try
            {
                // Get metadata from the image file
                PhotoMetadata metadata = await this
                    .GetPhotoMetadataAsync(src);

                // Autofill the URL to the license if the copyright field names a CC license
                if (!string.IsNullOrWhiteSpace(metadata.Copyright))
                {
                    metadata.CopyrightUrl = KnownLicenseUrls
                        .Where(license => license.Key.Equals(metadata.Copyright, StringComparison.OrdinalIgnoreCase))
                        .Select(license => license.Value)
                        .FirstOrDefault();
                }

                return this
                    .Partial("Partials/_Photo_Metadata", metadata);
            }
            catch (Exception)
            {
                return this
                    .StatusCode(StatusCodes.Status204NoContent);
            }
        }

        /// <summary>
        /// Submit the new photo form.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the HTTP request.
        /// Results in a page redirect.
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            // Validate the image paths
            string pathPhotoJpeg = this.GetFullPath(this.FormData.ImageFullSize.Files[0].Source);
            string pathPhotoWebp = this.GetFullPath(this.FormData.ImageFullSize.Files[1].Source);
            string pathThumbnailJpeg = this.GetFullPath(this.FormData.ImageThumbnail.Files[0].Source);
            string pathThumbnailWebp = this.GetFullPath(this.FormData.ImageThumbnail.Files[1].Source);

            bool isCustomValid = true;
            if (!System.IO.File.Exists(pathPhotoJpeg))
            {
                this.ModelState.AddModelError("FormData.ImageFullSize", "Invalid file path");
                isCustomValid = false;
            }

            if (!System.IO.File.Exists(pathPhotoWebp))
            {
                this.ModelState.AddModelError("FormData.ImageFullSize", "Invalid file path");
                isCustomValid = false;
            }

            if (!System.IO.File.Exists(pathThumbnailJpeg))
            {
                this.ModelState.AddModelError("FormData.ImageThumbnail", "Invalid file path");
                isCustomValid = false;
            }

            if (!System.IO.File.Exists(pathThumbnailWebp))
            {
                this.ModelState.AddModelError("FormData.ImageThumbnail", "Invalid file path");
                isCustomValid = false;
            }

            if (!isCustomValid)
            {
                return this.Page();
            }

            Photo newItem = this.FormData with
            {
                Id = Guid.NewGuid(),
                ImageFullSize = this.FormData.ImageFullSize with
                {
                    Alt = this.FormData.Metadata.ImageDescription!,
                },
                ImageThumbnail = this.FormData.ImageThumbnail with
                {
                    Alt = this.FormData.Title!,
                },
                Metadata = this.FormData.Metadata with
                {
                    DateTimeOriginal = this.FormData.CapturedOn,
                },
            };

            _ = context.Photos
                .Add(newItem);

            _ = await context
                .SaveChangesAsync();

            return this.RedirectToPage("/Photo/Index", new { Area = "CMS" });
        }

        /// <summary>
        /// Gets the full file system path to the specified image source.
        /// </summary>
        /// <param name="src">The local URL to the image.</param>
        /// <returns>The full system path of the specified image file.</returns>
        private string GetFullPath(string src)
        {
            string localPath = src
                .TrimStart('/')
                .Replace('/', Path.DirectorySeparatorChar);

            return Path
                .Combine(hostEnvironment.WebRootPath, localPath);
        }

        /// <summary>
        /// Get the metadata for the specified image.
        /// </summary>
        /// <param name="src">The local URL path of the image to fetch.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the duration of the file read.
        /// Results in a <see cref="PhotoMetadata"/> record.
        /// </returns>
        private async Task<PhotoMetadata> GetPhotoMetadataAsync(string src)
        {
            string filePath = this
                .GetFullPath(src);

            ImageFile imageFile = await ImageFile
                    .FromFileAsync(filePath);

            ExifPropertyCollection<ExifProperty> metadata = imageFile.Properties;

            // Remove seconds so this is read correctly by the browser
            DateTime? dateTimeOriginal = metadata.GetDateTimeOriginal();
            dateTimeOriginal = dateTimeOriginal?.AddSeconds(-1 * dateTimeOriginal.Value.Second);

            return new PhotoMetadata()
            {
                ApertureValue = metadata.GetFNumber(),
                CameraModel = metadata.GetModel(),
                Copyright = metadata.GetCopyright(),
                DateTimeOriginal = dateTimeOriginal,
                ExposureTime = metadata.GetExposureTime(),
                Flash = metadata.GetFlash(),
                ImageDescription = metadata.GetImageDescription(),
                IsoSpeedRating = metadata.GetIsoSpeedRating(),
                LensModel = metadata.GetLensCombined(),
                FocalLength = metadata.GetFocalLength(),
            };
        }
    }
}
