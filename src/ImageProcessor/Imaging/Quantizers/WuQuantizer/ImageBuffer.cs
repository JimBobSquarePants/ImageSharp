﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageBuffer.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The image buffer for storing pixel information.
//   Adapted from <see href="https://github.com/drewnoakes" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Imaging.Quantizers.WuQuantizer
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    using ImageProcessor.Common.Exceptions;

    /// <summary>
    /// The image buffer for storing pixel information.
    /// Adapted from <see href="https://github.com/drewnoakes"/>
    /// </summary>
    internal class ImageBuffer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBuffer"/> class.
        /// </summary>
        /// <param name="image">
        /// The image to store.
        /// </param>
        public ImageBuffer(Bitmap image)
        {
            this.Image = image;
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        public Bitmap Image { get; private set; }

        /// <summary>
        /// Gets the enumerable pixel array representing each row of pixels.
        /// </summary>
        /// <exception cref="QuantizationException">
        /// Thrown if the given image is not a 32 bit per pixel image.
        /// </exception>
        public IEnumerable<Pixel[]> PixelLines
        {
            get
            {
                int bitDepth = System.Drawing.Image.GetPixelFormatSize(this.Image.PixelFormat);
                if (bitDepth != 32)
                {
                    throw new QuantizationException(
                        string.Format(
                            "The image you are attempting to quantize does not contain a 32 bit ARGB palette. This image has a bit depth of {0} with {1} colors.",
                            bitDepth,
                            this.Image.Palette.Entries.Length));
                }

                int width = this.Image.Width;
                int height = this.Image.Height;
                Pixel[] pixels = new Pixel[width];

                using (FastBitmap bitmap = new FastBitmap(this.Image))
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            Color color = bitmap.GetPixel(x, y);
                            pixels[x] = new Pixel(color.A, color.R, color.G, color.B);
                        }

                        yield return pixels;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the pixel indexes.
        /// </summary>
        /// <param name="lineIndexes">
        /// The enumerable byte array representing each row of pixels.
        /// </param>
        public void UpdatePixelIndexes(IEnumerable<byte[]> lineIndexes)
        {
            int width = this.Image.Width;
            int height = this.Image.Height;
            int rowIndex = 0;

            BitmapData data = this.Image.LockBits(Rectangle.FromLTRB(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            try
            {
                IntPtr pixelBase = data.Scan0;
                int scanWidth = data.Stride;
                foreach (byte[] scanLine in lineIndexes)
                {
                    // TODO: Use unsafe code
                    Marshal.Copy(scanLine, 0, IntPtr.Add(pixelBase, scanWidth * rowIndex), width);

                    if (++rowIndex >= height)
                    {
                        break;
                    }
                }
            }
            finally
            {
                this.Image.UnlockBits(data);
            }
        }
    }
}
