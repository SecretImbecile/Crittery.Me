namespace Crittery.Me.Data.Models
{
    using Crittery.Me.Data.Enums;
    using Crittery.Me.Data.Models.Images;

    /// <summary>
    /// A homepage link.
    /// </summary>
    public sealed record class Link : BaseOrderedContent
    {
        /// <summary>
        /// Gets or sets the link's URL.
        /// </summary>
        public required string Url { get; set; }

        /// <summary>
        /// Gets or sets the link's title attribute.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the visiblity flags for the link.
        /// </summary>
        public required LinkVisiblity Visiblity { get; set; }

        /// <summary>
        /// Gets or sets the link's icon.
        /// </summary>
        public required Image Icon { get; set; }

    }
}
