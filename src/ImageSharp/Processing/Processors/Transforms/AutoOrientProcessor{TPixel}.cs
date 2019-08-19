// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;

using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace SixLabors.ImageSharp.Processing.Processors.Transforms
{
    /// <summary>
    /// Adjusts an image so that its orientation is suitable for viewing. Adjustments are based on EXIF metadata embedded in the image.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal class AutoOrientProcessor<TPixel> : ImageProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoOrientProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="source">The target <see cref="Image{T}"/> for the current processor instance.</param>
        /// <param name="sourceRectangle">The target area to process for the current processor instance.</param>
        public AutoOrientProcessor(Image<TPixel> source, Rectangle sourceRectangle)
            : base(source, sourceRectangle)
        { }

        /// <inheritdoc/>
        protected override void BeforeImageApply(Image<TPixel> source, Rectangle sourceRectangle)
        {
            OrientationMode orientation = GetExifOrientation(source);
            Size size = sourceRectangle.Size;
            switch (orientation)
            {
                case OrientationMode.TopRight:
                    new FlipProcessor(FlipMode.Horizontal).Apply(source, sourceRectangle);
                    break;

                case OrientationMode.BottomRight:
                    new RotateProcessor((int)RotateMode.Rotate180, size).Apply(source, sourceRectangle);
                    break;

                case OrientationMode.BottomLeft:
                    new FlipProcessor(FlipMode.Vertical).Apply(source, sourceRectangle);
                    break;

                case OrientationMode.LeftTop:
                    new RotateProcessor((int)RotateMode.Rotate90, size).Apply(source, sourceRectangle);
                    new FlipProcessor(FlipMode.Horizontal).Apply(source, sourceRectangle);
                    break;

                case OrientationMode.RightTop:
                    new RotateProcessor((int)RotateMode.Rotate90, size).Apply(source, sourceRectangle);
                    break;

                case OrientationMode.RightBottom:
                    new FlipProcessor(FlipMode.Vertical).Apply(source, sourceRectangle);
                    new RotateProcessor((int)RotateMode.Rotate270, size).Apply(source, sourceRectangle);
                    break;

                case OrientationMode.LeftBottom:
                    new RotateProcessor((int)RotateMode.Rotate270, size).Apply(source, sourceRectangle);
                    break;

                case OrientationMode.Unknown:
                case OrientationMode.TopLeft:
                default:
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void OnFrameApply(
            ImageFrame<TPixel> sourceBase,
            Rectangle sourceRectangle,
            Configuration config)
        {
            // All processing happens at the image level within BeforeImageApply();
        }

        /// <summary>
        /// Returns the current EXIF orientation
        /// </summary>
        /// <param name="source">The image to auto rotate.</param>
        /// <returns>The <see cref="OrientationMode"/></returns>
        private static OrientationMode GetExifOrientation(Image<TPixel> source)
        {
            if (source.Metadata.ExifProfile is null)
            {
                return OrientationMode.Unknown;
            }

            ExifValue value = source.Metadata.ExifProfile.GetValue(ExifTag.Orientation);
            if (value is null)
            {
                return OrientationMode.Unknown;
            }

            OrientationMode orientation;
            if (value.DataType == ExifDataType.Short)
            {
                orientation = (OrientationMode)value.Value;
            }
            else
            {
                orientation = (OrientationMode)Convert.ToUInt16(value.Value);
                source.Metadata.ExifProfile.RemoveValue(ExifTag.Orientation);
            }

            source.Metadata.ExifProfile.SetValue(ExifTag.Orientation, (ushort)OrientationMode.TopLeft);

            return orientation;
        }
    }
}
