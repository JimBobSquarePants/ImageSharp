﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tint.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Tints an image with the given color.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Processors
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    /// <summary>
    /// Tints an image with the given color.
    /// </summary>
    public class Tint : IGraphicsProcessor
    {
        /// <summary>
        /// Gets or sets DynamicParameter.
        /// </summary>
        public dynamic DynamicParameter { get; set; }

        /// <summary>
        /// Gets or sets any additional settings required by the processor.
        /// </summary>
        public Dictionary<string, string> Settings { get; set; }

        /// <summary>
        /// Processes the image.
        /// </summary>
        /// <param name="factory">
        /// The current instance of the <see cref="T:ImageProcessor.ImageFactory"/> class containing
        /// the image to process.
        /// </param>
        /// <returns>
        /// The processed image from the current instance of the <see cref="T:ImageProcessor.ImageFactory"/> class.
        /// </returns>
        public Image ProcessImage(ImageFactory factory)
        {
            Bitmap newImage = null;
            Image image = factory.Image;

            try
            {
                Color tintColour = this.DynamicParameter;
                float[][] colorMatrixElements =
                    {
                        new[] { tintColour.R / 255f, 0, 0, 0, 0 }, // Red 
                        new[] { 0, tintColour.G / 255f, 0, 0, 0 }, // Green 
                        new[] { 0, 0, tintColour.B / 255f, 0, 0 }, // Blue  
                        new[] { 0, 0, 0, tintColour.A / 255f, 0 }, // Alpha 
                        new float[] { 0, 0, 0, 0, 1 }
                    };

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
                newImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppPArgb);

                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;

                    using (ImageAttributes attributes = new ImageAttributes())
                    {
                        attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

                        image.Dispose();
                        image = newImage;
                    }
                }

                return image;
            }
            catch
            {
                if (newImage != null)
                {
                    newImage.Dispose();
                }
            }

            return image;
        }
    }
}