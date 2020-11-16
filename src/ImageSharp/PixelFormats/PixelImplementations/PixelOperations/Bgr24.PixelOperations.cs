// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.Formats;

namespace SixLabors.ImageSharp.PixelFormats
{
    /// <content>
    /// Provides optimized overrides for bulk operations.
    /// </content>
    public partial struct Bgr24
    {
        /// <summary>
        /// Provides optimized overrides for bulk operations.
        /// </summary>
        internal partial class PixelOperations : PixelOperations<Bgr24>
        {
            /// <inheritdoc />
            public override PixelTypeInfo GetPixelTypeInfo()
                => PixelTypeInfo.Create<Bgr24>(PixelAlphaRepresentation.None);
        }
    }
}