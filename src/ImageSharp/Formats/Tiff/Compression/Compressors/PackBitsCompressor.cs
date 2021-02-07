// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.IO;
using SixLabors.ImageSharp.Formats.Experimental.Tiff.Compression.Compressors;
using SixLabors.ImageSharp.Formats.Experimental.Tiff.Constants;
using SixLabors.ImageSharp.Memory;

namespace SixLabors.ImageSharp.Formats.Experimental.Tiff.Compression.Compressors
{
    internal class PackBitsCompressor : TiffBaseCompressor
    {
        private IManagedByteBuffer pixelData;

        public PackBitsCompressor(Stream output, MemoryAllocator allocator, int width, int bitsPerPixel)
            : base(output, allocator, width, bitsPerPixel)
        {
        }

        public override TiffEncoderCompression Method => TiffEncoderCompression.PackBits;

        public override void Initialize(int rowsPerStrip)
        {
            int additionalBytes = (this.BytesPerRow / 127) + 1;
            this.pixelData = this.Allocator.AllocateManagedByteBuffer((this.BytesPerRow + additionalBytes) * rowsPerStrip);
        }

        public override void CompressStrip(Span<byte> rows, int height)
        {
            this.pixelData.Clear();
            Span<byte> span = this.pixelData.GetSpan();
            int size = PackBitsWriter.PackBits(rows, span);
            this.Output.Write(span.Slice(0, size));
        }

        protected override void Dispose(bool disposing) => this.pixelData?.Dispose();
    }
}
