// <copyright file="JpegDecoderTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

// ReSharper disable InconsistentNaming
namespace ImageSharp.Tests
{
    using System;
    using System.IO;

    using ImageSharp.Formats;
    using ImageSharp.PixelFormats;

    using Xunit;

    public class JpegDecoderTests : TestBase
    {
        public static string[] BaselineTestJpegs =
            {
                TestImages.Jpeg.Baseline.Calliphora, TestImages.Jpeg.Baseline.Cmyk,
                TestImages.Jpeg.Baseline.Jpeg400, TestImages.Jpeg.Baseline.Jpeg444,
                TestImages.Jpeg.Baseline.Testimgorig
            };

        public static string[] ProgressiveTestJpegs = TestImages.Jpeg.Progressive.All;

        [Theory]
        [WithFileCollection(nameof(BaselineTestJpegs), PixelTypes.Rgba32 | PixelTypes.Rgba32 | PixelTypes.Argb32)]
        public void OpenBaselineJpeg_SaveBmp<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                provider.Utility.SaveTestOutputFile(image, "bmp");
            }
        }

        [Theory]
        [WithFileCollection(nameof(ProgressiveTestJpegs), PixelTypes.Rgba32 | PixelTypes.Rgba32 | PixelTypes.Argb32)]
        public void OpenProgressiveJpeg_SaveBmp<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                provider.Utility.SaveTestOutputFile(image, "bmp");
            }
        }

        [Theory]
        [WithSolidFilledImages(16, 16, 255, 0, 0, PixelTypes.Rgba32, JpegSubsample.Ratio420, 75)]
        [WithSolidFilledImages(16, 16, 255, 0, 0, PixelTypes.Rgba32, JpegSubsample.Ratio420, 100)]
        [WithSolidFilledImages(16, 16, 255, 0, 0, PixelTypes.Rgba32, JpegSubsample.Ratio444, 75)]
        [WithSolidFilledImages(16, 16, 255, 0, 0, PixelTypes.Rgba32, JpegSubsample.Ratio444, 100)]
        [WithSolidFilledImages(8, 8, 255, 0, 0, PixelTypes.Rgba32, JpegSubsample.Ratio444, 100)]
        public void DecodeGenerated_SaveBmp<TPixel>(
            TestImageProvider<TPixel> provider,
            JpegSubsample subsample,
            int quality)
            where TPixel : struct, IPixel<TPixel>
        {
            byte[] data;
            using (Image<TPixel> image = provider.GetImage())
            {
                JpegEncoder encoder = new JpegEncoder { Subsample = subsample, Quality = quality };

                data = new byte[65536];
                using (MemoryStream ms = new MemoryStream(data))
                {
                    image.Save(ms, encoder);
                }
            }

            // TODO: Automatic image comparers could help here a lot :P
            Image<TPixel> mirror = provider.Factory.CreateImage(data);
            provider.Utility.TestName += $"_{subsample}_Q{quality}";
            provider.Utility.SaveTestOutputFile(mirror, "bmp");
        }

        [Theory]
        [WithSolidFilledImages(42, 88, 255, 0, 0, PixelTypes.Rgba32)]
        public void DecodeGenerated_MetadataOnly<TPixel>(
            TestImageProvider<TPixel> provider)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, new JpegEncoder());
                    ms.Seek(0, SeekOrigin.Begin);
                    
                    using (JpegDecoderCore decoder = new JpegDecoderCore(null, new JpegDecoder()))
                    {
                        Image<TPixel> mirror = decoder.Decode<TPixel>(ms);

                        Assert.Equal(decoder.ImageWidth, image.Width);
                        Assert.Equal(decoder.ImageHeight, image.Height);
                    }
                }
            }
        }

        [Fact]
        public void Decoder_Reads_Correct_Resolution_From_Jfif()
        {
            using (Image<Rgba32> image = TestFile.Create(TestImages.Jpeg.Baseline.Floorplan).CreateImage())
            {
                Assert.Equal(300, image.MetaData.HorizontalResolution);
                Assert.Equal(300, image.MetaData.VerticalResolution);
            }
        }

        [Fact]
        public void Decoder_Reads_Correct_Resolution_From_Exif()
        {
            using (Image<Rgba32> image = TestFile.Create(TestImages.Jpeg.Baseline.Jpeg420).CreateImage())
            {
                Assert.Equal(72, image.MetaData.HorizontalResolution);
                Assert.Equal(72, image.MetaData.VerticalResolution);
            }
        }

        [Fact]
        public void Decode_IgnoreMetadataIsFalse_ExifProfileIsRead()
        {
            JpegDecoder decoder = new JpegDecoder()
            {
                IgnoreMetadata = false
            };

            TestFile testFile = TestFile.Create(TestImages.Jpeg.Baseline.Floorplan);

            using (Image<Rgba32> image = testFile.CreateImage(decoder))
            {
                Assert.NotNull(image.MetaData.ExifProfile);
            }
        }

        [Fact]
        public void Decode_IgnoreMetadataIsTrue_ExifProfileIgnored()
        {
            JpegDecoder options = new JpegDecoder()
            {
                IgnoreMetadata = true
            };

            TestFile testFile = TestFile.Create(TestImages.Jpeg.Baseline.Floorplan);

            using (Image<Rgba32> image = testFile.CreateImage(options))
            {
                Assert.Null(image.MetaData.ExifProfile);
            }
        }

        [Theory]
        [InlineData(TestImages.Jpeg.Progressive.Progress, 24)]
        [InlineData(TestImages.Jpeg.Progressive.Fb, 24)]
        [InlineData(TestImages.Jpeg.Baseline.Cmyk, 32)]
        [InlineData(TestImages.Jpeg.Baseline.Ycck, 32)]
        [InlineData(TestImages.Jpeg.Baseline.Jpeg400, 8)]
        [InlineData(TestImages.Jpeg.Baseline.Snake, 24)]
        public void DetectPixelSize(string imagePath, int expectedPixelSize)
        {
            TestFile testFile = TestFile.Create(imagePath);
            using (var stream = new MemoryStream(testFile.Bytes, false))
            {
                Assert.Equal(expectedPixelSize, Image.DetectPixelType(stream)?.BitsPerPixel);
            }
        }
    }
}