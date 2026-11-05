namespace Crittery.Me.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Base record for text content records.
    /// </summary>
    public abstract record BaseContent
    {
        /// <summary>
        /// Gets or sets the primary identifier of the record.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the text of the item.
        /// </summary>
        public required string Text { get; set; }
    }
}
