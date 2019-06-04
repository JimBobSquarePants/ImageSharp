﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

namespace SixLabors.ImageSharp.Formats.Bmp
{
    /// <summary>
    /// Defines possible options, how skipped pixels during decoding of run length encoded bitmaps should be treated.
    /// </summary>
    public enum RleSkippePixelHandling : int
    {
        /// <summary>
        /// Undefined pixels should be black. This is how System.Drawing handles undefined pixels.
        /// </summary>
        Black = 0,

        /// <summary>
        /// Undefined pixels should be transparent.
        /// </summary>
        Transparent = 1,

        /// <summary>
        /// Undefined pixels should have the first color of the palette.
        /// </summary>
        FirstColorOfPalette = 2
    }
}
