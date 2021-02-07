// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.IO;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Formats.Experimental.Tiff.Writers
{
    internal class TiffRgbWriter<TPixel> : TiffCompositeColorWriter<TPixel>
        where TPixel : unmanaged, IPixel<TPixel>
    {
        public TiffRgbWriter(ImageFrame<TPixel> image, MemoryAllocator memoryAllocator, Configuration configuration, TiffEncoderEntriesCollector entriesCollector)
            : base(image, memoryAllocator, configuration, entriesCollector)
        {
        }

        public override int BitsPerPixel => 24;

        protected override void EncodePixels(Span<TPixel> pixels, Span<byte> buffer) => PixelOperations<TPixel>.Instance.ToRgb24Bytes(this.Configuration, pixels, buffer, pixels.Length);
    }
}
