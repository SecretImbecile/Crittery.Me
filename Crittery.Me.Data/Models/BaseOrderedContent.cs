namespace Crittery.Me.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Base record for ordered text content records.
    /// </summary>
    public abstract record BaseOrderedContent : BaseContent
    {
        /// <summary>
        /// Gets or sets the display order of the interest.
        /// </summary>
        public required int Order { get; set; }
    }
}
