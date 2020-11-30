// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.IO;

using SixLabors.ImageSharp.Formats.Tiff.Compression;
using SixLabors.ImageSharp.Formats.Tiff.Utils;

using Xunit;

namespace SixLabors.ImageSharp.Tests.Formats.Tiff.Compression
{
    [Trait("Category", "Tiff")]
    public class LzwTiffCompressionTests
    {
        [Theory]
        [InlineData(new byte[] { })]
        [InlineData(new byte[] { 42 })] // One byte
        [InlineData(new byte[] { 42, 16, 128, 53, 96, 218, 7, 64, 3, 4, 97 })] // Random bytes
        [InlineData(new byte[] { 1, 2, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 3, 4 })] // Repeated bytes
        [InlineData(new byte[] { 1, 2, 42, 53, 42, 53, 42, 53, 42, 53, 42, 53, 3, 4 })] // Repeated sequence
        public void Decompress_ReadsData(byte[] data)
        {
            using Stream stream = CreateCompressedStream(data);
            var buffer = new byte[data.Length];

            new LzwTiffCompression(Configuration.Default.MemoryAllocator).Decompress(stream, (int)stream.Length, buffer);

            Assert.Equal(data, buffer);
        }

        private static Stream CreateCompressedStream(byte[] data)
        {
            Stream compressedStream = new MemoryStream();

            using (var encoder = new TiffLzwEncoder(data, 8))
            {
                encoder.Encode(compressedStream);
            }

            compressedStream.Seek(0, SeekOrigin.Begin);
            return compressedStream;
        }
    }
}
