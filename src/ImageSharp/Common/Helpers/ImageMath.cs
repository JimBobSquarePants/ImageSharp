// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp
{
    /// <summary>
    /// Provides common mathematical methods used for image processing.
    /// </summary>
    internal static class ImageMath
    {
        /// <summary>
        /// Vector for converting pixel to gray value as specified by ITU-R Recommendation BT.709.
        /// </summary>
        private static readonly Vector4 Bt709 = new Vector4(.2126f, .7152f, .0722f, 0.0f);

        /// <summary>
        /// Convert a pixel value to grayscale using ITU-R Recommendation BT.709.
        /// </summary>
        /// <param name="vector">The vector to get the luminance from.</param>
        /// <param name="luminanceLevels">The number of luminance levels (256 for 8 bit, 65536 for 16 bit grayscale images)</param>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static int GetBT709Luminance(ref Vector4 vector, int luminanceLevels)
            => (int)MathF.Round(Vector4.Dot(vector, Bt709) * (luminanceLevels - 1));

        /// <summary>
        /// Gets the luminance from the rgb components using the formula as specified by ITU-R Recommendation BT.709.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <returns>The <see cref="byte"/>.</returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static byte Get8BitBT709Luminance(byte r, byte g, byte b) =>
            (byte)((r * .2126F) + (g * .7152F) + (b * .0722F) + 0.5F);

        /// <summary>
        /// Gets the luminance from the rgb components using the formula as specified by ITU-R Recommendation BT.709.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <returns>The <see cref="ushort"/>.</returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static ushort Get16BitBT709Luminance(ushort r, ushort g, ushort b) =>
            (ushort)((r * .2126F) + (g * .7152F) + (b * .0722F) + 0.5F);

        /// <summary>
        /// Gets the luminance from the rgb components using the formula as specified by ITU-R Recommendation BT.709.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <returns>The <see cref="ushort"/>.</returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static ushort Get16BitBT709Luminance(float r, float g, float b) =>
            (ushort)((r * .2126F) + (g * .7152F) + (b * .0722F) + 0.5F);

        /// <summary>
        /// Scales a value from a 16 bit <see cref="ushort"/> to it's 8 bit <see cref="byte"/> equivalent.
        /// </summary>
        /// <param name="component">The 8 bit component value.</param>
        /// <returns>The <see cref="byte"/></returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static byte DownScaleFrom16BitTo8Bit(ushort component)
        {
            // To scale to 8 bits From a 16-bit value V the required value (from the PNG specification) is:
            //
            //    (V * 255) / 65535
            //
            // This reduces to round(V / 257), or floor((V + 128.5)/257)
            //
            // Represent V as the two byte value vhi.vlo.  Make a guess that the
            // result is the top byte of V, vhi, then the correction to this value
            // is:
            //
            //    error = floor(((V-vhi.vhi) + 128.5) / 257)
            //          = floor(((vlo-vhi) + 128.5) / 257)
            //
            // This can be approximated using integer arithmetic (and a signed
            // shift):
            //
            //    error = (vlo-vhi+128) >> 8;
            //
            // The approximate differs from the exact answer only when (vlo-vhi) is
            // 128; it then gives a correction of +1 when the exact correction is
            // 0.  This gives 128 errors.  The exact answer (correct for all 16-bit
            // input values) is:
            //
            //    error = (vlo-vhi+128)*65535 >> 24;
            //
            // An alternative arithmetic calculation which also gives no errors is:
            //
            //    (V * 255 + 32895) >> 16
            return (byte)(((component * 255) + 32895) >> 16);
        }

        /// <summary>
        /// Scales a value from an 8 bit <see cref="byte"/> to it's 16 bit <see cref="ushort"/> equivalent.
        /// </summary>
        /// <param name="component">The 8 bit component value.</param>
        /// <returns>The <see cref="ushort"/></returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static ushort UpscaleFrom8BitTo16Bit(byte component) => (ushort)(component * 257);

        /// <summary>
        /// Returns how many bits are required to store the specified number of colors.
        /// Performs a Log2() on the value.
        /// </summary>
        /// <param name="colors">The number of colors.</param>
        /// <returns>
        /// The <see cref="int"/>
        /// </returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static int GetBitsNeededForColorDepth(int colors) => Math.Max(1, (int)Math.Ceiling(Math.Log(colors, 2)));

        /// <summary>
        /// Returns how many colors will be created by the specified number of bits.
        /// </summary>
        /// <param name="bitDepth">The bit depth.</param>
        /// <returns>The <see cref="int"/></returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static int GetColorCountForBitDepth(int bitDepth) => 1 << bitDepth;

        /// <summary>
        /// Gets the bounding <see cref="Rectangle"/> from the given points.
        /// </summary>
        /// <param name="topLeft">
        /// The <see cref="Point"/> designating the top left position.
        /// </param>
        /// <param name="bottomRight">
        /// The <see cref="Point"/> designating the bottom right position.
        /// </param>
        /// <returns>
        /// The bounding <see cref="Rectangle"/>.
        /// </returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static Rectangle GetBoundingRectangle(Point topLeft, Point bottomRight) => new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);

        /// <summary>
        /// Finds the bounding rectangle based on the first instance of any color component other
        /// than the given one.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="bitmap">The <see cref="Image{TPixel}"/> to search within.</param>
        /// <param name="componentValue">The color component value to remove.</param>
        /// <param name="channel">The <see cref="RgbaComponent"/> channel to test against.</param>
        /// <returns>
        /// The <see cref="Rectangle"/>.
        /// </returns>
        public static Rectangle GetFilteredBoundingRectangle<TPixel>(ImageFrame<TPixel> bitmap, float componentValue, RgbaComponent channel = RgbaComponent.B)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            Point topLeft = default;
            Point bottomRight = default;

            Func<ImageFrame<TPixel>, int, int, float, bool> delegateFunc;

            // Determine which channel to check against
            switch (channel)
            {
                case RgbaComponent.R:
                    delegateFunc = (pixels, x, y, b) => MathF.Abs(pixels[x, y].ToVector4().X - b) > Constants.Epsilon;
                    break;

                case RgbaComponent.G:
                    delegateFunc = (pixels, x, y, b) => MathF.Abs(pixels[x, y].ToVector4().Y - b) > Constants.Epsilon;
                    break;

                case RgbaComponent.B:
                    delegateFunc = (pixels, x, y, b) => MathF.Abs(pixels[x, y].ToVector4().Z - b) > Constants.Epsilon;
                    break;

                default:
                    delegateFunc = (pixels, x, y, b) => MathF.Abs(pixels[x, y].ToVector4().W - b) > Constants.Epsilon;
                    break;
            }

            int GetMinY(ImageFrame<TPixel> pixels)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (delegateFunc(pixels, x, y, componentValue))
                        {
                            return y;
                        }
                    }
                }

                return 0;
            }

            int GetMaxY(ImageFrame<TPixel> pixels)
            {
                for (int y = height - 1; y > -1; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (delegateFunc(pixels, x, y, componentValue))
                        {
                            return y;
                        }
                    }
                }

                return height;
            }

            int GetMinX(ImageFrame<TPixel> pixels)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (delegateFunc(pixels, x, y, componentValue))
                        {
                            return x;
                        }
                    }
                }

                return 0;
            }

            int GetMaxX(ImageFrame<TPixel> pixels)
            {
                for (int x = width - 1; x > -1; x--)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (delegateFunc(pixels, x, y, componentValue))
                        {
                            return x;
                        }
                    }
                }

                return width;
            }

            topLeft.Y = GetMinY(bitmap);
            topLeft.X = GetMinX(bitmap);
            bottomRight.Y = Numerics.Clamp(GetMaxY(bitmap) + 1, 0, height);
            bottomRight.X = Numerics.Clamp(GetMaxX(bitmap) + 1, 0, width);

            return GetBoundingRectangle(topLeft, bottomRight);
        }
    }
}
