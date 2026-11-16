// <copyright file="Link.cs" company="Jack Kelly">
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
    using Crittery.Me.Data.Models.Images;

    /// <summary>
    /// A homepage link.
    /// </summary>
    public sealed record class Link : BaseOrderedContent
    {
        /// <summary>
        /// Gets or sets the link's URL.
        /// </summary>
        public required string Url { get; set; }

        /// <summary>
        /// Gets or sets the link's title attribute.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the visiblity flags for the link.
        /// </summary>
        public required LinkVisiblity Visiblity { get; set; }

        /// <summary>
        /// Gets or sets the link's icon.
        /// </summary>
        public required Image Icon { get; set; }
    }
}
