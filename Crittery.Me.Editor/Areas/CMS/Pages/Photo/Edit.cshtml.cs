namespace Crittery.Me.Editor.Areas.CMS.Pages.Photo
{
    using Crittery.Me.Data;
    using Crittery.Me.Data.Models;
    using Crittery.Me.Editor.Areas.CMS.Classes;
    using ExifLibrary;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the edit photo form.
    /// </summary>
    /// <param name="hostEnvironment">The current host environment.</param>
    public class EditModel(
        CritteryMeContext context,
        IWebHostEnvironment hostEnvironment) : PageModel
    {
        /// <summary>
        /// Gets or sets the submitted form data.
        /// </summary>
        [BindProperty]
        public Photo FormData { get; set; } = null!;

        /// <summary>
        /// Load the edit photo form.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Photo? dbItem = await context.Photos
                .AsNoTracking()
                .SingleOrDefaultAsync(photo => photo.Id == id);

            if (dbItem != null)
            {
                // Ensure files are in the same order as the form
                dbItem.ImageFullSize.Files = dbItem.ImageFullSize.Files
                    .OrderBy(file => file.MediaType.EndsWith("webp", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                dbItem.ImageThumbnail.Files = dbItem.ImageThumbnail.Files
                    .OrderBy(file => file.MediaType.EndsWith("webp", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                this.FormData = dbItem;

                return this.Page();
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Fetch the metadata for an iamge file.
        /// </summary>
        /// <returns></returns>
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
                    metadata.CopyrightUrl = NewModel.KnownLicenseUrls
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
        /// Submit the edit photo form.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(Guid id)
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

            Photo? dbItem = await context.Photos
                .SingleOrDefaultAsync(photo => photo.Id == id);

            if (dbItem != null)
            {
                dbItem.Stub = this.FormData.Stub;
                dbItem.CapturedOn = this.FormData.CapturedOn;
                dbItem.Title = this.FormData.Title;
                dbItem.UrlFurTrack = this.FormData.UrlFurTrack;
                dbItem.ImageFullSize.Alt = this.FormData.Metadata.ImageDescription!;
                dbItem.ImageFullSize.Files = this.FormData.ImageFullSize.Files;
                dbItem.ImageThumbnail.Alt = this.FormData.Title;
                dbItem.ImageThumbnail.Files = this.FormData.ImageThumbnail.Files;
                dbItem.Metadata = this.FormData.Metadata;
                dbItem.Metadata.DateTimeOriginal = this.FormData.CapturedOn;

                _ = await context
                .SaveChangesAsync();

                return this
                    .RedirectToPage("/Photo/Index", new { Area = "CMS" });
            }
            else
            {
                return this
                    .NotFound("");
            }
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
