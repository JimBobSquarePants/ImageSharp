// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.Drawing.Imaging;
using System.IO;

using BenchmarkDotNet.Attributes;

using SixLabors.ImageSharp.Formats.Experimental.Tiff;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Tests;

namespace SixLabors.ImageSharp.Benchmarks.Codecs
{
    [MarkdownExporter]
    [HtmlExporter]
    [Config(typeof(Config.ShortMultiFramework))]
    public class EncodeTiff
    {
        private System.Drawing.Image drawing;
        private Image<Rgba32> core;

        private Configuration configuration;

        private string TestImageFullPath => Path.Combine(TestEnvironment.InputImagesDirectoryFullPath, this.TestImage);

        [Params(TestImages.Tiff.RgbUncompressed)]
        public string TestImage { get; set; }

        [Params(
            TiffEncoderCompression.None,
            ////TiffEncoderCompression.Deflate,
            TiffEncoderCompression.Lzw,
            ////TiffEncoderCompression.PackBits,
            TiffEncoderCompression.CcittGroup3Fax,
            TiffEncoderCompression.ModifiedHuffman)]
        public TiffEncoderCompression Compression { get; set; }

        [GlobalSetup]
        public void ReadImages()
        {
            if (this.core == null)
            {
                this.configuration = new Configuration();
                this.configuration.AddTiff();

                this.core = Image.Load<Rgba32>(this.configuration, this.TestImageFullPath);
                this.drawing = System.Drawing.Image.FromFile(this.TestImageFullPath);
            }
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            this.core.Dispose();
            this.drawing.Dispose();
        }

        [Benchmark(Baseline = true, Description = "System.Drawing Tiff")]
        public void SystemDrawing()
        {
            ImageCodecInfo codec = FindCodecForType("image/tiff");
            using var parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Compression, (long)Cast(this.Compression));

            using var memoryStream = new MemoryStream();
            this.drawing.Save(memoryStream, codec, parameters);
        }

        [Benchmark(Description = "ImageSharp Tiff")]
        public void TiffCore()
        {
            var encoder = new TiffEncoder() { Compression = this.Compression };
            using var memoryStream = new MemoryStream();
            this.core.SaveAsTiff(memoryStream, encoder);
        }

        private static ImageCodecInfo FindCodecForType(string mimeType)
        {
            ImageCodecInfo[] imgEncoders = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < imgEncoders.GetLength(0); i++)
            {
                if (imgEncoders[i].MimeType == mimeType)
                {
                    return imgEncoders[i];
                }
            }

            return null;
        }

        private static EncoderValue Cast(TiffEncoderCompression compression)
        {
            switch (compression)
            {
                case TiffEncoderCompression.None:
                    return EncoderValue.CompressionNone;

                case TiffEncoderCompression.CcittGroup3Fax:
                    return EncoderValue.CompressionCCITT3;

                case TiffEncoderCompression.ModifiedHuffman:
                    return EncoderValue.CompressionRle;

                case TiffEncoderCompression.Lzw:
                    return EncoderValue.CompressionLZW;

                default:
                    throw new System.ArgumentOutOfRangeException(nameof(compression));
            }
        }
    }
}
