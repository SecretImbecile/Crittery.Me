namespace Crittery.Me.Data.Models
{
    using Crittery.Me.Data.Enums;

    /// <summary>
    /// A calendar item, i.e. events I am going to.
    /// </summary>
    public sealed record CalendarEvent
    {
        /// <summary>
        /// Gets or sets the primary identifier of the record.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        public required CalendarEventType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the start date of the event.
        /// </summary>
        public required DateOnly DateStart { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event.
        /// </summary>
        public required DateOnly DateEnd { get; set; }

        /// <summary>
        /// Gets or sets the URL of the event's website.
        /// </summary>
        public string? Url { get; set; }
    }
}
