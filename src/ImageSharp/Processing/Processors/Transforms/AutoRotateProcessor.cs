﻿// <copyright file="FlipProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Processing.Processors
{
    using System;
    using System.Threading.Tasks;

    using ImageSharp.Memory;
    using ImageSharp.PixelFormats;
    using SixLabors.Primitives;

    /// <summary>
    /// Adjusts an image so that its orientation is suitable for viewing. Adjustments are based on EXIF metadata embedded in the image.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal class AutoRotateProcessor<TPixel> : ImageProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRotateProcessor{TPixel}"/> class.
        /// </summary>
        public AutoRotateProcessor()
        {
        }

        /// <inheritdoc/>
        protected override void OnApply(ImageBase<TPixel> sourceBase, Rectangle sourceRectangle)
        {
            // can only apply to the origional image
            var source = sourceBase as Image<TPixel>;
            if (source != null)
            {
                Orientation orientation = GetExifOrientation(source);

                switch (orientation)
                {
                    case Orientation.TopRight:
                        new FlipProcessor<TPixel>(FlipType.Horizontal).Apply(source, sourceRectangle);
                        break;

                    case Orientation.BottomRight:
                        new RotateProcessor<TPixel>() { Angle = (int)RotateType.Rotate180, Expand = false }.Apply(source, sourceRectangle);
                        break;

                    case Orientation.BottomLeft:
                        new FlipProcessor<TPixel>(FlipType.Vertical).Apply(source, sourceRectangle);
                        break;

                    case Orientation.LeftTop:
                        new RotateProcessor<TPixel>() { Angle = (int)RotateType.Rotate90, Expand = false }.Apply(source, sourceRectangle);
                        new FlipProcessor<TPixel>(FlipType.Horizontal).Apply(source, sourceRectangle);
                        break;

                    case Orientation.RightTop:
                        new RotateProcessor<TPixel>() { Angle = (int)RotateType.Rotate90, Expand = false }.Apply(source, sourceRectangle);
                        break;

                    case Orientation.RightBottom:
                        new FlipProcessor<TPixel>(FlipType.Vertical).Apply(source, sourceRectangle);
                        new RotateProcessor<TPixel>() { Angle = (int)RotateType.Rotate270, Expand = false }.Apply(source, sourceRectangle);
                        break;

                    case Orientation.LeftBottom:
                        new RotateProcessor<TPixel>() { Angle = (int)RotateType.Rotate270, Expand = false }.Apply(source, sourceRectangle);
                        break;

                    case Orientation.Unknown:
                    case Orientation.TopLeft:
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Returns the current EXIF orientation
        /// </summary>
        /// <param name="source">The image to auto rotate.</param>
        /// <returns>The <see cref="Orientation"/></returns>
        private static Orientation GetExifOrientation(Image<TPixel> source)
        {
            if (source.MetaData.ExifProfile == null)
            {
                return Orientation.Unknown;
            }

            ExifValue value = source.MetaData.ExifProfile.GetValue(ExifTag.Orientation);
            if (value == null)
            {
                return Orientation.Unknown;
            }

            var orientation = (Orientation)value.Value;

            source.MetaData.ExifProfile.SetValue(ExifTag.Orientation, (ushort)Orientation.TopLeft);

            return orientation;
        }
    }
}