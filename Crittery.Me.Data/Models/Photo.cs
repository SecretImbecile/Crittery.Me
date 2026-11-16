// <copyright file="Photo.cs" company="Jack Kelly">
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
    using Crittery.Me.Data.Models.Images;

    /// <summary>
    /// A gallery photo.
    /// </summary>
    public sealed record class Photo
    {
        /// <summary>
        /// Gets or sets the primary identifier of the photo.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the URL stub for this photo.
        /// </summary>
        public required string Stub { get; set; }

        /// <summary>
        /// Gets or sets the photo's capture date.
        /// </summary>
        public required DateTime CapturedOn { get; set; }

        /// <summary>
        /// Gets or sets the photo's Title.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the full-size image.
        /// </summary>
        public required Image ImageFullSize { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail image.
        /// </summary>
        public required Image ImageThumbnail { get; set; }

        /// <summary>
        /// Gets or sets the photo's metadata.
        /// </summary>
        public required PhotoMetadata Metadata { get; set; }

        /// <summary>
        /// Gets or sets the URL of the photo on FurTrack.
        /// </summary>
        public string? UrlFurTrack { get; set; }
    }
}
