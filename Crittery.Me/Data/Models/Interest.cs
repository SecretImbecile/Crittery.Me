namespace Crittery.Me.Data.Models
{
    /// <summary>
    /// A record that describes a personal interest.
    /// </summary>
    public sealed record Interest
    {
        /// <summary>
        /// Gets or sets the primary identifier of the record.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the display order of the interest.
        /// </summary>
        public required int Order { get; set; }

        /// <summary>
        /// Gets or sets the name of the interest.
        /// </summary>
        public required string Text { get; set; }

        /// <summary>
        /// Gets or sets an emoji that depicts the interest.
        /// </summary>
        public string? Emoji { get; set; }
    }
}
