// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.IO;
using System.Linq;

using SixLabors.ImageSharp.Formats.Experimental.Tiff;
using SixLabors.ImageSharp.Formats.Experimental.Tiff.Constants;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Metadata.Profiles.Icc;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using SixLabors.ImageSharp.PixelFormats;

using Xunit;

using static SixLabors.ImageSharp.Tests.TestImages.Tiff;

namespace SixLabors.ImageSharp.Tests.Formats.Tiff
{
    [Trait("Format", "Tiff")]
    public class TiffMetadataTests
    {
        private readonly Configuration configuration;

        public TiffMetadataTests()
        {
            this.configuration = new Configuration();
            this.configuration.AddTiff();
        }

        [Fact]
        public void CloneIsDeep()
        {
            var meta = new TiffMetadata
            {
                Compression = TiffCompression.Deflate,
                BitsPerPixel = TiffBitsPerPixel.Pixel8,
                ByteOrder = ByteOrder.BigEndian,
                XmpProfile = new byte[3]
            };

            var clone = (TiffMetadata)meta.DeepClone();

            clone.Compression = TiffCompression.None;
            clone.BitsPerPixel = TiffBitsPerPixel.Pixel24;
            clone.ByteOrder = ByteOrder.LittleEndian;
            clone.XmpProfile = new byte[1];

            Assert.False(meta.Compression == clone.Compression);
            Assert.False(meta.BitsPerPixel == clone.BitsPerPixel);
            Assert.False(meta.ByteOrder == clone.ByteOrder);
            Assert.False(meta.XmpProfile.SequenceEqual(clone.XmpProfile));
        }

        [Theory]
        [InlineData(Calliphora_BiColorUncompressed, TiffBitsPerPixel.Pixel1)]
        [InlineData(GrayscaleUncompressed, TiffBitsPerPixel.Pixel8)]
        [InlineData(RgbUncompressed, TiffBitsPerPixel.Pixel24)]
        public void Identify_DetectsCorrectBitPerPixel(string imagePath, TiffBitsPerPixel expectedBitsPerPixel)
        {
            var testFile = TestFile.Create(imagePath);
            using var stream = new MemoryStream(testFile.Bytes, false);

            IImageInfo imageInfo = Image.Identify(this.configuration, stream);

            Assert.NotNull(imageInfo);
            TiffMetadata tiffMetadata = imageInfo.Metadata.GetTiffMetadata();
            Assert.NotNull(tiffMetadata);
            Assert.Equal(expectedBitsPerPixel, tiffMetadata.BitsPerPixel);
        }

        [Theory]
        [InlineData(GrayscaleUncompressed, TiffCompression.None)]
        [InlineData(RgbDeflate, TiffCompression.Deflate)]
        [InlineData(SmallRgbLzw, TiffCompression.Lzw)]
        [InlineData(Calliphora_Fax3Compressed, TiffCompression.CcittGroup3Fax)]
        [InlineData(Calliphora_Fax4Compressed, TiffCompression.CcittGroup4Fax)]
        [InlineData(Calliphora_HuffmanCompressed, TiffCompression.Ccitt1D)]
        [InlineData(Calliphora_RgbPackbits, TiffCompression.PackBits)]
        public void Identify_DetectsCorrectCompression(string imagePath, TiffCompression expectedCompression)
        {
            var testFile = TestFile.Create(imagePath);
            using var stream = new MemoryStream(testFile.Bytes, false);

            IImageInfo imageInfo = Image.Identify(this.configuration, stream);

            Assert.NotNull(imageInfo);
            TiffMetadata tiffMetadata = imageInfo.Metadata.GetTiffMetadata();
            Assert.NotNull(tiffMetadata);
            Assert.Equal(expectedCompression, tiffMetadata.Compression);
        }

        [Theory]
        [InlineData(GrayscaleUncompressed, ByteOrder.BigEndian)]
        [InlineData(LittleEndianByteOrder, ByteOrder.LittleEndian)]
        public void Identify_DetectsCorrectByteOrder(string imagePath, ByteOrder expectedByteOrder)
        {
            var testFile = TestFile.Create(imagePath);
            using var stream = new MemoryStream(testFile.Bytes, false);

            IImageInfo imageInfo = Image.Identify(this.configuration, stream);

            Assert.NotNull(imageInfo);
            TiffMetadata tiffMetadata = imageInfo.Metadata.GetTiffMetadata();
            Assert.NotNull(tiffMetadata);
            Assert.Equal(expectedByteOrder, tiffMetadata.ByteOrder);
        }

        [Theory]
        [WithFile(SampleMetadata, PixelTypes.Rgba32, false)]
        [WithFile(SampleMetadata, PixelTypes.Rgba32, true)]
        public void MetadataProfiles<TPixel>(TestImageProvider<TPixel> provider, bool ignoreMetadata)
          where TPixel : unmanaged, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage(new TiffDecoder() { IgnoreMetadata = ignoreMetadata }))
            {
                TiffMetadata meta = image.Metadata.GetTiffMetadata();
                Assert.NotNull(meta);
                if (ignoreMetadata)
                {
                    Assert.Null(meta.XmpProfile);
                }
                else
                {
                    Assert.NotNull(meta.XmpProfile);
                    Assert.Equal(2599, meta.XmpProfile.Length);
                }
            }
        }

        [Theory]
        [WithFile(SampleMetadata, PixelTypes.Rgba32)]
        public void BaselineTags<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage(new TiffDecoder()))
            {
                TiffMetadata meta = image.Metadata.GetTiffMetadata();

                Assert.NotNull(meta);
                Assert.Equal(ByteOrder.LittleEndian, meta.ByteOrder);
                Assert.Equal(PixelResolutionUnit.PixelsPerInch, image.Metadata.ResolutionUnits);
                Assert.Equal(10, image.Metadata.HorizontalResolution);
                Assert.Equal(10, image.Metadata.VerticalResolution);

                TiffFrameMetadata frame = image.Frames.RootFrame.Metadata.GetTiffMetadata();
                Assert.Equal(32u, frame.Width);
                Assert.Equal(32u, frame.Height);
                Assert.Equal(new ushort[] { 4 }, frame.BitsPerSample);
                Assert.Equal(TiffCompression.Lzw, frame.Compression);
                Assert.Equal(TiffPhotometricInterpretation.PaletteColor, frame.PhotometricInterpretation);
                Assert.Equal("This is Название", frame.ImageDescription);
                Assert.Equal("This is Изготовитель камеры", frame.Make);
                Assert.Equal("This is Модель камеры", frame.Model);
                TiffTestUtils.Compare(new Number[] { 8 }, frame.StripOffsets);
                Assert.Equal(1, frame.SamplesPerPixel);
                Assert.Equal(32u, frame.RowsPerStrip);
                TiffTestUtils.Compare(new Number[] { 297 }, frame.StripByteCounts);
                Assert.Equal(10, frame.HorizontalResolution);
                Assert.Equal(10, frame.VerticalResolution);
                Assert.Equal(TiffPlanarConfiguration.Chunky, frame.PlanarConfiguration);
                Assert.Equal(PixelResolutionUnit.PixelsPerInch, frame.ResolutionUnit);
                Assert.Equal("IrfanView", frame.Software);
                Assert.Null(frame.DateTime);
                Assert.Equal("This is author1;Author2", frame.Artist);
                Assert.Null(frame.HostComputer);
                Assert.Equal(48, frame.ColorMap.Length);
                Assert.Equal(10537, frame.ColorMap[0]);
                Assert.Equal(14392, frame.ColorMap[1]);
                Assert.Equal(58596, frame.ColorMap[46]);
                Assert.Equal(3855, frame.ColorMap[47]);

                Assert.Null(frame.ExtraSamples);
                Assert.Equal(TiffPredictor.None, frame.Predictor);
                Assert.Null(frame.SampleFormat);
                Assert.Equal("This is Авторские права", frame.Copyright);
            }
        }

        [Theory]
        [WithFile(MultiframeDeflateWithPreview, PixelTypes.Rgba32)]
        public void SubfileType<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage(new TiffDecoder()))
            {
                TiffMetadata meta = image.Metadata.GetTiffMetadata();
                Assert.NotNull(meta);

                Assert.Equal(2, image.Frames.Count);

                TiffFrameMetadata frame0 = image.Frames[0].Metadata.GetTiffMetadata();
                Assert.Equal(TiffNewSubfileType.FullImage, frame0.SubfileType);
                Assert.Null(frame0.OldSubfileType);
                Assert.Equal(255u, frame0.Width);
                Assert.Equal(255u, frame0.Height);

                TiffFrameMetadata frame1 = image.Frames[1].Metadata.GetTiffMetadata();
                Assert.Equal(TiffNewSubfileType.Preview, frame1.SubfileType);
                Assert.Null(frame1.OldSubfileType);
                Assert.Equal(255u, frame1.Width);
                Assert.Equal(255u, frame1.Height);
            }
        }

        [Theory]
        [WithFile(SampleMetadata, PixelTypes.Rgba32, true)]
        [WithFile(SampleMetadata, PixelTypes.Rgba32, false)]
        public void PreserveMetadata<TPixel>(TestImageProvider<TPixel> provider, bool preserveMetadata)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            // Load Tiff image
            using Image<TPixel> image = provider.GetImage(new TiffDecoder() { IgnoreMetadata = false });

            ImageMetadata coreMeta = image.Metadata;
            TiffMetadata tiffMeta = image.Metadata.GetTiffMetadata();
            TiffFrameMetadata frameMeta = image.Frames.RootFrame.Metadata.GetTiffMetadata();

            // Save to Tiff
            var tiffEncoder = new TiffEncoder() { Mode = TiffEncodingMode.Rgb };
            if (!preserveMetadata)
            {
                ClearMeta(image);
            }

            using var ms = new MemoryStream();
            image.Save(ms, tiffEncoder);

            // Assert
            ms.Position = 0;
            using var output = Image.Load<Rgba32>(this.configuration, ms);

            ImageMetadata coreMetaOut = output.Metadata;
            TiffMetadata tiffMetaOut = output.Metadata.GetTiffMetadata();
            TiffFrameMetadata frameMetaOut = output.Frames.RootFrame.Metadata.GetTiffMetadata();

            Assert.Equal(TiffBitsPerPixel.Pixel4, tiffMeta.BitsPerPixel);
            Assert.Equal(TiffBitsPerPixel.Pixel24, tiffMetaOut.BitsPerPixel);
            Assert.Equal(TiffCompression.Lzw, tiffMeta.Compression);
            Assert.Equal(TiffCompression.None, tiffMetaOut.Compression);

            Assert.Equal(coreMeta.HorizontalResolution, coreMetaOut.HorizontalResolution);
            Assert.Equal(coreMeta.VerticalResolution, coreMetaOut.VerticalResolution);
            Assert.Equal(coreMeta.ResolutionUnits, coreMetaOut.ResolutionUnits);

            Assert.Equal(frameMeta.Width, frameMetaOut.Width);
            Assert.Equal(frameMeta.Height, frameMetaOut.Height);
            Assert.Equal(frameMeta.ResolutionUnit, frameMetaOut.ResolutionUnit);
            Assert.Equal(frameMeta.HorizontalResolution, frameMetaOut.HorizontalResolution);
            Assert.Equal(frameMeta.VerticalResolution, frameMetaOut.VerticalResolution);

            Assert.Equal("ImageSharp", frameMetaOut.Software);

            if (preserveMetadata)
            {
                Assert.Equal(tiffMeta.XmpProfile, tiffMetaOut.XmpProfile);

                Assert.Equal("IrfanView", frameMeta.Software);
                Assert.Equal("This is Название", frameMeta.ImageDescription);
                Assert.Equal("This is Изготовитель камеры", frameMeta.Make);
                Assert.Equal("This is Авторские права", frameMeta.Copyright);

                Assert.Equal(frameMeta.ImageDescription, frameMetaOut.ImageDescription);
                Assert.Equal(frameMeta.Make, frameMetaOut.Make);
                Assert.Equal(frameMeta.Copyright, frameMetaOut.Copyright);
            }
            else
            {
                Assert.Null(tiffMetaOut.XmpProfile);

                Assert.Null(frameMeta.Software);
                Assert.Null(frameMeta.ImageDescription);
                Assert.Null(frameMeta.Make);
                Assert.Null(frameMeta.Copyright);

                Assert.Null(frameMetaOut.ImageDescription);
                Assert.Null(frameMetaOut.Make);
                Assert.Null(frameMetaOut.Copyright);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CreateMetadata(bool preserveMetadata)
        {
            // Create image
            int w = 10;
            int h = 20;
            using Image image = new Image<Rgb24>(w, h);

            // set metadata
            ImageMetadata coreMeta = image.Metadata;
            TiffMetadata tiffMeta = image.Metadata.GetTiffMetadata();
            TiffFrameMetadata frameMeta = image.Frames.RootFrame.Metadata.GetTiffMetadata();

            tiffMeta.XmpProfile = new byte[] { 1, 2, 3, 4, 5 };

            coreMeta.IptcProfile = new IptcProfile();
            coreMeta.IptcProfile.SetValue(IptcTag.Caption, "iptc caption");

            coreMeta.IccProfile = new IccProfile(new IccProfileHeader() { CreationDate = DateTime.Now }, new IccTagDataEntry[] { new IccTextTagDataEntry("test string"), new IccDataTagDataEntry(new byte[] { 11, 22, 33, 44 }) });

            coreMeta.ResolutionUnits = PixelResolutionUnit.PixelsPerMeter;
            coreMeta.HorizontalResolution = 4500;
            coreMeta.VerticalResolution = 5400;

            var datetime = DateTime.Now.ToString();
            frameMeta.ImageDescription = "test ImageDescription";
            frameMeta.DateTime = datetime;

            // Save to Tiff
            var tiffEncoder = new TiffEncoder { Mode = TiffEncodingMode.Default, Compression = TiffEncoderCompression.Deflate };
            if (!preserveMetadata)
            {
                ClearMeta(image);
            }

            using var ms = new MemoryStream();
            image.Save(ms, tiffEncoder);

            // Assert
            ms.Position = 0;
            using var output = Image.Load<Rgba32>(this.configuration, ms);
            TiffMetadata meta = output.Metadata.GetTiffMetadata();

            ImageMetadata coreMetaOut = output.Metadata;
            TiffMetadata tiffMetaOut = output.Metadata.GetTiffMetadata();
            TiffFrameMetadata frameMetaOut = output.Frames.RootFrame.Metadata.GetTiffMetadata();

            Assert.Equal(PixelResolutionUnit.PixelsPerCentimeter, coreMetaOut.ResolutionUnits);
            Assert.Equal(45, coreMetaOut.HorizontalResolution);
            Assert.Equal(54, coreMetaOut.VerticalResolution, 8);

            //// Assert.Equal(tiffEncoder.Compression, tiffMetaOut.Compression);
            Assert.Equal(TiffBitsPerPixel.Pixel24, tiffMetaOut.BitsPerPixel);

            Assert.Equal((uint)w, frameMetaOut.Width);
            Assert.Equal((uint)h, frameMetaOut.Height);
            Assert.Equal(frameMeta.ResolutionUnit, frameMetaOut.ResolutionUnit);
            Assert.Equal(frameMeta.HorizontalResolution, frameMetaOut.HorizontalResolution);
            Assert.Equal(frameMeta.VerticalResolution, frameMetaOut.VerticalResolution);

            Assert.Equal("ImageSharp", frameMetaOut.Software);

            if (preserveMetadata)
            {
                Assert.NotNull(tiffMeta.XmpProfile);
                Assert.NotNull(coreMeta.IptcProfile);
                Assert.NotNull(coreMeta.IccProfile);

                Assert.Equal(tiffMeta.XmpProfile, tiffMetaOut.XmpProfile);
                Assert.Equal(coreMeta.IptcProfile.Data, coreMetaOut.IptcProfile.Data);
                Assert.Equal(coreMeta.IccProfile.ToByteArray(), coreMetaOut.IccProfile.ToByteArray());

                Assert.Equal("test ImageDescription", frameMeta.ImageDescription);
                Assert.Equal(datetime, frameMeta.DateTime);

                Assert.Equal(frameMeta.ImageDescription, frameMetaOut.ImageDescription);
                Assert.Equal(frameMeta.DateTime, frameMetaOut.DateTime);
            }
            else
            {
                Assert.Null(tiffMetaOut.XmpProfile);
                Assert.Null(coreMetaOut.IptcProfile);
                Assert.Null(coreMetaOut.IccProfile);

                Assert.Null(frameMeta.ImageDescription);
                Assert.Null(frameMeta.DateTime);

                Assert.Null(frameMetaOut.ImageDescription);
                Assert.Null(frameMetaOut.DateTime);
            }
        }

        private static void ClearMeta(Image image)
        {
            ImageMetadata coreMeta = image.Metadata;
            TiffMetadata tiffMeta = image.Metadata.GetTiffMetadata();
            TiffFrameMetadata frameMeta = image.Frames.RootFrame.Metadata.GetTiffMetadata();

            coreMeta.ExifProfile = null;
            coreMeta.IccProfile = null;
            coreMeta.IptcProfile = null;

            tiffMeta.XmpProfile = null;

            frameMeta.ClearMetadata();
        }
    }
}
