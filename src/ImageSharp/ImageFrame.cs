﻿// <copyright file="ImageFrame.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp
{
    using System.Diagnostics;

    /// <summary>
    /// An optimized frame for the <see cref="Image"/> class.
    /// </summary>
    [DebuggerDisplay("ImageFrame: {Width}x{Height}")]
    public class ImageFrame : ImageFrame<Color>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageFrame"/> class.
        /// </summary>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <param name="bootstrapper">
        /// The bootstrapper providing initialization code which allows extending the library.
        /// </param>
        public ImageFrame(int width, int height, Bootstrapper bootstrapper = null)
            : base(width, height, bootstrapper)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageFrame"/> class.
        /// </summary>
        /// <param name="image">
        /// The image to create the frame from.
        /// </param>
        public ImageFrame(ImageBase<Color> image)
            : base(image)
        {
        }

        /// <inheritdoc />
        public override PixelAccessor<Color> Lock()
        {
            return new PixelAccessor(this);
        }

        /// <inheritdoc />
        internal override ImageFrame<Color> Clone()
        {
            return new ImageFrame(this);
        }
    }
}
