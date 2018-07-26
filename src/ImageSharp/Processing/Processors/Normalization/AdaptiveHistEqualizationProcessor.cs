﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Numerics;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Memory;
using SixLabors.Primitives;

namespace SixLabors.ImageSharp.Processing.Processors.Normalization
{
    /// <summary>
    /// Applies an adaptive histogram equalization to the image.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal class AdaptiveHistEqualizationProcessor<TPixel> : HistogramEqualizationProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveHistEqualizationProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="luminanceLevels">The number of different luminance levels. Typical values are 256 for 8-bit grayscale images
        /// or 65536 for 16-bit grayscale images.</param>
        public AdaptiveHistEqualizationProcessor(int luminanceLevels)
            : base(luminanceLevels)
        {
        }

        /// <inheritdoc/>
        protected override void OnFrameApply(ImageFrame<TPixel> source, Rectangle sourceRectangle, Configuration configuration)
        {
            MemoryAllocator memoryAllocator = configuration.MemoryAllocator;
            int numberOfPixels = source.Width * source.Height;
            Span<TPixel> pixels = source.GetPixelSpan();

            int gridSize = 64;
            int pixelsInGrid = gridSize * gridSize;
            int contrastLimit = 5;
            int halfGridSize = gridSize / 2;
            using (IBuffer<int> histogramBuffer = memoryAllocator.AllocateClean<int>(this.LuminanceLevels))
            using (IBuffer<int> histogramBufferCopy = memoryAllocator.AllocateClean<int>(this.LuminanceLevels))
            using (IBuffer<int> cdfBuffer = memoryAllocator.AllocateClean<int>(this.LuminanceLevels))
            using (Buffer2D<TPixel> targetPixels = configuration.MemoryAllocator.Allocate2D<TPixel>(source.Width, source.Height))
            {
                Span<int> histogram = histogramBuffer.GetSpan();
                Span<int> histogramCopy = histogramBufferCopy.GetSpan();
                Span<int> cdf = cdfBuffer.GetSpan();

                for (int x = 0; x < source.Width; x++)
                {
                    histogram.Clear();

                    // Build the histogram of grayscale values for the current grid.
                    for (int dy = 0; dy < gridSize; dy++)
                    {
                        Span<TPixel> rowSpan = this.GetPixelRow(source, x - halfGridSize, dy, gridSize);
                        this.AddPixelsTooHistogram(rowSpan, histogram, this.LuminanceLevels);
                    }

                    for (int y = 1; y < source.Height; y++)
                    {
                        // Remove top most row from the histogram, mirroring rows which exceeds the borders
                        Span<TPixel> rowSpan = this.GetPixelRow(source, x - halfGridSize, y - halfGridSize - 1, gridSize);
                        this.RemovePixelsFromHistogram(rowSpan, histogram, this.LuminanceLevels);

                        // Add new bottom row to the histogram, mirroring rows which exceeds the borders
                        rowSpan = this.GetPixelRow(source, x - halfGridSize, y + halfGridSize - 1, gridSize);
                        this.AddPixelsTooHistogram(rowSpan, histogram, this.LuminanceLevels);

                        // Clipping the histogram, but doing it on a copy to keep the original un-clipped values for the next iteration
                        histogram.CopyTo(histogramCopy);
                        this.ClipHistogram(histogramCopy, gridSize, contrastLimit);
                        cdf.Clear();
                        int cdfMin = this.CalculateCdf(cdf, histogramCopy);
                        double numberOfPixelsMinusCdfMin = (double)(pixelsInGrid - cdfMin);

                        // Map the current pixel to the new equalized value
                        int luminance = this.GetLuminance(pixels[(y * source.Width) + x], this.LuminanceLevels);
                        double luminanceEqualized = cdf[luminance] / numberOfPixelsMinusCdfMin;
                        targetPixels[x, y].PackFromVector4(new Vector4((float)luminanceEqualized));
                    }
                }

                Buffer2D<TPixel>.SwapOrCopyContent(source.PixelBuffer, targetPixels);
            }
        }

        /// <summary>
        /// Get the a pixel row at a given position with a length of the grid size. Mirrors pixels which exceeds the edges.
        /// </summary>
        /// <param name="source">The source image.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="gridSize">The grid size.</param>
        /// <returns>A pixel row of the length of the grid size.</returns>
        private Span<TPixel> GetPixelRow(ImageFrame<TPixel> source, int x, int y, int gridSize)
        {
            if (y < 0)
            {
                y = Math.Abs(y);
            }
            else if (y >= source.Height)
            {
                int diff = y - source.Height;
                y = source.Height - diff - 1;
            }

            // special cases for the left and the right border where GetPixelRowSpan can not be used
            if (x < 0)
            {
                var rowPixels = new List<TPixel>();
                for (int dx = x; dx < x + gridSize; dx++)
                {
                    rowPixels.Add(source[Math.Abs(dx), y]);
                }

                return rowPixels.ToArray();
            }
            else if (x + gridSize > source.Width)
            {
                var rowPixels = new List<TPixel>();
                for (int dx = x; dx < x + gridSize; dx++)
                {
                    if (dx >= source.Width)
                    {
                        int diff = dx - source.Width;
                        rowPixels.Add(source[dx - diff - 1, y]);
                    }
                    else
                    {
                        rowPixels.Add(source[dx, y]);
                    }
                }

                return rowPixels.ToArray();
            }

            return source.GetPixelRowSpan(y).Slice(start: x, length: gridSize);
        }

        /// <summary>
        /// AHE tends to over amplify the contrast in near-constant regions of the image, since the histogram in such regions is highly concentrated.
        /// Clipping the histogram is meant to reduce this effect, by cutting of histogram bin's which exceed a certain amount and redistribute
        /// the values over the clip limit to all other bins equally.
        /// </summary>
        /// <param name="histogram">The histogram to apply the clipping</param>
        /// <param name="gridSize">The grid size of the AHE</param>
        /// <param name="contrastLimit">The contrast limit. Defaults to 5.</param>
        /// <returns>The number of pixels redistributed to all other bins</returns>
        private int ClipHistogram(Span<int> histogram, int gridSize, int contrastLimit = 5)
        {
            int clipLimit = (contrastLimit * (gridSize * gridSize)) / this.LuminanceLevels;

            int sumOverClip = 0;
            for (int i = 0; i < histogram.Length; i++)
            {
                if (histogram[i] > clipLimit)
                {
                    sumOverClip += histogram[i] - clipLimit;
                    histogram[i] = clipLimit;
                }
            }

            int addToEachBin = (int)Math.Floor(sumOverClip / (double)this.LuminanceLevels);
            for (int i = 0; i < histogram.Length; i++)
            {
                histogram[i] += addToEachBin;
            }

            // The redistribution will push some bins over the clip limit again.
            // Re-Applying the clipping until this effect no longer occurs.
            while (addToEachBin > 1)
            {
                addToEachBin = this.ClipHistogram(histogram, gridSize, contrastLimit);
            }

            return addToEachBin;
        }

        /// <summary>
        /// Adds a row of grey values to the histogram.
        /// </summary>
        /// <param name="greyValues">The grey values to add</param>
        /// <param name="histogram">The histogram</param>
        /// <param name="luminanceLevels">The number of different luminance levels.</param>
        private void AddPixelsTooHistogram(Span<TPixel> greyValues, Span<int> histogram, int luminanceLevels)
        {
            for (int i = 0; i < greyValues.Length; i++)
            {
                int luminance = this.GetLuminance(greyValues[i], luminanceLevels);
                histogram[luminance]++;
            }
        }

        /// <summary>
        /// Removes a row of grey values from the histogram.
        /// </summary>
        /// <param name="greyValues">The grey values to remove</param>
        /// <param name="histogram">The histogram</param>
        /// <param name="luminanceLevels">The number of different luminance levels.</param>
        private void RemovePixelsFromHistogram(Span<TPixel> greyValues, Span<int> histogram, int luminanceLevels)
        {
            for (int i = 0; i < greyValues.Length; i++)
            {
                int luminance = this.GetLuminance(greyValues[i], luminanceLevels);
                histogram[luminance]--;
            }
        }
    }
}
