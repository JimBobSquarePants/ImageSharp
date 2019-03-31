﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

// <auto-generated />

using SixLabors.ImageSharp.PixelFormats.Utils;
using System;
using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace SixLabors.ImageSharp.PixelFormats
{
    /// <content>
    /// Provides optimized overrides for bulk operations.
    /// </content>
    public partial struct Bgra32
    {
        /// <summary>
        /// Provides optimized overrides for bulk operations.
        /// </summary>
        internal class PixelOperations : PixelOperations<Bgra32>
        {
            /// <inheritdoc />
            internal override void FromBgra32(Configuration configuration, ReadOnlySpan<Bgra32> source, Span<Bgra32> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(source, destPixels, nameof(destPixels));

                source.CopyTo(destPixels);
            }

            /// <inheritdoc />
            internal override void ToBgra32(Configuration configuration, ReadOnlySpan<Bgra32> sourcePixels, Span<Bgra32> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                sourcePixels.CopyTo(destPixels);
            }

            /// <inheritdoc />
            internal override void FromVector4(Configuration configuration, Span<Vector4> sourceVectors, Span<Bgra32> destPixels, PixelConversionModifiers modifiers)
            {
                Vector4Converters.RgbaCompatible.FromVector4(configuration, this, sourceVectors, destPixels, modifiers.Remove(PixelConversionModifiers.Scale));
            }

            /// <inheritdoc />
            internal override void ToVector4(Configuration configuration, ReadOnlySpan<Bgra32> sourcePixels, Span<Vector4> destVectors, PixelConversionModifiers modifiers)
            {
                Vector4Converters.RgbaCompatible.ToVector4(configuration, this, sourcePixels, destVectors, modifiers.Remove(PixelConversionModifiers.Scale));
            }
            /// <inheritdoc />
            internal override void ToRgba32(Configuration configuration, ReadOnlySpan<Bgra32> sourcePixels, Span<Rgba32> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref uint sourceRef = ref Unsafe.As<Bgra32,uint>(ref MemoryMarshal.GetReference(sourcePixels));
                ref uint destRef = ref Unsafe.As<Rgba32, uint>(ref MemoryMarshal.GetReference(destPixels));

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    uint sp = Unsafe.Add(ref sourceRef, i);
                    Unsafe.Add(ref destRef, i) = PixelConverter.FromBgra32.ToRgba32(sp);
                }
            }

            /// <inheritdoc />
            internal override void FromRgba32(Configuration configuration, ReadOnlySpan<Rgba32> sourcePixels, Span<Bgra32> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref uint sourceRef = ref Unsafe.As<Rgba32,uint>(ref MemoryMarshal.GetReference(sourcePixels));
                ref uint destRef = ref Unsafe.As<Bgra32, uint>(ref MemoryMarshal.GetReference(destPixels));

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    uint sp = Unsafe.Add(ref sourceRef, i);
                    Unsafe.Add(ref destRef, i) = PixelConverter.FromRgba32.ToBgra32(sp);
                }
            }
            /// <inheritdoc />
            internal override void ToArgb32(Configuration configuration, ReadOnlySpan<Bgra32> sourcePixels, Span<Argb32> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref uint sourceRef = ref Unsafe.As<Bgra32,uint>(ref MemoryMarshal.GetReference(sourcePixels));
                ref uint destRef = ref Unsafe.As<Argb32, uint>(ref MemoryMarshal.GetReference(destPixels));

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    uint sp = Unsafe.Add(ref sourceRef, i);
                    Unsafe.Add(ref destRef, i) = PixelConverter.FromBgra32.ToArgb32(sp);
                }
            }

            /// <inheritdoc />
            internal override void FromArgb32(Configuration configuration, ReadOnlySpan<Argb32> sourcePixels, Span<Bgra32> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref uint sourceRef = ref Unsafe.As<Argb32,uint>(ref MemoryMarshal.GetReference(sourcePixels));
                ref uint destRef = ref Unsafe.As<Bgra32, uint>(ref MemoryMarshal.GetReference(destPixels));

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    uint sp = Unsafe.Add(ref sourceRef, i);
                    Unsafe.Add(ref destRef, i) = PixelConverter.FromArgb32.ToBgra32(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToBgr24(Configuration configuration, ReadOnlySpan<Bgra32> sourcePixels, Span<Bgr24> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgra32 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Bgr24 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgra32 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Bgr24 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgra32(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToGray8(Configuration configuration, ReadOnlySpan<Bgra32> sourcePixels, Span<Gray8> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgra32 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Gray8 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgra32 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Gray8 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgra32(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToGray16(Configuration configuration, ReadOnlySpan<Bgra32> sourcePixels, Span<Gray16> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgra32 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Gray16 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgra32 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Gray16 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgra32(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToRgb24(Configuration configuration, ReadOnlySpan<Bgra32> sourcePixels, Span<Rgb24> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgra32 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Rgb24 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgra32 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Rgb24 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgra32(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToRgb48(Configuration configuration, ReadOnlySpan<Bgra32> sourcePixels, Span<Rgb48> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgra32 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Rgb48 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgra32 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Rgb48 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgra32(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToRgba64(Configuration configuration, ReadOnlySpan<Bgra32> sourcePixels, Span<Rgba64> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgra32 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Rgba64 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgra32 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Rgba64 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgra32(sp);
                }
            }
        }
    }
}