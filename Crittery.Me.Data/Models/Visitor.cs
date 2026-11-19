// <copyright file="Visitor.cs" company="Jack Kelly">
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
    /// <summary>
    /// Records a visit to the site.
    /// </summary>
    public sealed record Visitor
    {
        /// <summary>
        /// Gets or sets the primary identifier for the record.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the page that was visited.
        /// </summary>
        public required string Page { get; set; }

        /// <summary>
        /// Gets or sets the referrer header of the visitor.
        /// </summary>
        /// <remarks>
        /// Use <see cref="string.Empty"/> if no referrer header is present.
        /// </remarks>
        public required string Referrer { get; set; }

        /// <summary>
        /// Gets or sets the number of visitors of the matching page and referrer.
        /// </summary>
        public int Count { get; set; } = 0;

        /// <summary>
        /// Gets or sets the date this record was last modified,
        /// i.e. the date of the last visit.
        /// </summary>
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}
