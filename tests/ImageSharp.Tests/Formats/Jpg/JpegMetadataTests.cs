// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.Formats.Jpeg;
using Xunit;

namespace SixLabors.ImageSharp.Tests.Formats.Jpg
{
    [Trait("Format", "Jpg")]
    public class JpegMetadataTests
    {
        [Fact]
        public void CloneIsDeep()
        {
            var meta = new JpegMetadata { Quality = 50, ColorType = JpegColorType.Luminance };
            var clone = (JpegMetadata)meta.DeepClone();

            clone.Quality = 99;
            clone.ColorType = JpegColorType.YCbCr;

            Assert.False(meta.Quality.Equals(clone.Quality));
            Assert.False(meta.ColorType.Equals(clone.ColorType));
        }
    }
}
