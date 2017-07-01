﻿// <copyright file="GaussianSharpen.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp
{
    using System;

    using ImageSharp.PixelFormats;

    using ImageSharp.Processing;
    using Processing.Processors;
    using SixLabors.Primitives;

    /// <summary>
    /// Extension methods for the <see cref="Image{TPixel}"/> type.
    /// </summary>
    public static partial class ImageExtensions
    {
        /// <summary>
        /// Applies a Gaussian sharpening filter to the image.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="sigma">The 'sigma' value representing the weight of the blur.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageOperations<TPixel> GaussianSharpen<TPixel>(this IImageOperations<TPixel> source, float sigma = 3f)
            where TPixel : struct, IPixel<TPixel>
        => source.ApplyProcessor(new GaussianSharpenProcessor<TPixel>(sigma));

        /// <summary>
        /// Applies a Gaussian sharpening filter to the image.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="sigma">The 'sigma' value representing the weight of the blur.</param>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> structure that specifies the portion of the image object to alter.
        /// </param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageOperations<TPixel> GaussianSharpen<TPixel>(this IImageOperations<TPixel> source, float sigma, Rectangle rectangle)
            where TPixel : struct, IPixel<TPixel>
        => source.ApplyProcessor(new GaussianSharpenProcessor<TPixel>(sigma), rectangle);
    }
}
