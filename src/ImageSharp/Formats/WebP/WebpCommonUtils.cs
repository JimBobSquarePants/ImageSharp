// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp.PixelFormats;
#if SUPPORTS_RUNTIME_INTRINSICS
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

namespace SixLabors.ImageSharp.Formats.Webp
{
    /// <summary>
    /// Utility methods for lossy and lossless webp format.
    /// </summary>
    internal static class WebpCommonUtils
    {
        /// <summary>
        /// Returns 31 ^ clz(n) = log2(n).Returns 31 ^ clz(n) = log2(n).
        /// </summary>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static int BitsLog2Floor(uint n)
        {
            int logValue = 0;
            while (n >= 256)
            {
                logValue += 8;
                n >>= 8;
            }

            return logValue + Unsafe.Add(ref MemoryMarshal.GetReference(WebpLookupTables.LogTable8Bit), (int)n);
        }

        /// <summary>
        /// Checks if the pixel row is not opaque.
        /// </summary>
        /// <param name="row">The row to check.</param>
        /// <returns>Returns true if alpha has non-0xff values.</returns>
        public static unsafe bool CheckNonOpaque(Span<Bgra32> row)
        {
#if SUPPORTS_RUNTIME_INTRINSICS
            if (Avx2.IsSupported)
            {
                ReadOnlySpan<byte> rowBytes = MemoryMarshal.AsBytes(row);
                var alphaMaskVector256 = Vector256.Create(0, 0, 0, 255, 0, 0, 0, 255, 0, 0, 0, 255, 0, 0, 0, 255, 0, 0, 0, 255, 0, 0, 0, 255, 0, 0, 0, 255, 0, 0, 0, 255);
                Vector256<byte> all0x80Vector256 = Vector256.Create((byte)0x80).AsByte();
                var alphaMask = Vector128.Create(0, 0, 0, 255, 0, 0, 0, 255, 0, 0, 0, 255, 0, 0, 0, 255);
                Vector128<byte> all0x80 = Vector128.Create((byte)0x80).AsByte();

                int i = 0;
                int length = (row.Length * 4) - 3;
                fixed (byte* src = rowBytes)
                {
                    for (; i + 128 <= length; i += 128)
                    {
                        Vector256<byte> a0 = Avx.LoadVector256(src + i).AsByte();
                        Vector256<byte> a1 = Avx.LoadVector256(src + i + 32).AsByte();
                        Vector256<byte> a2 = Avx.LoadVector256(src + i + 64).AsByte();
                        Vector256<byte> a3 = Avx.LoadVector256(src + i + 96).AsByte();
                        Vector256<int> b0 = Avx2.And(a0, alphaMaskVector256).AsInt32();
                        Vector256<int> b1 = Avx2.And(a1, alphaMaskVector256).AsInt32();
                        Vector256<int> b2 = Avx2.And(a2, alphaMaskVector256).AsInt32();
                        Vector256<int> b3 = Avx2.And(a3, alphaMaskVector256).AsInt32();
                        Vector256<short> c0 = Avx2.PackSignedSaturate(b0, b1).AsInt16();
                        Vector256<short> c1 = Avx2.PackSignedSaturate(b2, b3).AsInt16();
                        Vector256<byte> d = Avx2.PackSignedSaturate(c0, c1).AsByte();
                        Vector256<byte> bits = Avx2.CompareEqual(d, all0x80Vector256);
                        int mask = Avx2.MoveMask(bits);
                        if (mask != -1)
                        {
                            return true;
                        }
                    }

                    for (; i + 64 <= length; i += 64)
                    {
                        if (IsNoneOpaque64Bytes(src, i, alphaMask, all0x80))
                        {
                            return true;
                        }
                    }

                    for (; i + 32 <= length; i += 32)
                    {
                        if (IsNoneOpaque32Bytes(src, i, alphaMask, all0x80))
                        {
                            return true;
                        }
                    }

                    for (; i <= length; i += 4)
                    {
                        if (src[i + 3] != 0xFF)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (Sse2.IsSupported)
            {
                ReadOnlySpan<byte> rowBytes = MemoryMarshal.AsBytes(row);
                var alphaMask = Vector128.Create(0, 0, 0, 255, 0, 0, 0, 255, 0, 0, 0, 255, 0, 0, 0, 255);
                Vector128<byte> all0x80 = Vector128.Create((byte)0x80).AsByte();

                int i = 0;
                int length = (row.Length * 4) - 3;
                fixed (byte* src = rowBytes)
                {
                    for (; i + 64 <= length; i += 64)
                    {
                        if (IsNoneOpaque64Bytes(src, i, alphaMask, all0x80))
                        {
                            return true;
                        }
                    }

                    for (; i + 32 <= length; i += 32)
                    {
                        if (IsNoneOpaque32Bytes(src, i, alphaMask, all0x80))
                        {
                            return true;
                        }
                    }

                    for (; i <= length; i += 4)
                    {
                        if (src[i + 3] != 0xFF)
                        {
                            return true;
                        }
                    }
                }
            }
            else
#endif
            {
                for (int x = 0; x < row.Length; x++)
                {
                    if (row[x].A != 0xFF)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

#if SUPPORTS_RUNTIME_INTRINSICS
        private static unsafe bool IsNoneOpaque64Bytes(byte* src, int i, Vector128<byte> alphaMask, Vector128<byte> all0x80)
        {
            Vector128<byte> a0 = Sse2.LoadVector128(src + i).AsByte();
            Vector128<byte> a1 = Sse2.LoadVector128(src + i + 16).AsByte();
            Vector128<byte> a2 = Sse2.LoadVector128(src + i + 32).AsByte();
            Vector128<byte> a3 = Sse2.LoadVector128(src + i + 48).AsByte();
            Vector128<int> b0 = Sse2.And(a0, alphaMask).AsInt32();
            Vector128<int> b1 = Sse2.And(a1, alphaMask).AsInt32();
            Vector128<int> b2 = Sse2.And(a2, alphaMask).AsInt32();
            Vector128<int> b3 = Sse2.And(a3, alphaMask).AsInt32();
            Vector128<short> c0 = Sse2.PackSignedSaturate(b0, b1).AsInt16();
            Vector128<short> c1 = Sse2.PackSignedSaturate(b2, b3).AsInt16();
            Vector128<byte> d = Sse2.PackSignedSaturate(c0, c1).AsByte();
            Vector128<byte> bits = Sse2.CompareEqual(d, all0x80);
            int mask = Sse2.MoveMask(bits);
            if (mask != 0xFFFF)
            {
                return true;
            }

            return false;
        }

        private static unsafe bool IsNoneOpaque32Bytes(byte* src, int i, Vector128<byte> alphaMask, Vector128<byte> all0x80)
        {
            Vector128<byte> a0 = Sse2.LoadVector128(src + i).AsByte();
            Vector128<byte> a1 = Sse2.LoadVector128(src + i + 16).AsByte();
            Vector128<int> b0 = Sse2.And(a0, alphaMask).AsInt32();
            Vector128<int> b1 = Sse2.And(a1, alphaMask).AsInt32();
            Vector128<short> c = Sse2.PackSignedSaturate(b0, b1).AsInt16();
            Vector128<byte> d = Sse2.PackSignedSaturate(c, c).AsByte();
            Vector128<byte> bits = Sse2.CompareEqual(d, all0x80);
            int mask = Sse2.MoveMask(bits);
            if (mask != 0xFFFF)
            {
                return true;
            }

            return false;
        }
#endif
    }
}
