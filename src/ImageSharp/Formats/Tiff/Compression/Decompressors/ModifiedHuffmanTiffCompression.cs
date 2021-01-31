// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;

using SixLabors.ImageSharp.Formats.Experimental.Tiff.Constants;
using SixLabors.ImageSharp.IO;
using SixLabors.ImageSharp.Memory;

namespace SixLabors.ImageSharp.Formats.Experimental.Tiff.Compression.Decompressors
{
    /// <summary>
    /// Class to handle cases where TIFF image data is compressed using Modified Huffman Compression.
    /// </summary>
    internal class ModifiedHuffmanTiffCompression : T4TiffCompression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiedHuffmanTiffCompression" /> class.
        /// </summary>
        /// <param name="allocator">The memory allocator.</param>
        /// <param name="photometricInterpretation">The photometric interpretation.</param>
        /// <param name="width">The image width.</param>
        public ModifiedHuffmanTiffCompression(MemoryAllocator allocator, TiffPhotometricInterpretation photometricInterpretation, int width)
            : base(allocator, FaxCompressionOptions.None, photometricInterpretation, width)
        {
        }

        /// <inheritdoc/>
        protected override void Decompress(BufferedReadStream stream, int byteCount, Span<byte> buffer)
        {
            bool isWhiteZero = this.PhotometricInterpretation == TiffPhotometricInterpretation.WhiteIsZero;
            byte whiteValue = (byte)(isWhiteZero ? 0 : 1);
            byte blackValue = (byte)(isWhiteZero ? 1 : 0);

            using var bitReader = new T4BitReader(stream, byteCount, this.Allocator, eolPadding: false, isModifiedHuffman: true);

            buffer.Clear();
            uint bitsWritten = 0;
            uint pixelsWritten = 0;
            while (bitReader.HasMoreData)
            {
                bitReader.ReadNextRun();

                if (bitReader.RunLength > 0)
                {
                    if (bitReader.IsWhiteRun)
                    {
                        BitWriterUtils.WriteBits(buffer, (int)bitsWritten, bitReader.RunLength, whiteValue);
                        bitsWritten += bitReader.RunLength;
                        pixelsWritten += bitReader.RunLength;
                    }
                    else
                    {
                        BitWriterUtils.WriteBits(buffer, (int)bitsWritten, bitReader.RunLength, blackValue);
                        bitsWritten += bitReader.RunLength;
                        pixelsWritten += bitReader.RunLength;
                    }
                }

                if (pixelsWritten % this.Width == 0)
                {
                    bitReader.StartNewRow();

                    // Write padding bits, if necessary.
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
