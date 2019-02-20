﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Drawing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace SixLabors.ImageSharp.Processing
{
    /// <summary>
    /// Adds extensions that allow the filling of rectangles to the <see cref="Image{TPixel}"/> type.
    /// </summary>
    public static class FillRectangleExtensions
    {
        /// <summary>
        /// Flood fills the image in the shape of the provided rectangle with the specified brush.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="options">The options.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="shape">The shape.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, GraphicsOptions options, IBrush<TPixel> brush, RectangleF shape)
            where TPixel : struct, IPixel<TPixel>
            => source.Fill(options, brush, new RectangularPolygon(shape.X, shape.Y, shape.Width, shape.Height));

        /// <summary>
        /// Flood fills the image in the shape of the provided rectangle with the specified brush.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="shape">The shape.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, IBrush<TPixel> brush, RectangleF shape)
            where TPixel : struct, IPixel<TPixel>
            => source.Fill(brush, new RectangularPolygon(shape.X, shape.Y, shape.Width, shape.Height));

        /// <summary>
        /// Flood fills the image in the shape of the provided rectangle with the specified color.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="options">The options.</param>
        /// <param name="color">The color.</param>
        /// <param name="shape">The shape.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, GraphicsOptions options, TPixel color, RectangleF shape)
            where TPixel : struct, IPixel<TPixel>
            => source.Fill(options, new SolidBrush<TPixel>(color), shape);

        /// <summary>
        /// Flood fills the image in the shape of the provided rectangle with the specified color.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color.</param>
        /// <param name="shape">The shape.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, TPixel color, RectangleF shape)
            where TPixel : struct, IPixel<TPixel>
            => source.Fill(new SolidBrush<TPixel>(color), shape);

        /// <summary>
        /// Fills the image in the shape of the provided rectangle with the specified color.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color.</param>
        /// <param name="rectangle">The rectangle shape.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, TPixel color, Rectangle rectangle)
            where TPixel : struct, IPixel<TPixel>
            => source.Fill(GraphicsOptions.Default, new SolidBrush<TPixel>(color), rectangle);

        /// <summary>
        /// Fills the image in the shape of the provided rectangle with the specified brush.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="brush">The details how to fill the region of interest.</param>
        /// <param name="rectangle">The rectangle shape.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, IBrush<TPixel> brush, Rectangle rectangle)
            where TPixel : struct, IPixel<TPixel>
            => source.Fill(GraphicsOptions.Default, brush, rectangle);

        /// <summary>
        /// Fills the image in the shape of the provided rectangle with the specified color.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="options">The graphics options.</param>
        /// <param name="color">The color.</param>
        /// <param name="rectangle">The rectangle shape.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, GraphicsOptions options, TPixel color, Rectangle rectangle)
            where TPixel : struct, IPixel<TPixel>
            => source.Fill(options, new SolidBrush<TPixel>(color), rectangle);

        /// <summary>
        /// Fills the image in the shape of the provided rectangle with the specified brush.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="options">The graphics options.</param>
        /// <param name="brush">The details how to fill the region of interest.</param>
        /// <param name="rectangle">The rectangle shape.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> Fill<TPixel>(this IImageProcessingContext<TPixel> source, GraphicsOptions options, IBrush<TPixel> brush, Rectangle rectangle)
            where TPixel : struct, IPixel<TPixel>
            => source.ApplyProcessor(new FillProcessor<TPixel>(brush, options), rectangle);
    }
}