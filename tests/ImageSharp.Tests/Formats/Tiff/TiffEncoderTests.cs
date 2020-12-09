// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.IO;

using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Experimental.Tiff;
using SixLabors.ImageSharp.Formats.Experimental.Tiff.Constants;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Tests.TestUtilities.ImageComparison;
using SixLabors.ImageSharp.Tests.TestUtilities.ReferenceCodecs;

using Xunit;

using static SixLabors.ImageSharp.Tests.TestImages.Tiff;

namespace SixLabors.ImageSharp.Tests.Formats.Tiff
{
    [Trait("Format", "Tiff")]
    public class TiffEncoderTests
    {
        private static readonly IImageDecoder ReferenceDecoder = new MagickReferenceDecoder();

        private readonly Configuration configuration;

        public TiffEncoderTests()
        {
            this.configuration = new Configuration();
            this.configuration.ImageFormatsManager.AddImageFormat(TiffFormat.Instance);
            this.configuration.ImageFormatsManager.AddImageFormatDetector(new TiffImageFormatDetector());
            this.configuration.ImageFormatsManager.SetDecoder(TiffFormat.Instance, new TiffDecoder());
            this.configuration.ImageFormatsManager.SetEncoder(TiffFormat.Instance, new TiffEncoder());
        }

        [Theory]
        [InlineData(TiffEncodingMode.Default, TiffEncoderCompression.None, TiffBitsPerPixel.Pixel24, TiffCompression.None)]
        [InlineData(TiffEncodingMode.Rgb, TiffEncoderCompression.None, TiffBitsPerPixel.Pixel24, TiffCompression.None)]
        [InlineData(TiffEncodingMode.ColorPalette, TiffEncoderCompression.None, TiffBitsPerPixel.Pixel8, TiffCompression.None)]
        [InlineData(TiffEncodingMode.Gray, TiffEncoderCompression.None, TiffBitsPerPixel.Pixel8, TiffCompression.None)]
        [InlineData(TiffEncodingMode.BiColor, TiffEncoderCompression.None, TiffBitsPerPixel.Pixel1, TiffCompression.None)]
        [InlineData(TiffEncodingMode.Default, TiffEncoderCompression.Deflate, TiffBitsPerPixel.Pixel24, TiffCompression.Deflate)]
        [InlineData(TiffEncodingMode.Rgb, TiffEncoderCompression.Deflate, TiffBitsPerPixel.Pixel24, TiffCompression.Deflate)]
        [InlineData(TiffEncodingMode.ColorPalette, TiffEncoderCompression.Deflate, TiffBitsPerPixel.Pixel8, TiffCompression.Deflate)]
        [InlineData(TiffEncodingMode.Gray, TiffEncoderCompression.Deflate, TiffBitsPerPixel.Pixel8, TiffCompression.Deflate)]
        [InlineData(TiffEncodingMode.BiColor, TiffEncoderCompression.Deflate, TiffBitsPerPixel.Pixel1, TiffCompression.Deflate)]
        [InlineData(TiffEncodingMode.Default, TiffEncoderCompression.PackBits, TiffBitsPerPixel.Pixel24, TiffCompression.PackBits)]
        [InlineData(TiffEncodingMode.Rgb, TiffEncoderCompression.PackBits, TiffBitsPerPixel.Pixel24, TiffCompression.PackBits)]
        [InlineData(TiffEncodingMode.ColorPalette, TiffEncoderCompression.PackBits, TiffBitsPerPixel.Pixel8, TiffCompression.PackBits)]
        [InlineData(TiffEncodingMode.Gray, TiffEncoderCompression.PackBits, TiffBitsPerPixel.Pixel8, TiffCompression.PackBits)]
        [InlineData(TiffEncodingMode.BiColor, TiffEncoderCompression.PackBits, TiffBitsPerPixel.Pixel1, TiffCompression.PackBits)]
        [InlineData(TiffEncodingMode.Rgb, TiffEncoderCompression.Lzw, TiffBitsPerPixel.Pixel24, TiffCompression.Lzw)]
        [InlineData(TiffEncodingMode.ColorPalette, TiffEncoderCompression.Lzw, TiffBitsPerPixel.Pixel8, TiffCompression.Lzw)]
        [InlineData(TiffEncodingMode.Gray, TiffEncoderCompression.Lzw, TiffBitsPerPixel.Pixel8, TiffCompression.Lzw)]
        [InlineData(TiffEncodingMode.BiColor, TiffEncoderCompression.CcittGroup3Fax, TiffBitsPerPixel.Pixel1, TiffCompression.CcittGroup3Fax)]
        [InlineData(TiffEncodingMode.BiColor, TiffEncoderCompression.ModifiedHuffman, TiffBitsPerPixel.Pixel1, TiffCompression.Ccitt1D)]
        public void EncoderOptions_Work(TiffEncodingMode mode, TiffEncoderCompression compression, TiffBitsPerPixel expectedBitsPerPixel, TiffCompression expectedCompression)
        {
            // arrange
            var tiffEncoder = new TiffEncoder { Mode = mode, Compression = compression };
            Image input = new Image<Rgb24>(10, 10);
            using var memStream = new MemoryStream();

            // act
            input.Save(memStream, tiffEncoder);

            // assert
            memStream.Position = 0;
            using var output = Image.Load<Rgba32>(this.configuration, memStream);
            TiffMetadata meta = output.Metadata.GetTiffMetadata();
            Assert.Equal(expectedBitsPerPixel, meta.BitsPerPixel);
            Assert.Equal(expectedCompression, meta.Compression);
        }

        [Theory]
        [WithFile(Calliphora_BiColorUncompressed, PixelTypes.Rgba32, TiffBitsPerPixel.Pixel1)]
        [WithFile(GrayscaleUncompressed, PixelTypes.Rgba32, TiffBitsPerPixel.Pixel8)]
        [WithFile(RgbUncompressed, PixelTypes.Rgba32, TiffBitsPerPixel.Pixel24)]
        public void TiffEncoder_PreserveBitsPerPixel<TPixel>(TestImageProvider<TPixel> provider, TiffBitsPerPixel expectedBitsPerPixel)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            // arrange
            var tiffEncoder = new TiffEncoder();
            using Image<TPixel> input = provider.GetImage();
            using var memStream = new MemoryStream();

            // act
            input.Save(memStream, tiffEncoder);

            // assert
            memStream.Position = 0;
            using var output = Image.Load<Rgba32>(this.configuration, memStream);
            TiffMetadata meta = output.Metadata.GetTiffMetadata();
            Assert.Equal(expectedBitsPerPixel, meta.BitsPerPixel);
        }

        [Theory]
        [WithFile(Calliphora_RgbUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeRgb_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.Rgb);

        [Theory]
        [WithFile(Calliphora_RgbUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeRgb_WithDeflateCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.Rgb, TiffEncoderCompression.Deflate);

        [Theory]
        [WithFile(Calliphora_RgbUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeRgb_WithDeflateCompressionAndPredictor_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.Rgb, TiffEncoderCompression.Deflate, usePredictor: true);

        [Theory]
        [WithFile(Calliphora_RgbUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeRgb_WithLzwCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.Rgb, TiffEncoderCompression.Lzw);

        [Theory]
        [WithFile(Calliphora_RgbUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeRgb_WithLzwCompressionAndPredictor_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.Rgb, TiffEncoderCompression.Lzw, usePredictor: true);

        [Theory]
        [WithFile(Calliphora_RgbUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeRgb_WithPackBitsCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.Rgb, TiffEncoderCompression.PackBits);

        [Theory]
        [WithFile(Calliphora_GrayscaleUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeGray_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel8, TiffEncodingMode.Gray);

        [Theory]
        [WithFile(Calliphora_GrayscaleUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeGray_WithDeflateCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel8, TiffEncodingMode.Gray, TiffEncoderCompression.Deflate);

        [Theory]
        [WithFile(Calliphora_GrayscaleUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeGray_WithDeflateCompressionAndPredictor_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel8, TiffEncodingMode.Gray, TiffEncoderCompression.Deflate, usePredictor: true);

        [Theory]
        [WithFile(Calliphora_GrayscaleUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeGray_WithLzwCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel8, TiffEncodingMode.Gray, TiffEncoderCompression.Lzw);

        [Theory]
        [WithFile(Calliphora_GrayscaleUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeGray_WithLzwCompressionAndPredictor_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel8, TiffEncodingMode.Gray, TiffEncoderCompression.Lzw, usePredictor: true);

        [Theory]
        [WithFile(Calliphora_GrayscaleUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeGray_WithPackBitsCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel8, TiffEncodingMode.Gray, TiffEncoderCompression.PackBits);

        [Theory]
        [WithFile(Calliphora_PaletteUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeColorPalette_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var encoder = new TiffEncoder { Mode = TiffEncodingMode.ColorPalette, Compression = TiffEncoderCompression.None };

            this.TiffEncoderPaletteTest(provider, encoder);
        }

        [Theory]
        [WithFile(Calliphora_PaletteUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeColorPalette_WithDeflateCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var encoder = new TiffEncoder { Mode = TiffEncodingMode.ColorPalette, Compression = TiffEncoderCompression.Deflate };

            this.TiffEncoderPaletteTest(provider, encoder);
        }

        [Theory]
        [WithFile(Calliphora_PaletteUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeColorPalette_WithDeflateCompressionAndPredictor_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var encoder = new TiffEncoder { Mode = TiffEncodingMode.ColorPalette, Compression = TiffEncoderCompression.Deflate, UseHorizontalPredictor = true };

            this.TiffEncoderPaletteTest(provider, encoder);
        }

        [Theory]
        [WithFile(Calliphora_PaletteUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeColorPalette_WithLzwCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var encoder = new TiffEncoder { Mode = TiffEncodingMode.ColorPalette, Compression = TiffEncoderCompression.Lzw };

            this.TiffEncoderPaletteTest(provider, encoder);
        }

        [Theory]
        [WithFile(Calliphora_PaletteUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeColorPalette_WithLzwCompressionAndPredictor_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var encoder = new TiffEncoder { Mode = TiffEncodingMode.ColorPalette, Compression = TiffEncoderCompression.Lzw, UseHorizontalPredictor = true };

            this.TiffEncoderPaletteTest(provider, encoder);
        }

        [Theory]
        [WithFile(Calliphora_PaletteUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeColorPalette_WithPackBitsCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var encoder = new TiffEncoder { Mode = TiffEncodingMode.ColorPalette, Compression = TiffEncoderCompression.PackBits };

            this.TiffEncoderPaletteTest(provider, encoder);
        }

        private void TiffEncoderPaletteTest<TPixel>(TestImageProvider<TPixel> provider, TiffEncoder encoder)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            // Because a quantizer is used to create the palette (and therefore changes to the original are expected),
            // we do not compare the encoded image against the original:
            // Instead we load the encoded image with a reference decoder and compare against that image.
            using Image<TPixel> image = provider.GetImage();
            using var memStream = new MemoryStream();

            image.Save(memStream, encoder);
            memStream.Position = 0;

            using var encodedImage = (Image<TPixel>)Image.Load(this.configuration, memStream);
            var encodedImagePath = provider.Utility.SaveTestOutputFile(encodedImage, "tiff", encoder);
            TiffTestUtils.CompareWithReferenceDecoder(encodedImagePath, encodedImage);
        }

        [Theory]
        [WithFile(Calliphora_BiColorUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeBiColor_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.BiColor);

        [Theory]
        [WithFile(Calliphora_BiColorUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeBiColor_WithDeflateCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel1, TiffEncodingMode.BiColor, TiffEncoderCompression.Deflate);

        [Theory]
        [WithFile(Calliphora_BiColorUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeBiColor_WithPackBitsCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel1, TiffEncodingMode.BiColor, TiffEncoderCompression.PackBits);

        [Theory]
        [WithFile(Calliphora_BiColorUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeBiColor_WithCcittGroup3FaxCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel1, TiffEncodingMode.BiColor, TiffEncoderCompression.CcittGroup3Fax);

        [Theory]
        [WithFile(Calliphora_BiColorUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeBiColor_WithModifiedHuffmanCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel1, TiffEncodingMode.BiColor, TiffEncoderCompression.ModifiedHuffman);

        private static void TestTiffEncoderCore<TPixel>(
            TestImageProvider<TPixel> provider,
            TiffBitsPerPixel bitsPerPixel,
            TiffEncodingMode mode,
            TiffEncoderCompression compression = TiffEncoderCompression.None,
            bool usePredictor = false,
            bool useExactComparer = true,
            float compareTolerance = 0.01f)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            using Image<TPixel> image = provider.GetImage();
            var encoder = new TiffEncoder { Mode = mode, Compression = compression, UseHorizontalPredictor = usePredictor };

            // Does DebugSave & load reference CompareToReferenceInput():
            image.VerifyEncoder(provider, "tiff", bitsPerPixel, encoder, useExactComparer ? ImageComparer.Exact : ImageComparer.Tolerant(compareTolerance), referenceDecoder: ReferenceDecoder);
        }
    }
}
