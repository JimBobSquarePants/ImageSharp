// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Formats.Tiff
{
    /// <summary>
    /// Implements the 'RGB' photometric interpretation (optimised for 8-bit full color images).
    /// </summary>
    internal class Rgb888TiffColor<TPixel> : TiffBaseColorDecoder<TPixel>
        where TPixel : unmanaged, IPixel<TPixel>
    {
        public Rgb888TiffColor()
        {
        }

        /// <inheritdoc/>
        public override void Decode(ReadOnlySpan<byte> data, Buffer2D<TPixel> pixels, int left, int top, int width, int height)
        {
            var color = default(TPixel);

            int offset = 0;

            for (int y = top; y < top + height; y++)
            {
                Span<TPixel> pixelRow = pixels.GetRowSpan(y);

                for (int x = left; x < left + width; x++)
                {
                    byte r = data[offset++];
                    byte g = data[offset++];
                    byte b = data[offset++];

                    color.FromRgba32(new Rgba32(r, g, b, 255));
                    pixelRow[x] = color;
                }
            }
        }
    }
}
