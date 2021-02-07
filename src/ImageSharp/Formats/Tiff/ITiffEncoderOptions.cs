// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.Compression.Zlib;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

namespace SixLabors.ImageSharp.Formats.Experimental.Tiff
{
    /// <summary>
    /// Encapsulates the options for the <see cref="TiffEncoder"/>.
    /// </summary>
    internal interface ITiffEncoderOptions
    {
        /// <summary>
        /// Gets the compression type to use.
        /// </summary>
        TiffEncoderCompression Compression { get; }

        /// <summary>
        /// Gets the compression level 1-9 for the deflate compression mode.
        /// <remarks>Defaults to <see cref="DeflateCompressionLevel.DefaultCompression"/>.</remarks>
        /// </summary>
        DeflateCompressionLevel CompressionLevel { get; }

        /// <summary>
        /// Gets the encoding mode to use. RGB, RGB with color palette or gray.
        /// If no mode is specified in the options, RGB will be used.
        /// </summary>
        TiffEncodingMode Mode { get; }

        /// <summary>
        /// Gets a value indicating whether to use horizontal prediction. This can improve the compression ratio with deflate or lzw compression.
        /// </summary>
        bool UseHorizontalPredictor { get; }

        /// <summary>
        /// Gets the quantizer for creating a color palette image.
        /// </summary>
        IQuantizer Quantizer { get; }

        /// <summary>
        /// Gets the pixel storage method.
        /// </summary>
        TiffEncoderPixelStorageMethod PixelStorageMethod { get; }

        /// <summary>
        /// Gets the maximum size of strip (bytes).
        /// </summary>
        int MaxStripBytes { get; }
    }
}
