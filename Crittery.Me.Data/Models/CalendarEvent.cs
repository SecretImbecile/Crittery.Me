// <copyright file="CalendarEvent.cs" company="Jack Kelly">
// Copyright © 2026 Jack Kelly.
//
// The crittery.me website is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the
// Free Software Foundation, either version 3 of the License, or
// (at your option)any later version.
//
// The crittery.me source code is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
// more details.
//
// You should have received a copy of the GNU General Public License along with
// The crittery.me source code. If not, see [https://www.gnu.org/licenses/].
// </copyright>

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
