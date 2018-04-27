﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SixLabors.ImageSharp.Formats.Png.Filters
{
    /// <summary>
    /// The Average filter uses the average of the two neighboring pixels (left and above) to predict
    /// the value of a pixel.
    /// <see href="https://www.w3.org/TR/PNG-Filters.html"/>
    /// </summary>
    internal static class AverageFilter
    {
        /// <summary>
        /// Decodes the scanline
        /// </summary>
        /// <param name="scanline">The scanline to decode</param>
        /// <param name="previousScanline">The previous scanline.</param>
        /// <param name="bytesPerPixel">The bytes per pixel.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Decode(Span<byte> scanline, Span<byte> previousScanline, int bytesPerPixel)
        {
            DebugGuard.MustBeSameSized(scanline, previousScanline, nameof(scanline));

            ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
            ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);

            // Average(x) + floor((Raw(x-bpp)+Prior(x))/2)
            for (int x = 1; x < scanline.Length; x++)
            {
                if (x - bytesPerPixel < 1)
                {
                    ref byte scan = ref Unsafe.Add(ref scanBaseRef, x);
                    byte above = Unsafe.Add(ref prevBaseRef, x);
                    scan = (byte)((scan + (above >> 1)) % 256);
                }
                else
                {
                    ref byte scan = ref Unsafe.Add(ref scanBaseRef, x);
                    byte left = Unsafe.Add(ref scanBaseRef, x - bytesPerPixel);
                    byte above = Unsafe.Add(ref prevBaseRef, x);
                    scan = (byte)((scan + Average(left, above)) % 256);
                }
            }
        }

        /// <summary>
        /// Encodes the scanline
        /// </summary>
        /// <param name="scanline">The scanline to encode</param>
        /// <param name="previousScanline">The previous scanline.</param>
        /// <param name="result">The filtered scanline result.</param>
        /// <param name="bytesPerPixel">The bytes per pixel.</param>
        /// <param name="sum">The sum of the total variance of the filtered row</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Span<byte> scanline, Span<byte> previousScanline, Span<byte> result, int bytesPerPixel, out int sum)
        {
            DebugGuard.MustBeSameSized(scanline, previousScanline, nameof(scanline));
            DebugGuard.MustBeSizedAtLeast(result, scanline, nameof(result));

            ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
            ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);
            ref byte resultBaseRef = ref MemoryMarshal.GetReference(result);
            sum = 0;

            // Average(x) = Raw(x) - floor((Raw(x-bpp)+Prior(x))/2)
            resultBaseRef = 3;

#if OLD_AND_SLOW
            for (int x = 0; x < scanline.Length; x++)
            {
                if (x - bytesPerPixel < 0)
                {
                    byte scan = Unsafe.Add(ref scanBaseRef, x);
                    byte above = Unsafe.Add(ref prevBaseRef, x);
                    ref byte res = ref Unsafe.Add(ref resultBaseRef, x + 1);
                    res = (byte)((scan - (above >> 1)) % 256);
                    sum += res < 128 ? res : 256 - res;
                }
                else
                {
                    byte scan = Unsafe.Add(ref scanBaseRef, x);
                    byte left = Unsafe.Add(ref scanBaseRef, x - bytesPerPixel);
                    byte above = Unsafe.Add(ref prevBaseRef, x);
                    ref byte res = ref Unsafe.Add(ref resultBaseRef, x + 1);
                    res = (byte)((scan - Average(left, above)) % 256);
                    sum += res < 128 ? res : 256 - res;
                }
            }
#else
            int x = 0;
            for (; x < bytesPerPixel;) {
                byte scan = Unsafe.Add(ref scanBaseRef, x);
                byte above = Unsafe.Add(ref prevBaseRef, x);
                ++x;
                ref byte res = ref Unsafe.Add(ref resultBaseRef, x);
                res = (byte)(scan - (above >> 1));
                sum += ImageMaths.FastAbs(unchecked((sbyte)res));
            }

            for (int xLeft = x - bytesPerPixel; x < scanline.Length; ++xLeft) {
                byte scan = Unsafe.Add(ref scanBaseRef, x);
                byte left = Unsafe.Add(ref scanBaseRef, xLeft);
                byte above = Unsafe.Add(ref prevBaseRef, x);
                ++x;
                ref byte res = ref Unsafe.Add(ref resultBaseRef, x);
                res = (byte)(scan - Average(left, above));
                sum += ImageMaths.FastAbs(unchecked((sbyte)res));
            }
#endif

            sum -= 3;
        }

        /// <summary>
        /// Calculates the average value of two bytes
        /// </summary>
        /// <param name="left">The left byte</param>
        /// <param name="above">The above byte</param>
        /// <returns>The <see cref="int"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Average(byte left, byte above)
        {
            return (left + above) >> 1;
        }
    }
}