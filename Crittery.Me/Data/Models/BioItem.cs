namespace Crittery.Me.Data.Models
{
    /// <summary>
    /// A record that is part of the itemised bio.
    /// </summary>
    public sealed record BioItem
    {
        /// <summary>
        /// Gets or sets the primary identifier of the record.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the display order of the item.
        /// </summary>
        public required int Order { get; set; }

        /// <summary>
        /// Gets the text of the item.
        /// </summary>
        public required string Text { get; set; }
    }
}
