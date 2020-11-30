// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Numerics;
using SixLabors.ImageSharp.Formats.Tiff.Utils;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Formats.Tiff
{
    /// <summary>
    /// Implements the 'RGB' photometric interpretation (for all bit depths).
    /// </summary>
    internal class RgbTiffColor<TPixel> : TiffBaseColorDecoder<TPixel>
        where TPixel : unmanaged, IPixel<TPixel>
    {
        private readonly float rFactor;

        private readonly float gFactor;

        private readonly float bFactor;

        private readonly ushort bitsPerSampleR;

        private readonly ushort bitsPerSampleG;

        private readonly ushort bitsPerSampleB;

        public RgbTiffColor(ushort[] bitsPerSample)
        {
            this.bitsPerSampleR = bitsPerSample[0];
            this.bitsPerSampleG = bitsPerSample[1];
            this.bitsPerSampleB = bitsPerSample[2];

            this.rFactor = (float)(1 << this.bitsPerSampleR) - 1.0f;
            this.gFactor = (float)(1 << this.bitsPerSampleG) - 1.0f;
            this.bFactor = (float)(1 << this.bitsPerSampleB) - 1.0f;
        }

        /// <inheritdoc/>
        public override void Decode(ReadOnlySpan<byte> data, Buffer2D<TPixel> pixels, int left, int top, int width, int height)
        {
            var color = default(TPixel);

            var bitReader = new BitReader(data);

            for (int y = top; y < top + height; y++)
            {
                for (int x = left; x < left + width; x++)
                {
                    float r = ((float)bitReader.ReadBits(this.bitsPerSampleR)) / this.rFactor;
                    float g = ((float)bitReader.ReadBits(this.bitsPerSampleG)) / this.gFactor;
                    float b = ((float)bitReader.ReadBits(this.bitsPerSampleB)) / this.bFactor;

                    color.FromVector4(new Vector4(r, g, b, 1.0f));
                    pixels[x, y] = color;
                }

                bitReader.NextRow();
            }
        }
    }
}
