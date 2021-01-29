// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;

using SixLabors.ImageSharp.Formats.Experimental.Tiff.Constants;
using SixLabors.ImageSharp.IO;
using SixLabors.ImageSharp.Memory;

namespace SixLabors.ImageSharp.Formats.Experimental.Tiff.Compression
{
    /// <summary>
    /// Class to handle cases where TIFF image data is compressed using CCITT T4 compression.
    /// </summary>
    internal class T4TiffCompression : TiffBaseCompression
    {
        private readonly FaxCompressionOptions faxCompressionOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="T4TiffCompression" /> class.
        /// </summary>
        /// <param name="allocator">The memory allocator.</param>
        /// <param name="faxOptions">Fax compression options.</param>
        /// <param name="photometricInterpretation">The photometric interpretation.</param>
        /// <param name="width">The image width.</param>
        public T4TiffCompression(MemoryAllocator allocator, FaxCompressionOptions faxOptions, TiffPhotometricInterpretation photometricInterpretation, int width)
            : base(allocator, photometricInterpretation, width) => this.faxCompressionOptions = faxOptions;

        /// <inheritdoc/>
        protected override void Decompress(BufferedReadStream stream, int byteCount, Span<byte> buffer)
        {
            if (this.faxCompressionOptions.HasFlag(FaxCompressionOptions.TwoDimensionalCoding))
            {
                TiffThrowHelper.ThrowNotSupported("TIFF CCITT 2D compression is not yet supported");
            }

            bool isWhiteZero = this.PhotometricInterpretation == TiffPhotometricInterpretation.WhiteIsZero;
            byte whiteValue = (byte)(isWhiteZero ? 0 : 1);
            byte blackValue = (byte)(isWhiteZero ? 1 : 0);

            var eolPadding = this.faxCompressionOptions.HasFlag(FaxCompressionOptions.EolPadding);
            using var bitReader = new T4BitReader(stream, byteCount, this.Allocator, eolPadding);

            buffer.Clear();
            uint bitsWritten = 0;
            while (bitReader.HasMoreData)
            {
                bitReader.ReadNextRun();

                if (bitReader.RunLength > 0)
                {
                    if (bitReader.IsWhiteRun)
                    {
                        BitWriterUtils.WriteBits(buffer, (int)bitsWritten, bitReader.RunLength, whiteValue);
                        bitsWritten += bitReader.RunLength;
                    }
                    else
                    {
                        BitWriterUtils.WriteBits(buffer, (int)bitsWritten, bitReader.RunLength, blackValue);
                        bitsWritten += bitReader.RunLength;
                    }
                }

                if (bitReader.IsEndOfScanLine)
                {
                    // Write padding bytes, if necessary.
                    uint pad = 8 - (bitsWritten % 8);
                    if (pad != 8)
                    {
                        BitWriterUtils.WriteBits(buffer, (int)bitsWritten, pad, 0);
                        bitsWritten += pad;
                    }
                }
            }
        }
    }
}
