// <copyright file="BaseContent.cs" company="Jack Kelly">
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
    using System;

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
        /// Gets or sets the text of the item.
        /// </summary>
        public required string Text { get; set; }
    }
}
