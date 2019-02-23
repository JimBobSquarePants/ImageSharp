﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.ParallelUtils;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace SixLabors.ImageSharp.Processing.Processors.Filters
{
    /// <summary>
    /// Reduces the luminance of the input image, proportially to the luminance of each pixel.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal class ReduceLuminanceProcessor<TPixel> : ImageProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SixLabors.ImageSharp.Processing.Processors.Filters.ReduceLuminanceProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="amount">The strength of the luminance reduction filter.</param>
        public ReduceLuminanceProcessor(float amount) => this.Amount = amount;

        /// <summary>
        /// Gets the strength factor for the luminance reduction processor.
        /// </summary>
        public float Amount { get; }

        /// <inheritdoc/>
        protected override void OnFrameApply(ImageFrame<TPixel> source, Rectangle sourceRectangle, Configuration configuration)
        {
            var interest = Rectangle.Intersect(sourceRectangle, source.Bounds());
            int startX = interest.X;
            float amount = this.Amount;

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

                            ref Vector4 baseRef = ref MemoryMarshal.GetReference(vectorSpan);

                            for (int i = 0; i < vectorSpan.Length; i++)
                            {
                                ref Vector4 v = ref Unsafe.Add(ref baseRef, i);
                                Vector4Utils.Premultiply(ref v);
                                float
                                    w = v.W,
                                    luminosity = 1 - (((v.X * 0.2125f) + (v.Y * 0.7154f) + (v.Z * 0.0721f)) * amount);
                                v *= luminosity;
                                v.W = w;
                                Vector4Utils.UnPremultiply(ref v);
                            }

                            PixelOperations<TPixel>.Instance.FromVector4(configuration, vectorSpan, rowSpan);
                        }
                    });
        }
    }
}
