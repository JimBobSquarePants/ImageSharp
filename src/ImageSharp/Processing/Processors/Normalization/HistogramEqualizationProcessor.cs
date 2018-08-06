﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Processing.Processors.Normalization
{
    /// <summary>
    /// Defines a processor that normalizes the histogram of an image.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal abstract class HistogramEqualizationProcessor<TPixel> : ImageProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramEqualizationProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="luminanceLevels">The number of different luminance levels. Typical values are 256 for 8-bit grayscale images
        /// or 65536 for 16-bit grayscale images.</param>
        /// <param name="clipHistogram">Indicates, if histogram bins should be clipped.</param>
        /// <param name="clipLimitPercentage">Histogram clip limit in percent of the total pixels in the grid. Histogram bins which exceed this limit, will be capped at this value.</param>
        protected HistogramEqualizationProcessor(int luminanceLevels, bool clipHistogram, float clipLimitPercentage)
        {
            Guard.MustBeGreaterThan(luminanceLevels, 0, nameof(luminanceLevels));
            Guard.MustBeGreaterThan(clipLimitPercentage, 0.0f, nameof(clipLimitPercentage));

            this.LuminanceLevels = luminanceLevels;
            this.ClipHistogramEnabled = clipHistogram;
            this.ClipLimitPercentage = clipLimitPercentage;
        }

        /// <summary>
        /// Gets the number of luminance levels.
        /// </summary>
        public int LuminanceLevels { get; }

        /// <summary>
        /// Gets a value indicating whether to clip the histogram bins at a specific value.
        /// </summary>
        public bool ClipHistogramEnabled { get; }

        /// <summary>
        /// Gets the histogram clip limit in percent of the total pixels in the grid. Histogram bins which exceed this limit, will be capped at this value.
        /// </summary>
        public float ClipLimitPercentage { get; }

        /// <summary>
        /// Calculates the cumulative distribution function.
        /// </summary>
        /// <param name="cdf">The array holding the cdf.</param>
        /// <param name="histogram">The histogram of the input image.</param>
        /// <returns>The first none zero value of the cdf.</returns>
        protected int CalculateCdf(Span<int> cdf, Span<int> histogram)
        {
            // Calculate the cumulative histogram
            int histSum = 0;
            for (int i = 0; i < histogram.Length; i++)
            {
                histSum += histogram[i];
                cdf[i] = histSum;
            }

            // Get the first none zero value of the cumulative histogram
            int cdfMin = 0;
            for (int i = 0; i < histogram.Length; i++)
            {
                if (cdf[i] != 0)
                {
                    cdfMin = cdf[i];
                    break;
                }
            }

            // Creating the lookup table: subtracting cdf min, so we do not need to do that inside the for loop
            for (int i = 0; i < histogram.Length; i++)
            {
                cdf[i] = Math.Max(0, cdf[i] - cdfMin);
            }

            return cdfMin;
        }

        /// <summary>
        /// AHE tends to over amplify the contrast in near-constant regions of the image, since the histogram in such regions is highly concentrated.
        /// Clipping the histogram is meant to reduce this effect, by cutting of histogram bin's which exceed a certain amount and redistribute
        /// the values over the clip limit to all other bins equally.
        /// </summary>
        /// <param name="histogram">The histogram to apply the clipping.</param>
        /// <param name="clipLimitPercentage">Histogram clip limit in percent of the total pixels in the grid. Histogram bins which exceed this limit, will be capped at this value.</param>
        /// <param name="pixelCount">The numbers of pixels inside the grid.</param>
        protected void ClipHistogram(Span<int> histogram, float clipLimitPercentage, int pixelCount)
        {
            int clipLimit = Convert.ToInt32(pixelCount * clipLimitPercentage);
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
            if (addToEachBin > 0)
            {
                for (int i = 0; i < histogram.Length; i++)
                {
                    histogram[i] += addToEachBin;
                }
            }
        }

        /// <summary>
        /// Convert the pixel values to grayscale using ITU-R Recommendation BT.709.
        /// </summary>
        /// <param name="sourcePixel">The pixel to get the luminance from</param>
        /// <param name="luminanceLevels">The number of luminance levels (256 for 8 bit, 65536 for 16 bit grayscale images)</param>
        [System.Runtime.CompilerServices.MethodImpl(InliningOptions.ShortMethod)]
        protected int GetLuminance(TPixel sourcePixel, int luminanceLevels)
        {
            // Convert to grayscale using ITU-R Recommendation BT.709
            var vector = sourcePixel.ToVector4();
            int luminance = Convert.ToInt32(((.2126F * vector.X) + (.7152F * vector.Y) + (.0722F * vector.Y)) * (luminanceLevels - 1));

            return luminance;
        }
    }
}
