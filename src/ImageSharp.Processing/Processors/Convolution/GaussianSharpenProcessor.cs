﻿// <copyright file="GaussianSharpenProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Processing.Processors
{
    using System;

    /// <summary>
    /// Applies a Gaussian sharpening sampler to the image.
    /// </summary>
    /// <typeparam name="TColor">The pixel format.</typeparam>
    public class GaussianSharpenProcessor<TColor> : ImageProcessor<TColor>
        where TColor : struct, IPixel<TColor>
    {
        /// <summary>
        /// The maximum size of the kernel in either direction.
        /// </summary>
        private readonly int kernelSize;

        /// <summary>
        /// The spread of the blur.
        /// </summary>
        private readonly float sigma;

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianSharpenProcessor{TColor}"/> class.
        /// </summary>
        /// <param name="sigma">
        /// The 'sigma' value representing the weight of the sharpening.
        /// </param>
        public GaussianSharpenProcessor(float sigma = 3f)
        {
            this.kernelSize = ((int)Math.Ceiling(sigma) * 2) + 1;
            this.sigma = sigma;
            this.KernelX = this.CreateGaussianKernel(true);
            this.KernelY = this.CreateGaussianKernel(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianSharpenProcessor{TColor}"/> class.
        /// </summary>
        /// <param name="radius">
        /// The 'radius' value representing the size of the area to sample.
        /// </param>
        public GaussianSharpenProcessor(int radius)
        {
            this.kernelSize = (radius * 2) + 1;
            this.sigma = radius;
            this.KernelX = this.CreateGaussianKernel(true);
            this.KernelY = this.CreateGaussianKernel(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianSharpenProcessor{TColor}"/> class.
        /// </summary>
        /// <param name="sigma">
        /// The 'sigma' value representing the weight of the sharpen.
        /// </param>
        /// <param name="radius">
        /// The 'radius' value representing the size of the area to sample.
        /// This should be at least twice the sigma value.
        /// </param>
        public GaussianSharpenProcessor(float sigma, int radius)
        {
            this.kernelSize = (radius * 2) + 1;
            this.sigma = sigma;
            this.KernelX = this.CreateGaussianKernel(true);
            this.KernelY = this.CreateGaussianKernel(false);
        }

        /// <summary>
        /// Gets the horizontal gradient operator.
        /// </summary>
        public Fast2DArray<float> KernelX { get; }

        /// <summary>
        /// Gets the vertical gradient operator.
        /// </summary>
        public Fast2DArray<float> KernelY { get; }

        /// <inheritdoc/>
        protected override void OnApply(ImageBase<TColor> source, Rectangle sourceRectangle)
        {
            new Convolution2PassProcessor<TColor>(this.KernelX, this.KernelY).Apply(source, sourceRectangle);
        }

        /// <summary>
        /// Create a 1 dimensional Gaussian kernel using the Gaussian G(x) function
        /// </summary>
        /// <param name="horizontal">Whether to calculate a horizontal kernel.</param>
        /// <returns>The <see cref="Fast2DArray{T}"/></returns>
        private Fast2DArray<float> CreateGaussianKernel(bool horizontal)
        {
            int size = this.kernelSize;
            float weight = this.sigma;
            Fast2DArray<float> kernel = horizontal
                ? new Fast2DArray<float>(new float[1, size])
                : new Fast2DArray<float>(new float[size, 1]);

            float sum = 0;

            float midpoint = (size - 1) / 2f;
            for (int i = 0; i < size; i++)
            {
                float x = i - midpoint;
                float gx = ImageMaths.Gaussian(x, weight);
                sum += gx;
                if (horizontal)
                {
                    kernel[0, i] = gx;
                }
                else
                {
                    kernel[i, 0] = gx;
                }
            }

            // Invert the kernel for sharpening.
            int midpointRounded = (int)midpoint;

            if (horizontal)
            {
                for (int i = 0; i < size; i++)
                {
                    if (i == midpointRounded)
                    {
                        // Calculate central value
                        kernel[0, i] = (2F * sum) - kernel[0, i];
                    }
                    else
                    {
                        // invert value
                        kernel[0, i] = -kernel[0, i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    if (i == midpointRounded)
                    {
                        // Calculate central value
                        kernel[i, 0] = (2 * sum) - kernel[i, 0];
                    }
                    else
                    {
                        // invert value
                        kernel[i, 0] = -kernel[i, 0];
                    }
                }
            }

            // Normalise kernel so that the sum of all weights equals 1
            if (horizontal)
            {
                for (int i = 0; i < size; i++)
                {
                    kernel[0, i] = kernel[0, i] / sum;
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    kernel[i, 0] = kernel[i, 0] / sum;
                }
            }

            return kernel;
        }
    }
}