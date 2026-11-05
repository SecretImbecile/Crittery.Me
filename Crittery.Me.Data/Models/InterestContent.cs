namespace Crittery.Me.Data.Models
{
    /// <summary>
    /// A record that describes a personal interest.
    /// </summary>
    public sealed record InterestContent : BaseOrderedContent
    {
        /// <summary>
        /// Gets or sets an emoji that depicts the interest.
        /// </summary>
        public string? Emoji { get; set; }
    }
}
