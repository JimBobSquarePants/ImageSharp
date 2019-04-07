﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.PixelFormats;

using Xunit;

namespace SixLabors.ImageSharp.Tests.Processing.Filters
{
    using SixLabors.ImageSharp.Processing;
    using SixLabors.ImageSharp.Processing.Processors.Filters;

    public class FilterTest : BaseImageOperationsExtensionTest
    {
        [Fact]
        public void Filter_CorrectProcessor()
        {
            this.operations.Filter(KnownFilterMatrices.AchromatomalyFilter * KnownFilterMatrices.CreateHueFilter(90F));
            FilterProcessor p = this.Verify<FilterProcessor>();
        }

        [Fact]
        public void Filter_rect_CorrectProcessor()
        {
            this.operations.Filter(KnownFilterMatrices.AchromatomalyFilter * KnownFilterMatrices.CreateHueFilter(90F), this.rect);
            FilterProcessor p = this.Verify<FilterProcessor>(this.rect);
        }
    }
}