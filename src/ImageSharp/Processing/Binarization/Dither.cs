﻿// <copyright file="Dither.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp
{
    using System;

    using ImageSharp.Dithering;
    using ImageSharp.PixelFormats;
    using ImageSharp.Processing.Processors;
    using SixLabors.Primitives;

    /// <summary>
    /// Extension methods for the <see cref="Image{TPixel}"/> type.
    /// </summary>
    public static partial class ImageExtensions
    {
        /// <summary>
        /// Dithers the image reducing it to two colors using ordered dithering.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="dither">The ordered ditherer.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Dither<TPixel>(this IImageProcessingContext<TPixel> source, IOrderedDither dither)
            where TPixel : struct, IPixel<TPixel>
        {
            source.ApplyProcessor(new OrderedDitherProcessor<TPixel>(dither, 0));
            return source;
        }

        /// <summary>
        /// Dithers the image reducing it to two colors using ordered dithering.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="dither">The ordered ditherer.</param>
        /// <param name="index">The component index to test the threshold against. Must range from 0 to 3.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Dither<TPixel>(this IImageProcessingContext<TPixel> source, IOrderedDither dither, int index)
            where TPixel : struct, IPixel<TPixel>
        {
            source.ApplyProcessor(new OrderedDitherProcessor<TPixel>(dither, index));
            return source;
        }

        /// <summary>
        /// Dithers the image reducing it to two colors using ordered dithering.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="dither">The ordered ditherer.</param>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> structure that specifies the portion of the image object to alter.
        /// </param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Dither<TPixel>(this IImageProcessingContext<TPixel> source, IOrderedDither dither, Rectangle rectangle)
            where TPixel : struct, IPixel<TPixel>
        {
            source.ApplyProcessor(new OrderedDitherProcessor<TPixel>(dither, 0), rectangle);
            return source;
        }

        /// <summary>
        /// Dithers the image reducing it to two colors using ordered dithering.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="dither">The ordered ditherer.</param>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> structure that specifies the portion of the image object to alter.
        /// </param>
        /// <param name="index">The component index to test the threshold against. Must range from 0 to 3.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Dither<TPixel>(this IImageProcessingContext<TPixel> source, IOrderedDither dither, Rectangle rectangle, int index)
            where TPixel : struct, IPixel<TPixel>
        {
            source.ApplyProcessor(new OrderedDitherProcessor<TPixel>(dither, index), rectangle);
            return source;
        }

        /// <summary>
        /// Dithers the image reducing it to two colors using error diffusion.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="diffuser">The diffusion algorithm to apply.</param>
        /// <param name="threshold">The threshold to apply binarization of the image. Must be between 0 and 1.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Dither<TPixel>(this IImageProcessingContext<TPixel> source, IErrorDiffuser diffuser, float threshold)
            where TPixel : struct, IPixel<TPixel>
        {
            source.ApplyProcessor(new ErrorDiffusionDitherProcessor<TPixel>(diffuser, threshold));
            return source;
        }

        /// <summary>
        /// Dithers the image reducing it to two colors using error diffusion.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="diffuser">The diffusion algorithm to apply.</param>
        /// <param name="threshold">The threshold to apply binarization of the image. Must be between 0 and 1.</param>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> structure that specifies the portion of the image object to alter.
        /// </param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Dither<TPixel>(this IImageProcessingContext<TPixel> source, IErrorDiffuser diffuser, float threshold, Rectangle rectangle)
            where TPixel : struct, IPixel<TPixel>
        {
            source.ApplyProcessor(new ErrorDiffusionDitherProcessor<TPixel>(diffuser, threshold), rectangle);
            return source;
        }
    }
}
