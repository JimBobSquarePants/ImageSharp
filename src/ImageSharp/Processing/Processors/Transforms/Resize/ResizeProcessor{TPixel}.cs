// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Processing.Processors.Transforms
{
    /// <summary>
    /// Implements resizing of images using various resamplers.
    /// </summary>
    /// <remarks>
    /// The original code has been adapted from <see href="http://www.realtimerendering.com/resources/GraphicsGems/gemsiii/filter_rcg.c"/>.
    /// </remarks>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal class ResizeProcessor<TPixel> : TransformProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        private bool isDisposed;
        private readonly int targetWidth;
        private readonly int targetHeight;
        private readonly IResampler resampler;
        private readonly Rectangle targetRectangle;
        private readonly bool compand;

        // The following fields are not immutable but are optionally created on demand.
        private ResizeKernelMap horizontalKernelMap;
        private ResizeKernelMap verticalKernelMap;

        public ResizeProcessor(Configuration configuration, ResizeProcessor definition, Image<TPixel> source, Rectangle sourceRectangle)
            : base(configuration, source, sourceRectangle)
        {
            this.targetWidth = definition.TargetWidth;
            this.targetHeight = definition.TargetHeight;
            this.targetRectangle = definition.TargetRectangle;
            this.resampler = definition.Sampler;
            this.compand = definition.Compand;
        }

        /// <inheritdoc/>
        protected override Size GetTargetSize() => new Size(this.targetWidth, this.targetHeight);

        /// <inheritdoc/>
        protected override void BeforeImageApply(Image<TPixel> destination)
        {
            if (!(this.resampler is NearestNeighborResampler))
            {
                Image<TPixel> source = this.Source;
                Rectangle sourceRectangle = this.SourceRectangle;

                // Since all image frame dimensions have to be the same we can calculate this for all frames.
                MemoryAllocator memoryAllocator = source.GetMemoryAllocator();
                this.horizontalKernelMap = ResizeKernelMap.Calculate(
                    this.resampler,
                    this.targetRectangle.Width,
                    sourceRectangle.Width,
                    memoryAllocator);

                this.verticalKernelMap = ResizeKernelMap.Calculate(
                    this.resampler,
                    this.targetRectangle.Height,
                    sourceRectangle.Height,
                    memoryAllocator);
            }

            base.BeforeImageApply(destination);
        }

        /// <inheritdoc/>
        protected override void OnFrameApply(ImageFrame<TPixel> source, ImageFrame<TPixel> destination)
        {
            Rectangle sourceRectangle = this.SourceRectangle;
            Configuration configuration = this.Configuration;

            // Handle resize dimensions identical to the original
            if (source.Width == destination.Width
                && source.Height == destination.Height
                && sourceRectangle == this.targetRectangle)
            {
                // The cloned will be blank here copy all the pixel data over
                source.GetPixelSpan().CopyTo(destination.GetPixelSpan());
                return;
            }

            int width = this.targetWidth;
            int height = this.targetHeight;
            var interest = Rectangle.Intersect(this.targetRectangle, new Rectangle(0, 0, width, height));

            if (this.resampler is NearestNeighborResampler)
            {
                // Scaling factors
                float widthFactor = sourceRectangle.Width / (float)this.targetRectangle.Width;
                float heightFactor = sourceRectangle.Height / (float)this.targetRectangle.Height;

                ParallelRowIterator.IterateRows(
                    interest,
                    configuration,
                    new RowIntervalOperation(sourceRectangle, this.targetRectangle, widthFactor, heightFactor, source, destination));

                return;
            }

            PixelConversionModifiers conversionModifiers =
                PixelConversionModifiers.Premultiply.ApplyCompanding(this.compand);

            BufferArea<TPixel> sourceArea = source.PixelBuffer.GetArea(sourceRectangle);

            // To reintroduce parallel processing, we to launch multiple workers
            // for different row intervals of the image.
            using (var worker = new ResizeWorker<TPixel>(
                configuration,
                sourceArea,
                conversionModifiers,
                this.horizontalKernelMap,
                this.verticalKernelMap,
                width,
                interest,
                this.targetRectangle.Location))
            {
                worker.Initialize();

                var workingInterval = new RowInterval(interest.Top, interest.Bottom);
                worker.FillDestinationPixels(workingInterval, destination.PixelBuffer);
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.horizontalKernelMap?.Dispose();
                this.horizontalKernelMap = null;
                this.verticalKernelMap?.Dispose();
                this.verticalKernelMap = null;
            }

            this.isDisposed = true;
            base.Dispose(disposing);
        }

        private readonly struct RowIntervalOperation : IRowIntervalOperation
        {
            private readonly Rectangle sourceBounds;
            private readonly Rectangle destinationBounds;
            private readonly float widthFactor;
            private readonly float heightFactor;
            private readonly ImageFrame<TPixel> source;
            private readonly ImageFrame<TPixel> destination;

            [MethodImpl(InliningOptions.ShortMethod)]
            public RowIntervalOperation(
                Rectangle sourceBounds,
                Rectangle destinationBounds,
                float widthFactor,
                float heightFactor,
                ImageFrame<TPixel> source,
                ImageFrame<TPixel> destination)
            {
                this.sourceBounds = sourceBounds;
                this.destinationBounds = destinationBounds;
                this.widthFactor = widthFactor;
                this.heightFactor = heightFactor;
                this.source = source;
                this.destination = destination;
            }

            [MethodImpl(InliningOptions.ShortMethod)]
            public void Invoke(in RowInterval rows)
            {
                int sourceX = this.sourceBounds.X;
                int sourceY = this.sourceBounds.Y;
                int destX = this.destinationBounds.X;
                int destY = this.destinationBounds.Y;
                int destLeft = this.destinationBounds.Left;
                int destRight = this.destinationBounds.Right;

                for (int y = rows.Min; y < rows.Max; y++)
                {
                    // Y coordinates of source points
                    Span<TPixel> sourceRow = this.source.GetPixelRowSpan((int)(((y - destY) * this.heightFactor) + sourceY));
                    Span<TPixel> targetRow = this.destination.GetPixelRowSpan(y);

                    for (int x = destLeft; x < destRight; x++)
                    {
                        // X coordinates of source points
                        targetRow[x] = sourceRow[(int)(((x - destX) * this.widthFactor) + sourceX)];
                    }
                }
            }
        }
    }
}
