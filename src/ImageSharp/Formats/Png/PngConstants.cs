﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Text;

namespace SixLabors.ImageSharp.Formats.Png
{
    /// <summary>
    /// Defines png constants defined in the specification.
    /// </summary>
    internal static class PngConstants
    {
        /// <summary>
        /// The default encoding for text metadata.
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.ASCII;

        /// <summary>
        /// The list of mimetypes that equate to a png.
        /// </summary>
        public static readonly IEnumerable<string> MimeTypes = new[] { "image/png" };

        /// <summary>
        /// The list of file extensions that equate to a png.
        /// </summary>
        public static readonly IEnumerable<string> FileExtensions = new[] { "png" };
    }
}