// <copyright file="PhotoMetadata.cs" company="Jack Kelly">
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
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Metadata for a photo, mirroring standard EXIF values.
    /// </summary>
    [Owned]
    public sealed record PhotoMetadata
    {
        /// <summary>
        /// Gets or sets the photo's Copyright string.
        /// </summary>
        /// <example>
        /// CC-BY-NC-ND.
        /// </example>
        public string? Copyright { get; set; }

        /// <summary>
        /// Gets or sets the URL that explains the <see cref="Copyright"/> string,
        /// e.g. a link to the license file.
        /// </summary>
        public string? CopyrightUrl { get; set; }

        /// <summary>
        /// Gets or sets the photo's original capture time.
        /// </summary>
        public DateTime? DateTimeOriginal { get; set; }

        /// <summary>
        /// Gets or sets the photo's description text.
        /// </summary>
        public string? ImageDescription { get; set; }

        /// <summary>
        /// Gets or sets the name of the camera the photo was taken with.
        /// </summary>
        public string? CameraModel { get; set; }

        /// <summary>
        /// Gets or sets the name of the lens the photo was taken with.
        /// </summary>
        public string? LensModel { get; set; }

        /// <summary>
        /// Gets or sets the photo's focal length.
        /// </summary>
        public double? FocalLength { get; set; }

        /// <summary>
        /// Gets or sets the photo's aperture value.
        /// </summary>
        public double? ApertureValue { get; set; }

        /// <summary>
        /// Gets or sets the photo's exposure time.
        /// </summary>
        public double? ExposureTime { get; set; }

        /// <summary>
        /// Gets or sets the photo's exposure time.
        /// </summary>
        public int? IsoSpeedRating { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether flash was used for this photo.
        /// </summary>
        public bool? Flash { get; set; }
    }
}
