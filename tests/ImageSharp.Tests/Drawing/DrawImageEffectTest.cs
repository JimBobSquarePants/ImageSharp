﻿// <copyright file="DrawImageEffectTest.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests
{
    using System.IO;
    using ImageSharp.PixelFormats;
    using Xunit;

    public class DrawImageEffectTest : FileTestBase
    {
        [Fact]
        public void ImageShouldApplyDrawImageFilter()
        {
            string path = this.CreateOutputDirectory("Drawing", "DrawImageEffect");

            PixelTransformMode[] modes = (PixelTransformMode[])System.Enum.GetValues(typeof(PixelTransformMode));

            using (Image blend = TestFile.Create(TestImages.Png.Blur).CreateImage())
            {
                foreach (TestFile file in Files)
                {
                    using (Image image = file.CreateImage())
                    {
                        foreach (PixelTransformMode mode in modes)
                        {
                            using (FileStream output = File.OpenWrite($"{path}/{mode}.{file.FileName}"))
                            {
                                Size size = new Size(image.Width / 2, image.Height / 2);
                                Point loc = new Point(image.Width / 4, image.Height / 4);

                                image.DrawImage(blend, mode, 75, size, loc).Save(output);
                            }
                        }
                    }
                }
            }
        }
    }
}