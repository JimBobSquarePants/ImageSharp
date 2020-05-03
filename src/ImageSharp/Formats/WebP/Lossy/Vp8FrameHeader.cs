// Copyright (c) Six Labors and contributors.
// Licensed under the GNU Affero General Public License, Version 3.

namespace SixLabors.ImageSharp.Formats.WebP.Lossy
{
    /// <summary>
    /// Vp8 frame header information.
    /// </summary>
    internal class Vp8FrameHeader
    {
        /// <summary>
        /// Gets or sets a value indicating whether this is a key frame.
        /// </summary>
        public bool KeyFrame { get; set; }

        /// <summary>
        /// Gets or sets Vp8 profile [0..3].
        /// </summary>
        public sbyte Profile { get; set; }

        /// <summary>
        /// Gets or sets the partition length.
        /// </summary>
        public uint PartitionLength { get; set; }
    }
}
