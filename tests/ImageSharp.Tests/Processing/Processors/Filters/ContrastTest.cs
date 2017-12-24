﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Tests.TestUtilities.ImageComparison;

using SixLabors.Primitives;
using Xunit;

namespace SixLabors.ImageSharp.Tests.Processing.Processors.Effects
{
    public class ContrastTest
    {
        public static readonly TheoryData<float> ContrastValues
        = new TheoryData<float>
        {
            .5F,
           1.5F
        };

        [Theory]
        [WithTestPatternImages(nameof(ContrastValues), 48, 48, PixelTypes.Rgba32)]
        public void ApplyContrastFilter<TPixel>(TestImageProvider<TPixel> provider, float value)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                image.Mutate(x => x.Contrast(value));
                image.DebugSave(provider, value);
            }
        }

        [Theory]
        [WithTestPatternImages(nameof(ContrastValues), 48, 48, PixelTypes.Rgba32)]
        public void ApplyContrastFilterInBox<TPixel>(TestImageProvider<TPixel> provider, float value)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> source = provider.GetImage())
            using (Image<TPixel> image = source.Clone())
            {
                var bounds = new Rectangle(image.Width / 4, image.Width / 4, image.Width / 2, image.Height / 2);

                image.Mutate(x => x.Contrast(value, bounds));
                image.DebugSave(provider, value);

                ImageComparer.Tolerant().VerifySimilarityIgnoreRegion(source, image, bounds);
            }
        }
    }
}