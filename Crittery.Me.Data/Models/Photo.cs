namespace Crittery.Me.Data.Models
{
    using Crittery.Me.Data.Models.Images;

    /// <summary>
    /// A gallery photo.
    /// </summary>
    public sealed record class Photo
    {
        /// <summary>
        /// Gets or sets the primary identifier of the photo.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the photo's alt text.
        /// </summary>
        public required string AltText { get; set; }

        /// <summary>
        /// Gets or sets the full-size image.
        /// </summary>
        public required Image ImageFullSize { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail image.
        /// </summary>
        public required Image ImageThumbnail { get; set; }

        /// <summary>
        /// Gets or sets the URL of the photo on FurTrack.
        /// </summary>
        public string? UrlFurTrack { get; set; }
    }
}
