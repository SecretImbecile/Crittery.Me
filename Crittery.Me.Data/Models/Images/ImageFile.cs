namespace Crittery.Me.Data.Models.Images
{
    using Microsoft.EntityFrameworkCore;
    using System;

    /// <summary>
    /// The path to an image file of a specified media type.
    /// </summary>
    [Owned]
    public record ImageFile
    {
        /// <summary>
        /// Gets or sets the primary identifier for the image file.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the identifier of the image this file belongs to.
        /// </summary>
        public Guid ImageId { get; set; }

        /// <summary>
        /// The path or URI to the image file.
        /// </summary>
        public required string Source { get; set; }

        /// <summary>
        /// The MIME type of the specified image file.
        /// </summary>
        public required string MediaType { get; set; }

        // Navigations

        /// <summary>
        /// Gets the image this file belongs to.
        /// </summary>
        public Image Image { get; set; } = null!;
    }
}
