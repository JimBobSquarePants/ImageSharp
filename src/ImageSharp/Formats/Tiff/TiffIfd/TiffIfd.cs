﻿// <copyright file="TiffIfd.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Formats
{
    /// <summary>
    /// Data structure for holding details of each TIFF IFD.
    /// </summary>
    internal struct TiffIfd
    {
        /// <summary>
        /// An array of the entries within this IFD.
        /// </summary>
        public TiffIfdEntry[] Entries;

        /// <summary>
        /// Offset (in bytes) to the next IFD, or zero if this is the last IFD.
        /// </summary>
        public uint NextIfdOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffIfd" /> class.
        /// </summary>
        /// <param name="entries">An array of the entries within the IFD.</param>
        /// <param name="nextIfdOffset">Offset (in bytes) to the next IFD, or zero if this is the last IFD.</param>
        public TiffIfd(TiffIfdEntry[] entries, uint nextIfdOffset)
        {
            this.Entries = entries;
            this.NextIfdOffset = nextIfdOffset;
        }

        /// <summary>
        /// Gets the child <see cref="TiffIfdEntry"/> with the specified tag ID.
        /// </summary>
        /// <param name="tag">The tag ID to search for.</param>
        /// <param name="entry">The resulting <see cref="TiffIfdEntry"/>, if it exists.</param>
        /// <returns>A flag indicating whether the requested entry exists</returns>
        public bool TryGetIfdEntry(ushort tag, out TiffIfdEntry entry)
        {
            for (int i = 0; i < this.Entries.Length; i++)
            {
                if (this.Entries[i].Tag == tag)
                {
                    entry = this.Entries[i];
                    return true;
                }
            }

            entry = default(TiffIfdEntry);
            return false;
        }
    }
}
