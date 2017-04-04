// <copyright file="TiffSubfileType.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Formats.Tiff
{
    /// <summary>
    /// Enumeration representing the sub-file types defined by the Tiff file-format.
    /// </summary>
    internal enum TiffSubfileType
    {
        /// <summary>
        /// Full-resolution image data.
        /// </summary>
        FullImage = 1,

        /// <summary>
        /// Reduced-resolution image data.
        /// </summary>
        Preview = 2,

        /// <summary>
        /// A single page of a multi-page image.
        /// </summary>
        SinglePage = 3
    }
}