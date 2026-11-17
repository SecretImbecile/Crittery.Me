using Crittery.Me.Data.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Crittery.Me.Classes
{
    /// <summary>
    /// Extension members for the <see cref="IUrlHelper"/> class.
    /// </summary>
    public static class UrlHelperExtensions
    {
        extension(IUrlHelper urlHelper)
        {
            /// <summary>
            /// Generates an absolute URL to a photo's page.
            /// </summary>
            /// <param name="photo">The photo model.</param>
            /// <returns>The generated page URL.</returns>
            public string? PhotoPage(Photo photo)
            {
                return urlHelper.PageLink(
                    pageName: "/Photos",
                    values: new
                    {
                        stub = photo.Stub,
                    });
            }

            /// <summary>
            /// Generates an absolute URL to a photo's file source.
            /// </summary>
            /// <param name="photo">The photo model.</param>
            /// <returns>The generated image URL.</returns>
            public string? PhotoThumbnail(Photo photo)
            {
                string? localPath = photo.ImageThumbnail.Files
                    .OrderBy(file => file.MediaType.EndsWith("webp", StringComparison.OrdinalIgnoreCase))
                    .Select(file => file.Source)
                    .FirstOrDefault();

                localPath = urlHelper
                    .Content(localPath);

                string? baseUrl = urlHelper.PageLink(pageName: "/Index");

                if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(localPath))
                {
                    return null;
                }

                Uri baseUri = new(baseUrl);
                Uri imageUri = new(baseUri, localPath);

                return imageUri.AbsoluteUri;
            }
        }
    }
}
