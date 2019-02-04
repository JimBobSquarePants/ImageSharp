﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System.Numerics;

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace SixLabors.ImageSharp.Processing.Processors.Transforms
{
    /// <summary>
    /// Provides methods that allow the skewing of images.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    public class SkewProcessor<TPixel> : AffineTransformProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkewProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="degreesX">The angle in degrees to perform the skew along the x-axis.</param>
        /// <param name="degreesY">The angle in degrees to perform the skew along the y-axis.</param>
        /// <param name="sourceSize">The source image size</param>
        public SkewProcessor(float degreesX, float degreesY, Size sourceSize)
            : this(degreesX, degreesY, KnownResamplers.Bicubic, sourceSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkewProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="degreesX">The angle in degrees to perform the skew along the x-axis.</param>
        /// <param name="degreesY">The angle in degrees to perform the skew along the y-axis.</param>
        /// <param name="sampler">The sampler to perform the skew operation.</param>
        /// <param name="sourceSize">The source image size</param>
        public SkewProcessor(float degreesX, float degreesY, IResampler sampler, Size sourceSize)
            : this(
                 TransformUtils.CreateSkewMatrixDegrees(degreesX, degreesY, sourceSize),
                 sampler,
                 sourceSize)
        {
            this.DegreesX = degreesX;
            this.DegreesY = degreesY;
        }

        // Helper constructor:
        private SkewProcessor(Matrix3x2 skewMatrix, IResampler sampler, Size sourceSize)
            : base(skewMatrix, sampler, TransformUtils.GetTransformedSize(sourceSize, skewMatrix))
        {
        }

        /// <summary>
        /// Gets the angle of rotation along the x-axis in degrees.
        /// </summary>
        public float DegreesX { get; }

        /// <summary>
        /// Gets the angle of rotation along the y-axis in degrees.
        /// </summary>
        public float DegreesY { get; }
    }
}