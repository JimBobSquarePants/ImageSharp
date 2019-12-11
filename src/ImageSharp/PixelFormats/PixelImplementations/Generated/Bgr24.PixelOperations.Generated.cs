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
    public partial struct Bgr24
    {
        /// <summary>
        /// Provides optimized overrides for bulk operations.
        /// </summary>
        internal class PixelOperations : PixelOperations<Bgr24>
        {
            /// <inheritdoc />
            internal override void FromBgr24(Configuration configuration, ReadOnlySpan<Bgr24> source, Span<Bgr24> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(source, destPixels, nameof(destPixels));

                source.CopyTo(destPixels);
            }

            /// <inheritdoc />
            internal override void ToBgr24(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<Bgr24> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                sourcePixels.CopyTo(destPixels);
            }

            /// <inheritdoc />
            internal override void FromVector4Destructive(Configuration configuration, Span<Vector4> sourceVectors, Span<Bgr24> destPixels, PixelConversionModifiers modifiers)
            {
                Vector4Converters.RgbaCompatible.FromVector4(configuration, this, sourceVectors, destPixels, modifiers.Remove(PixelConversionModifiers.Scale | PixelConversionModifiers.Premultiply));
            }

            /// <inheritdoc />
            internal override void ToVector4(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<Vector4> destVectors, PixelConversionModifiers modifiers)
            {
                Vector4Converters.RgbaCompatible.ToVector4(configuration, this, sourcePixels, destVectors, modifiers.Remove(PixelConversionModifiers.Scale | PixelConversionModifiers.Premultiply));
            }

            /// <inheritdoc />
            internal override void ToArgb32(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<Argb32> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgr24 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Argb32 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgr24 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Argb32 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgr24(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToBgra32(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<Bgra32> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgr24 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Bgra32 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgr24 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Bgra32 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgr24(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToL8(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<L8> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgr24 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref L8 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgr24 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref L8 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgr24(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToL16(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<L16> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgr24 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref L16 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgr24 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref L16 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgr24(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToLa16(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<La16> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgr24 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref La16 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgr24 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref La16 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgr24(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToLa32(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<La32> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgr24 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref La32 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgr24 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref La32 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgr24(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToRgb24(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<Rgb24> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgr24 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Rgb24 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgr24 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Rgb24 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgr24(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToRgba32(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<Rgba32> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgr24 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Rgba32 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgr24 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Rgba32 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgr24(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToRgb48(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<Rgb48> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgr24 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Rgb48 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgr24 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Rgb48 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgr24(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToRgba64(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<Rgba64> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgr24 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Rgba64 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgr24 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Rgba64 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgr24(sp);
                }
            }

            /// <inheritdoc />
            internal override void ToBgra5551(Configuration configuration, ReadOnlySpan<Bgr24> sourcePixels, Span<Bgra5551> destPixels)
            {
                Guard.NotNull(configuration, nameof(configuration));
                Guard.DestinationShouldNotBeTooShort(sourcePixels, destPixels, nameof(destPixels));

                ref Bgr24 sourceRef = ref MemoryMarshal.GetReference(sourcePixels);
                ref Bgra5551 destRef = ref MemoryMarshal.GetReference(destPixels);

                for (int i = 0; i < sourcePixels.Length; i++)
                {
                    ref Bgr24 sp = ref Unsafe.Add(ref sourceRef, i);
                    ref Bgra5551 dp = ref Unsafe.Add(ref destRef, i);

                    dp.FromBgr24(sp);
                }
            }
            /// <inheritdoc />
            internal override void From<TSourcePixel>(
                Configuration configuration,
                ReadOnlySpan<TSourcePixel> sourcePixels,
                Span<Bgr24> destinationPixels)
            {
                PixelOperations<TSourcePixel>.Instance.ToBgr24(configuration, sourcePixels, destinationPixels);
            }

        }
    }
}
