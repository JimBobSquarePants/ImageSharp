﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using SixLabors.ImageSharp.Formats.Tiff;

namespace SixLabors.ImageSharp.Formats
{
    /// <summary>
    /// Encapsulates the means to encode and decode Tiff images.
    /// </summary>
    public class TiffFormat : IImageFormat
    {
        /// <inheritdoc/>
        public string Name => "TIFF";

        /// <inheritdoc/>
        public string DefaultMimeType => "image/tiff";

        /// <inheritdoc/>
        public IEnumerable<string> MimeTypes => TiffConstants.MimeTypes;

        /// <inheritdoc/>
        public IEnumerable<string> FileExtensions => TiffConstants.FileExtensions;
    }
}
