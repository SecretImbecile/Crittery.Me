namespace Crittery.Me.Editor.Areas.CMS.Pages.Photo
{
    using Crittery.Me.Data.Models;
    using Crittery.Me.Editor.Areas.CMS.Classes;
    using ExifLibrary;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Net.Mime;

    /// <summary>
    /// Page model for the new photo form.
    /// </summary>
    /// <param name="hostEnvironment">The current host environment.</param>
    public class NewModel(IWebHostEnvironment hostEnvironment) : PageModel
    {
        /// <summary>
        /// Gets a list of URLS for known license names
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
                        }
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
                        }
                    ],
                    Alt = string.Empty,
                },
                Metadata = new(),
            };
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
                    metadata.CopyrightUrl = KnownLicenseUrls
                        .Where(license => license.Key.Equals(metadata.Copyright, StringComparison.OrdinalIgnoreCase))
                        .Select(license => license.Value)
                        .FirstOrDefault();
                }

                return new JsonResult(metadata);
            }
            catch (Exception)
            {
                return this
                    .StatusCode(StatusCodes.Status204NoContent);
            }
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
            src = src.TrimStart('/');
            src = src.Replace('/', Path.DirectorySeparatorChar);

            string filePath = Path
                .Combine(hostEnvironment.WebRootPath, src);

            ImageFile imageFile = await ImageFile
                    .FromFileAsync(filePath);

            ExifPropertyCollection<ExifProperty> metadata = imageFile.Properties;

            return new PhotoMetadata()
            {
                ApertureValue = metadata.GetFNumber(),
                CameraModel = metadata.GetModel(),
                Copyright = metadata.GetCopyright(),
                DateTimeOriginal = metadata.GetDateTimeOriginal(),
                ExposureTime = metadata.GetExposureTime(),
                Flash = metadata.GetFlash(),
                ImageDescription = metadata.GetImageDescription(),
                LensModel = metadata.GetLensCombined(),
                FocalLength = metadata.GetFocalLength(),
            };
        }
    }
}
