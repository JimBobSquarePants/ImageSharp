// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Compression.Zlib;
using SixLabors.ImageSharp.Formats.Tiff.Compression;
using SixLabors.ImageSharp.Formats.Tiff.Constants;
using SixLabors.ImageSharp.Formats.Tiff.Writers;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

namespace SixLabors.ImageSharp.Formats.Tiff
{
    /// <summary>
    /// Performs the TIFF encoding operation.
    /// </summary>
    internal sealed class TiffEncoderCore : IImageEncoderInternals
    {
        private static readonly ushort ByteOrderMarker = BitConverter.IsLittleEndian
                ? TiffConstants.ByteOrderLittleEndianShort
                : TiffConstants.ByteOrderBigEndianShort;

        /// <summary>
        /// Used for allocating memory during processing operations.
        /// </summary>
        private readonly MemoryAllocator memoryAllocator;

        /// <summary>
        /// A scratch buffer to reduce allocations.
        /// </summary>
        private readonly byte[] buffer = new byte[4];

        /// <summary>
        /// The global configuration.
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// The quantizer for creating color palette image.
        /// </summary>
        private readonly IQuantizer quantizer;

        /// <summary>
        /// Sets the deflate compression level.
        /// </summary>
        private readonly DeflateCompressionLevel compressionLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffEncoderCore"/> class.
        /// </summary>
        /// <param name="options">The options for the encoder.</param>
        /// <param name="memoryAllocator">The memory allocator.</param>
        public TiffEncoderCore(ITiffEncoderOptions options, MemoryAllocator memoryAllocator)
        {
            this.memoryAllocator = memoryAllocator;
            this.PhotometricInterpretation = options.PhotometricInterpretation;
            this.quantizer = options.Quantizer ?? KnownQuantizers.Octree;
            this.BitsPerPixel = options.BitsPerPixel;
            this.HorizontalPredictor = options.HorizontalPredictor;
            this.CompressionType = options.Compression;
            this.compressionLevel = options.CompressionLevel ?? DeflateCompressionLevel.DefaultCompression;
        }

        /// <summary>
        /// Gets the photometric interpretation implementation to use when encoding the image.
        /// </summary>
        internal TiffPhotometricInterpretation? PhotometricInterpretation { get; private set; }

        /// <summary>
        /// Gets or sets the compression implementation to use when encoding the image.
        /// </summary>
        internal TiffCompression? CompressionType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating which horizontal predictor to use. This can improve the compression ratio with deflate compression.
        /// </summary>
        internal TiffPredictor? HorizontalPredictor { get; set; }

        /// <summary>
        /// Gets the bits per pixel.
        /// </summary>
        internal TiffBitsPerPixel? BitsPerPixel { get; private set; }

        /// <summary>
        /// Encodes the image to the specified stream from the <see cref="Image{TPixel}"/>.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="image">The <see cref="Image{TPixel}"/> to encode from.</param>
        /// <param name="stream">The <see cref="Stream"/> to encode the image data to.</param>
        /// <param name="cancellationToken">The token to request cancellation.</param>
        public void Encode<TPixel>(Image<TPixel> image, Stream stream, CancellationToken cancellationToken)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            Guard.NotNull(image, nameof(image));
            Guard.NotNull(stream, nameof(stream));

            this.configuration = image.GetConfiguration();

            TiffPhotometricInterpretation rootFramePhotometricInterpretation = GetRootFramePhotometricInterpretation(image);
            TiffPhotometricInterpretation photometricInterpretation = this.PhotometricInterpretation == TiffPhotometricInterpretation.PaletteColor
                ? TiffPhotometricInterpretation.PaletteColor
                : rootFramePhotometricInterpretation;

            ImageFrameMetadata rootFrameMetaData = image.Frames.RootFrame.Metadata;
            ExifProfile rootFrameExifProfile = image.Frames.RootFrame.Metadata.ExifProfile;
            TiffBitsPerPixel? rootFrameBitsPerPixel = rootFrameMetaData.GetTiffMetadata().BitsPerPixel;

            // If the user has not chosen a predictor or compression, set the values from the decoded image, if present.
            if (!this.HorizontalPredictor.HasValue && rootFrameExifProfile?.GetValue(ExifTag.Predictor) != null)
            {
                this.HorizontalPredictor = (TiffPredictor)rootFrameExifProfile?.GetValue(ExifTag.Predictor).Value;
            }

            if (!this.CompressionType.HasValue && rootFrameExifProfile?.GetValue(ExifTag.Compression) != null)
            {
                this.CompressionType = (TiffCompression)rootFrameExifProfile?.GetValue(ExifTag.Compression).Value;
            }

            this.SetBitsPerPixel(rootFrameBitsPerPixel, image.PixelType.BitsPerPixel, photometricInterpretation);
            this.SetPhotometricInterpretation(photometricInterpretation);

            using (var writer = new TiffStreamWriter(stream))
            {
                long firstIfdMarker = this.WriteHeader(writer);

                // TODO: multiframing is not supported
                this.WriteImage(writer, image, firstIfdMarker);
            }
        }

        /// <summary>
        /// Writes the TIFF file header.
        /// </summary>
        /// <param name="writer">The <see cref="TiffStreamWriter" /> to write data to.</param>
        /// <returns>
        /// The marker to write the first IFD offset.
        /// </returns>
        public long WriteHeader(TiffStreamWriter writer)
        {
            writer.Write(ByteOrderMarker);
            writer.Write(TiffConstants.HeaderMagicNumber);
            return writer.PlaceMarker();
        }

        /// <summary>
        /// Writes all data required to define an image.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="writer">The <see cref="BinaryWriter"/> to write data to.</param>
        /// <param name="image">The <see cref="Image{TPixel}"/> to encode from.</param>
        /// <param name="ifdOffset">The marker to write this IFD offset.</param>
        private void WriteImage<TPixel>(TiffStreamWriter writer, Image<TPixel> image, long ifdOffset)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var entriesCollector = new TiffEncoderEntriesCollector();

            using TiffBaseCompressor compressor = TiffCompressorFactory.Create(
                this.CompressionType ?? TiffCompression.None,
                writer.BaseStream,
                this.memoryAllocator,
                image.Width,
                (int)this.BitsPerPixel,
                this.compressionLevel,
                this.HorizontalPredictor == TiffPredictor.Horizontal ? this.HorizontalPredictor.Value : TiffPredictor.None);

            using TiffBaseColorWriter<TPixel> colorWriter = TiffColorWriterFactory.Create(
                this.PhotometricInterpretation,
                image.Frames.RootFrame,
                this.quantizer,
                this.memoryAllocator,
                this.configuration,
                entriesCollector,
                (int)this.BitsPerPixel);

            int rowsPerStrip = this.CalcRowsPerStrip(image.Frames.RootFrame.Height, colorWriter.BytesPerRow);

            colorWriter.Write(compressor, rowsPerStrip);

            entriesCollector.ProcessImageFormat(this);
            entriesCollector.ProcessGeneral(image);

            writer.WriteMarker(ifdOffset, (uint)writer.Position);
            long nextIfdMarker = this.WriteIfd(writer, entriesCollector.Entries);
        }

        /// <summary>
        /// Calculates the number of rows written per strip.
        /// </summary>
        /// <param name="height">The height of the image.</param>
        /// <param name="bytesPerRow">The number of bytes per row.</param>
        /// <returns>Number of rows per strip.</returns>
        private int CalcRowsPerStrip(int height, int bytesPerRow)
        {
            DebugGuard.MustBeGreaterThan(height, 0, nameof(height));
            DebugGuard.MustBeGreaterThan(bytesPerRow, 0, nameof(bytesPerRow));

            int rowsPerStrip = TiffConstants.DefaultStripSize / bytesPerRow;

            if (rowsPerStrip > 0)
            {
                if (rowsPerStrip < height)
                {
                    return rowsPerStrip;
                }

                return height;
            }

            return 1;
        }

        /// <summary>
        /// Writes a TIFF IFD block.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> to write data to.</param>
        /// <param name="entries">The IFD entries to write to the file.</param>
        /// <returns>The marker to write the next IFD offset (if present).</returns>
        private long WriteIfd(TiffStreamWriter writer, List<IExifValue> entries)
        {
            if (entries.Count == 0)
            {
                TiffThrowHelper.ThrowArgumentException("There must be at least one entry per IFD.");
            }

            uint dataOffset = (uint)writer.Position + (uint)(6 + (entries.Count * 12));
            var largeDataBlocks = new List<byte[]>();

            entries.Sort((a, b) => (ushort)a.Tag - (ushort)b.Tag);

            writer.Write((ushort)entries.Count);

            foreach (IExifValue entry in entries)
            {
                writer.Write((ushort)entry.Tag);
                writer.Write((ushort)entry.DataType);
                writer.Write(ExifWriter.GetNumberOfComponents(entry));

                uint length = ExifWriter.GetLength(entry);
                if (length <= 4)
                {
                    int sz = ExifWriter.WriteValue(entry, this.buffer, 0);
                    DebugGuard.IsTrue(sz == length, "Incorrect number of bytes written");
                    writer.WritePadded(this.buffer.AsSpan(0, sz));
                }
                else
                {
                    var raw = new byte[length];
                    int sz = ExifWriter.WriteValue(entry, raw, 0);
                    DebugGuard.IsTrue(sz == raw.Length, "Incorrect number of bytes written");
                    largeDataBlocks.Add(raw);
                    writer.Write(dataOffset);
                    dataOffset += (uint)(raw.Length + (raw.Length % 2));
                }
            }

            long nextIfdMarker = writer.PlaceMarker();

            foreach (byte[] dataBlock in largeDataBlocks)
            {
                writer.Write(dataBlock);

                if (dataBlock.Length % 2 == 1)
                {
                    writer.Write(0);
                }
            }

            return nextIfdMarker;
        }

        private void SetPhotometricInterpretation(TiffPhotometricInterpretation? photometricInterpretation)
        {
            // Make sure, that the fax compressions are only used together with the WhiteIsZero.
            if (this.CompressionType == TiffCompression.CcittGroup3Fax || this.CompressionType == TiffCompression.Ccitt1D)
            {
                // The user has not specified a preferred photometric interpretation.
                if (this.PhotometricInterpretation == null)
                {
                    this.PhotometricInterpretation = TiffPhotometricInterpretation.WhiteIsZero;
                    this.BitsPerPixel = TiffBitsPerPixel.Bit1;
                    return;
                }

                if (this.PhotometricInterpretation != TiffPhotometricInterpretation.WhiteIsZero && this.PhotometricInterpretation != TiffPhotometricInterpretation.BlackIsZero)
                {
                    TiffThrowHelper.ThrowImageFormatException(
                        $"The {this.CompressionType} compression and {this.PhotometricInterpretation} aren't compatible. Please use {this.CompressionType} only with {TiffPhotometricInterpretation.BlackIsZero} or {TiffPhotometricInterpretation.WhiteIsZero}.");
                }
                else
                {
                    // The “normal” PhotometricInterpretation for bilevel CCITT compressed data is WhiteIsZero.
                    this.PhotometricInterpretation = TiffPhotometricInterpretation.WhiteIsZero;
                }

                return;
            }

            switch (this.PhotometricInterpretation)
            {
                // The currently supported values by the encoder for photometric interpretation:
                case TiffPhotometricInterpretation.PaletteColor:
                case TiffPhotometricInterpretation.BlackIsZero:
                case TiffPhotometricInterpretation.WhiteIsZero:
                    break;

                default:
                    this.PhotometricInterpretation = TiffPhotometricInterpretation.Rgb;
                    break;
            }

            // Use the bits per pixel to determine the photometric interpretation.
            this.SetPhotometricInterpretationWithBitsPerPixel(this.BitsPerPixel, photometricInterpretation);
        }

        private void SetPhotometricInterpretationWithBitsPerPixel(TiffBitsPerPixel? bitsPerPixel, TiffPhotometricInterpretation? photometricInterpretation)
        {
            switch (bitsPerPixel)
            {
                case TiffBitsPerPixel.Bit1:
                    this.PhotometricInterpretation = TiffPhotometricInterpretation.BlackIsZero;
                    break;
                case TiffBitsPerPixel.Bit4:
                    this.PhotometricInterpretation = TiffPhotometricInterpretation.PaletteColor;
                    break;
                case TiffBitsPerPixel.Bit8:
                    this.PhotometricInterpretation = photometricInterpretation == TiffPhotometricInterpretation.PaletteColor
                        ? TiffPhotometricInterpretation.PaletteColor
                        : TiffPhotometricInterpretation.BlackIsZero;

                    break;
                default:
                    this.PhotometricInterpretation = TiffPhotometricInterpretation.Rgb;
                    break;
            }
        }

        private void SetBitsPerPixel(TiffBitsPerPixel? rootFrameBitsPerPixel, int inputBitsPerPixel, TiffPhotometricInterpretation photometricInterpretation)
        {
            this.BitsPerPixel ??= rootFrameBitsPerPixel;

            if (photometricInterpretation == TiffPhotometricInterpretation.PaletteColor)
            {
                if (this.BitsPerPixel != TiffBitsPerPixel.Bit8 && this.BitsPerPixel != TiffBitsPerPixel.Bit4)
                {
                    this.BitsPerPixel = TiffBitsPerPixel.Bit8;
                }

                return;
            }

            if (this.BitsPerPixel.HasValue)
            {
                return;
            }

            if (this.PhotometricInterpretation == null && inputBitsPerPixel == 8)
            {
                this.BitsPerPixel = TiffBitsPerPixel.Bit8;
                return;
            }

            switch (this.PhotometricInterpretation)
            {
                case TiffPhotometricInterpretation.BlackIsZero:
                case TiffPhotometricInterpretation.WhiteIsZero:
                    if (this.CompressionType == TiffCompression.Ccitt1D ||
                        this.CompressionType == TiffCompression.CcittGroup3Fax ||
                        this.CompressionType == TiffCompression.CcittGroup4Fax)
                    {
                        this.BitsPerPixel = TiffBitsPerPixel.Bit1;
                    }
                    else
                    {
                        this.BitsPerPixel = TiffBitsPerPixel.Bit8;
                    }

                    break;
                case TiffPhotometricInterpretation.PaletteColor:
                    if (this.BitsPerPixel != TiffBitsPerPixel.Bit8 && this.BitsPerPixel != TiffBitsPerPixel.Bit4)
                    {
                        this.BitsPerPixel = TiffBitsPerPixel.Bit8;
                    }

                    break;
                case TiffPhotometricInterpretation.Rgb:
                    this.BitsPerPixel = TiffBitsPerPixel.Bit24;
                    break;
                default:
                    this.PhotometricInterpretation = TiffPhotometricInterpretation.Rgb;
                    this.BitsPerPixel = TiffBitsPerPixel.Bit24;
                    break;
            }
        }

        private static TiffPhotometricInterpretation GetRootFramePhotometricInterpretation(Image image)
        {
            ExifProfile exifProfile = image.Frames.RootFrame.Metadata.ExifProfile;
            return exifProfile?.GetValue(ExifTag.PhotometricInterpretation) != null
                ? (TiffPhotometricInterpretation)exifProfile?.GetValue(ExifTag.PhotometricInterpretation).Value
                : TiffPhotometricInterpretation.BlackIsZero;
        }
    }
}