namespace Crittery.Me.Data.Models.Images
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// A record that represents the metadata for an img/picture tag.
    /// </summary>
    [Owned]
    public record Image
    {
        /// <summary>
        /// Gets or sets the primary identifier for the image.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the source files for the image.
        /// </summary>
        public required List<ImageFile> Files { get; set; }

        /// <summary>
        /// Get or sets the alt text for the image.
        /// </summary>
        public required string Alt { get; set; }
    }
}
