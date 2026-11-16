// <copyright file="ImageFile.cs" company="Jack Kelly">
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

namespace Crittery.Me.Data.Models.Images
{
    using System;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The path to an image file of a specified media type.
    /// </summary>
    [Owned]
    public record ImageFile
    {
        /// <summary>
        /// Gets or sets the primary identifier for the image file.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the image this file belongs to.
        /// </summary>
        public Guid ImageId { get; set; }

        /// <summary>
        /// Gets or sets the path or URI to the image file.
        /// </summary>
        public required string Source { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the specified image file.
        /// </summary>
        public required string MediaType { get; set; }

        // Navigations

        /// <summary>
        /// Gets or sets the image this file belongs to.
        /// </summary>
        public Image Image { get; set; } = null!;
    }
}
