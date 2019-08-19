// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Numerics;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.ParallelUtils;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Primitives;
using SixLabors.Primitives;

namespace SixLabors.ImageSharp.Processing.Processors.Filters
{
    /// <summary>
    /// Provides methods that accept a <see cref="ColorMatrix"/> matrix to apply free-form filters to images.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal class FilterProcessor<TPixel> : ImageProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        private readonly FilterProcessor definition;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="definition">The <see cref="FilterProcessor"/>.</param>
        /// <param name="image">The target <see cref="Image{T}"/> for the current processor instance.</param>
        /// <param name="rectangle">The target area to process for the current processor instance.</param>
        public FilterProcessor(FilterProcessor definition, Image<TPixel> image, Rectangle rectangle)
            : base(image, rectangle)
        {
            this.definition = definition;
        }

        /// <inheritdoc/>
        protected override void OnFrameApply(ImageFrame<TPixel> source, Rectangle sourceRectangle, Configuration configuration)
        {
            var interest = Rectangle.Intersect(sourceRectangle, source.Bounds());
            int startX = interest.X;

            ColorMatrix matrix = this.definition.Matrix;

            ParallelHelper.IterateRowsWithTempBuffer<Vector4>(
                interest,
                configuration,
                (rows, vectorBuffer) =>
                    {
                        for (int y = rows.Min; y < rows.Max; y++)
                        {
                            Span<Vector4> vectorSpan = vectorBuffer.Span;
                            int length = vectorSpan.Length;
                            Span<TPixel> rowSpan = source.GetPixelRowSpan(y).Slice(startX, length);
                            PixelOperations<TPixel>.Instance.ToVector4(configuration, rowSpan, vectorSpan);

                            Vector4Utils.Transform(vectorSpan, ref matrix);

                            PixelOperations<TPixel>.Instance.FromVector4Destructive(configuration, vectorSpan, rowSpan);
                        }
                    });
        }
    }
}
