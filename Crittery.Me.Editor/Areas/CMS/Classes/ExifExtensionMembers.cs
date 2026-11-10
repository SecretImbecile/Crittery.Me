namespace Crittery.Me.Editor.Areas.CMS.Classes
{
    using ExifLibrary;
    using System.Globalization;

    /// <summary>
    /// Extension members for EXIF image metadata.
    /// </summary>
    public static class ExifExtensionMembers
    {
        /// <summary>
        /// The string format used by EXIF datetimes.
        /// </summary>
        public const string DateTimeFormat = "yyyy:MM:dd HH:mm:ss";

        extension(ExifPropertyCollection<ExifProperty> exifProperties)
        {
            /// <summary>
            /// Gets the <see cref="ExifTag.Copyright"/> string value.
            /// </summary>
            /// <returns>A <see langword="string"/> representing the field, or <see langword="null"/>.</returns>
            public string? GetCopyright()
            {
                return GetString(exifProperties, ExifTag.Copyright);
            }

            /// <summary>
            /// Gets the <see cref="ExifTag.DateTimeOriginal"/> value as a <see cref="DateTime"/>.
            /// </summary>
            /// <returns>A <see cref="DateTime"/> representing the exposure time, or <see langword="null"/>.</returns>
            public DateTime? GetDateTimeOriginal()
            {
                return GetDateTime(exifProperties, ExifTag.DateTimeOriginal);
            }

            /// <summary>
            /// Gets the <see cref="ExifTag.ExposureTime"/> value as a <see langword="double"/>.
            /// </summary>
            /// <returns>A <see langword="double"/> representing the exposure time, or <see langword="null"/>.</returns>
            public double? GetExposureTime()
            {
                return GetRational(exifProperties, ExifTag.ExposureTime);
            }

            /// <summary>
            /// Gets the <see cref="ExifTag.Flash"/> value as a <see langword="bool"/>.
            /// </summary>
            /// <returns>A <see langword="bool"/> representing whether the flash fired, or <see langword="null"/> if indeterminate.</returns>
            public bool? GetFlash()
            {
                bool propertyExists = exifProperties
                    .Where(property => property.Tag == ExifTag.Flash)
                    .Any();

                if (propertyExists)
                {
                    ExifEnumProperty<Flash> exifFlash = exifProperties
                        .Get<ExifEnumProperty<Flash>>(ExifTag.Flash);

                    return exifFlash.Value switch
                    {
                        Flash.FlashDidNotFire => false,
                        Flash.FlashFired => true,
                        Flash.StrobeReturnLightDetected => true,
                        Flash.StrobeReturnLightNotDetected => null,
                        Flash.CompulsoryFlashMode => true,
                        Flash.AutoMode => null,
                        Flash.NoFlashFunction => false,
                        Flash.RedEyeReductionMode => true,
                        _ => null,
                    };
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Gets the <see cref="ExifTag.FNumber"/> value as a <see langword="double"/>.
            /// </summary>
            /// <returns>A <see langword="double"/> representing the F number, or <see langword="null"/>.</returns>
            public double? GetFNumber()
            {
                return GetRational(exifProperties, ExifTag.FNumber);
            }

            /// <summary>
            /// Gets the <see cref="ExifTag.FocalLength"/> value as a <see langword="double"/>.
            /// </summary>
            /// <returns>A <see langword="double"/> representing the focal length in mm, or <see langword="null"/>.</returns>
            public double? GetFocalLength()
            {
                return GetRational(exifProperties, ExifTag.FocalLength);
            }

            /// <summary>
            /// Gets the <see cref="ExifTag.ImageDescription"/> string value.
            /// </summary>
            /// <returns>A <see langword="string"/> representing the field, or <see langword="null"/>.</returns>
            public string? GetImageDescription()
            {
                return GetString(exifProperties, ExifTag.ImageDescription);
            }

            /// <summary>
            /// Gets the <see cref="ExifTag.LensMake"/> string value.
            /// </summary>
            /// <returns>A <see langword="string"/> representing the field, or <see langword="null"/>.</returns>
            public string? GetLensMake()
            {
                return GetString(exifProperties, ExifTag.LensMake);
            }

            /// <summary>
            /// Gets the <see cref="ExifTag.LensModel"/> string value.
            /// </summary>
            /// <returns>A <see langword="string"/> representing the field, or <see langword="null"/>.</returns>
            public string? GetLensModel()
            {
                return GetString(exifProperties, ExifTag.LensModel);
            }

            /// <summary>
            /// Gets the combined <see cref="ExifTag.LensMake"/> and <see cref="ExifTag.LensModel"/>.
            /// </summary>
            /// <returns>A <see langword="string"/> representing both fields, or <see langword="null"/>.</returns>
            public string? GetLensCombined()
            {
                string? make = GetLensMake(exifProperties);
                string? model = GetLensModel(exifProperties);

                if (!string.IsNullOrWhiteSpace(make) || !string.IsNullOrWhiteSpace(model))
                {
                    return string
                        .Join(' ', make, model)
                        .Trim();
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Gets the <see cref="ExifTag.Model"/> string value.
            /// </summary>
            /// <returns>A <see langword="string"/> representing the field, or <see langword="null"/>.</returns>
            public string? GetModel()
            {
                return GetString(exifProperties, ExifTag.Model);
            }

            /// <summary>
            /// Gets the specified <see cref="ExifDateTime"/> <see cref="ExifTag"/> string value.
            /// </summary>
            /// <param name="exifTag">The EXIF tag to get.</param>
            /// <returns>A <see cref="DateTime"/> representing the field, or <see langword="null"/>.</returns>
            private DateTime? GetDateTime(ExifTag exifTag)
            {
                bool propertyExists = exifProperties
                    .Where(property => property.Tag == exifTag)
                    .Any();

                if (propertyExists)
                {
                    ExifDateTime exifDateTime = exifProperties
                        .Get<ExifDateTime>(exifTag);

                    return exifDateTime.Value;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Gets the <see cref="ExifURational"/> <see cref="ExifTag"/> value as a <see langword="double"/>.
            /// </summary>
            /// <param name="exifTag">The EXIF tag to get.</param>
            /// <returns>A <see langword="double"/> representing the field, or <see langword="null"/>.</returns>
            private double? GetRational(ExifTag exifTag)
            {
                bool propertyExists = exifProperties
                    .Where(property => property.Tag == exifTag)
                    .Any();

                if (propertyExists)
                {
                    ExifURational fNumberRational = exifProperties
                        .Get<ExifURational>(exifTag);

                    return (double)fNumberRational.Value.Numerator / (double)fNumberRational.Value.Denominator;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Gets the specified <see cref="ExifAscii"/> <see cref="ExifTag"/> string value.
            /// </summary>
            /// <param name="exifTag">The EXIF tag to get.</param>
            /// <returns>A <see langword="string"/> representing the field, or <see langword="null"/>.</returns>
            private string? GetString(ExifTag exifTag)
            {
                bool propertyExists = exifProperties
                    .Where(property => property.Tag == exifTag)
                    .Any();

                if (propertyExists)
                {
                    ExifAscii exifString = exifProperties
                        .Get<ExifAscii>(exifTag);

                    return exifString.Value;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
