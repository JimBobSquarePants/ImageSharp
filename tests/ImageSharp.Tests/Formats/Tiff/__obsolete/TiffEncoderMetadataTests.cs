// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace SixLabors.ImageSharp.Tests
{
    [Trait("Category", "Tiff")]
    public class TiffEncoderMetadataTests
    {
        public static object[][] BaselineMetadataValues = new[] { new object[] { TiffTagId.Artist, TiffMetadataNames.Artist, "My Artist Name" },
                                                                  new object[] { TiffTagId.Copyright, TiffMetadataNames.Copyright, "My Copyright Statement" },
                                                                  new object[] { TiffTagId.DateTime, TiffMetadataNames.DateTime, "My DateTime Value" },
                                                                  new object[] { TiffTagId.HostComputer, TiffMetadataNames.HostComputer, "My Host Computer Name" },
                                                                  new object[] { TiffTagId.ImageDescription, TiffMetadataNames.ImageDescription, "My Image Description" },
                                                                  new object[] { TiffTagId.Make, TiffMetadataNames.Make, "My Camera Make" },
                                                                  new object[] { TiffTagId.Model, TiffMetadataNames.Model, "My Camera Model" },
                                                                  new object[] { TiffTagId.Software, TiffMetadataNames.Software, "My Imaging Software" }};

        [Fact]
        public void AddMetadata_SetsImageResolution()
        {
            Image<Rgba32> image = new Image<Rgba32>(100, 100);
            image.Metadata.HorizontalResolution = 40.0;
            image.Metadata.VerticalResolution = 50.5;
            TiffEncoderCore encoder = new TiffEncoderCore(null);

            List<TiffIfdEntry> ifdEntries = new List<TiffIfdEntry>();
            encoder.AddMetadata(image, ifdEntries);

            Assert.Equal(new Rational(40, 1), ifdEntries.GetUnsignedRational(TiffTagId.XResolution));
            Assert.Equal(new Rational(101, 2), ifdEntries.GetUnsignedRational(TiffTagId.YResolution));
            Assert.Equal(TiffResolutionUnit.Inch, (TiffResolutionUnit?)ifdEntries.GetInteger(TiffTagId.ResolutionUnit));
        }

        [Theory]
        [MemberData(nameof(BaselineMetadataValues))]
        public void AddMetadata_SetsAsciiMetadata(ushort tag, string metadataName, string metadataValue)
        {
            Image<Rgba32> image = new Image<Rgba32>(100, 100);

            TiffFrameMetadata tiffMetadata = image.Frames.RootFrame.Metadata.GetFormatMetadata(TiffFormat.Instance);
            tiffMetadata.TextTags.Add(new TiffMetadataTag(metadataName, metadataValue));
            TiffEncoderCore encoder = new TiffEncoderCore(null);

            List<TiffIfdEntry> ifdEntries = new List<TiffIfdEntry>();
            encoder.AddMetadata(image, ifdEntries);

            Assert.Equal(metadataValue + "\0", ifdEntries.GetAscii(tag));
        }
    }
}
