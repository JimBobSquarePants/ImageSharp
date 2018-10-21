﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SixLabors.ImageSharp.PixelFormats
{
    /// <summary>
    /// Packed pixel type containing four 16-bit signed normalized values, ranging from −1 to 1.
    /// <para>
    /// Ranges from [-1, -1, -1, -1] to [1, 1, 1, 1] in vector form.
    /// </para>
    /// </summary>
    public struct NormalizedShort4 : IPixel<NormalizedShort4>, IPackedVector<ulong>
    {
        private static readonly Vector4 Max = new Vector4(0x7FFF);
        private static readonly Vector4 Min = Vector4.Negate(Max);

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedShort4"/> struct.
        /// </summary>
        /// <param name="x">The x-component.</param>
        /// <param name="y">The y-component.</param>
        /// <param name="z">The z-component.</param>
        /// <param name="w">The w-component.</param>
        public NormalizedShort4(float x, float y, float z, float w)
            : this(new Vector4(x, y, z, w))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedShort4"/> struct.
        /// </summary>
        /// <param name="vector">The vector containing the component values.</param>
        public NormalizedShort4(Vector4 vector) => this.PackedValue = Pack(ref vector);

        /// <inheritdoc/>
        public ulong PackedValue { get; set; }

        /// <summary>
        /// Compares two <see cref="NormalizedShort4"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="NormalizedShort4"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="NormalizedShort4"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static bool operator ==(NormalizedShort4 left, NormalizedShort4 right) => left.Equals(right);

        /// <summary>
        /// Compares two <see cref="NormalizedShort4"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="NormalizedShort4"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="NormalizedShort4"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static bool operator !=(NormalizedShort4 left, NormalizedShort4 right) => !left.Equals(right);

        /// <inheritdoc />
        public PixelOperations<NormalizedShort4> CreatePixelOperations() => new PixelOperations<NormalizedShort4>();

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public void PackFromScaledVector4(Vector4 vector)
        {
            vector *= 2F;
            vector -= Vector4.One;
            this.PackFromVector4(vector);
        }

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public Vector4 ToScaledVector4()
        {
            var scaled = this.ToVector4();
            scaled += Vector4.One;
            scaled /= 2F;
            return scaled;
        }

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void PackFromVector4(Vector4 vector) => this.PackedValue = Pack(ref vector);

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public Vector4 ToVector4()
        {
            const float MaxVal = 0x7FFF;

            return new Vector4(
                         (short)((this.PackedValue >> 0x00) & 0xFFFF) / MaxVal,
                         (short)((this.PackedValue >> 0x10) & 0xFFFF) / MaxVal,
                         (short)((this.PackedValue >> 0x20) & 0xFFFF) / MaxVal,
                         (short)((this.PackedValue >> 0x30) & 0xFFFF) / MaxVal);
        }

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void PackFromArgb32(Argb32 source) => this.PackFromScaledVector4(source.ToScaledVector4());

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void PackFromBgr24(Bgr24 source) => this.PackFromScaledVector4(source.ToScaledVector4());

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void PackFromBgra32(Bgra32 source) => this.PackFromScaledVector4(source.ToScaledVector4());

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public void PackFromGray8(Gray8 source) => this.PackFromScaledVector4(source.ToScaledVector4());

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public void PackFromGray16(Gray16 source) => this.PackFromScaledVector4(source.ToScaledVector4());

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void PackFromRgb24(Rgb24 source) => this.PackFromScaledVector4(source.ToScaledVector4());

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void PackFromRgba32(Rgba32 source) => this.PackFromScaledVector4(source.ToScaledVector4());

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void ToRgba32(ref Rgba32 dest)
        {
            dest.PackFromScaledVector4(this.ToScaledVector4());
        }

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public void PackFromRgb48(Rgb48 source) => this.PackFromScaledVector4(source.ToScaledVector4());

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public void PackFromRgba64(Rgba64 source) => this.PackFromScaledVector4(source.ToScaledVector4());

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is NormalizedShort4 other && this.Equals(other);

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public bool Equals(NormalizedShort4 other) => this.PackedValue.Equals(other.PackedValue);

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public override int GetHashCode() => this.PackedValue.GetHashCode();

        /// <inheritdoc />
        public override string ToString()
        {
            var vector = this.ToVector4();
            return FormattableString.Invariant($"NormalizedShort4({vector.X:#0.##}, {vector.Y:#0.##}, {vector.Z:#0.##}, {vector.W:#0.##})");
        }

        [MethodImpl(InliningOptions.ShortMethod)]
        private static ulong Pack(ref Vector4 vector)
        {
            vector *= Max;
            vector = Vector4.Clamp(vector, Min, Max);

            // Round rather than truncate.
            ulong word4 = ((ulong)MathF.Round(vector.X) & 0xFFFF) << 0x00;
            ulong word3 = ((ulong)MathF.Round(vector.Y) & 0xFFFF) << 0x10;
            ulong word2 = ((ulong)MathF.Round(vector.Z) & 0xFFFF) << 0x20;
            ulong word1 = ((ulong)MathF.Round(vector.W) & 0xFFFF) << 0x30;

            return word4 | word3 | word2 | word1;
        }
    }
}