﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SixLabors.ImageSharp.PixelFormats
{
    /// <summary>
    /// Packed pixel type containing four 16-bit unsigned normalized values ranging from 0 to 635535.
    /// <para>
    /// Ranges from [0, 0, 0, 0] to [1, 1, 1, 1] in vector form.
    /// </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Rgba64 : IPixel<Rgba64>, IPackedVector<ulong>
    {
        private const float Max = ushort.MaxValue;

        /// <summary>
        /// Gets or sets the red component.
        /// </summary>
        public ushort R;

        /// <summary>
        /// Gets or sets the green component.
        /// </summary>
        public ushort G;

        /// <summary>
        /// Gets or sets the blue component.
        /// </summary>
        public ushort B;

        /// <summary>
        /// Gets or sets the alpha component.
        /// </summary>
        public ushort A;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rgba64"/> struct.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        public Rgba64(ushort r, ushort g, ushort b, ushort a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        /// <summary>
        /// Gets or sets the RGB components of this struct as <see cref="Rgb48"/>
        /// </summary>
        public Rgb48 Rgb
        {
            [MethodImpl(InliningOptions.ShortMethod)]
            get => Unsafe.As<Rgba64, Rgb48>(ref this);

            [MethodImpl(InliningOptions.ShortMethod)]
            set => Unsafe.As<Rgba64, Rgb48>(ref this) = value;
        }

        /// <inheritdoc/>
        public ulong PackedValue
        {
            [MethodImpl(InliningOptions.ShortMethod)]
            get => Unsafe.As<Rgba64, ulong>(ref this);

            [MethodImpl(InliningOptions.ShortMethod)]
            set => Unsafe.As<Rgba64, ulong>(ref this) = value;
        }

        /// <summary>
        /// Compares two <see cref="Rgba64"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="Rgba64"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="Rgba64"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static bool operator ==(Rgba64 left, Rgba64 right) => left.PackedValue == right.PackedValue;

        /// <summary>
        /// Compares two <see cref="Rgba64"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="Rgba64"/> on the left side of the operand.</param>
        /// <param name="right">The <see cref="Rgba64"/> on the right side of the operand.</param>
        /// <returns>
        /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public static bool operator !=(Rgba64 left, Rgba64 right) => left.PackedValue != right.PackedValue;

        /// <inheritdoc />
        public PixelOperations<Rgba64> CreatePixelOperations() => new PixelOperations();

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromScaledVector4(Vector4 vector) => this.FromVector4(vector);

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public Vector4 ToScaledVector4() => this.ToVector4();

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromVector4(Vector4 vector)
        {
            vector = Vector4.Clamp(vector, Vector4.Zero, Vector4.One) * Max;
            this.R = (ushort)MathF.Round(vector.X);
            this.G = (ushort)MathF.Round(vector.Y);
            this.B = (ushort)MathF.Round(vector.Z);
            this.A = (ushort)MathF.Round(vector.W);
        }

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public Vector4 ToVector4() => new Vector4(this.R, this.G, this.B, this.A) / Max;

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromArgb32(Argb32 source)
        {
            this.R = ImageMaths.UpscaleFrom8BitTo16Bit(source.R);
            this.G = ImageMaths.UpscaleFrom8BitTo16Bit(source.G);
            this.B = ImageMaths.UpscaleFrom8BitTo16Bit(source.B);
            this.A = ImageMaths.UpscaleFrom8BitTo16Bit(source.A);
        }

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromBgr24(Bgr24 source)
        {
            this.R = ImageMaths.UpscaleFrom8BitTo16Bit(source.R);
            this.G = ImageMaths.UpscaleFrom8BitTo16Bit(source.G);
            this.B = ImageMaths.UpscaleFrom8BitTo16Bit(source.B);
            this.A = ushort.MaxValue;
        }

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromBgra32(Bgra32 source)
        {
            this.R = ImageMaths.UpscaleFrom8BitTo16Bit(source.R);
            this.G = ImageMaths.UpscaleFrom8BitTo16Bit(source.G);
            this.B = ImageMaths.UpscaleFrom8BitTo16Bit(source.B);
            this.A = ImageMaths.UpscaleFrom8BitTo16Bit(source.A);
        }

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromBgra5551(Bgra5551 source) => this.FromScaledVector4(source.ToScaledVector4());

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromGray8(Gray8 source)
        {
            ushort rgb = ImageMaths.UpscaleFrom8BitTo16Bit(source.PackedValue);
            this.R = rgb;
            this.G = rgb;
            this.B = rgb;
            this.A = ushort.MaxValue;
        }

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromGray16(Gray16 source)
        {
            this.R = source.PackedValue;
            this.G = source.PackedValue;
            this.B = source.PackedValue;
            this.A = ushort.MaxValue;
        }

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromRgb24(Rgb24 source)
        {
            this.R = ImageMaths.UpscaleFrom8BitTo16Bit(source.R);
            this.G = ImageMaths.UpscaleFrom8BitTo16Bit(source.G);
            this.B = ImageMaths.UpscaleFrom8BitTo16Bit(source.B);
            this.A = ushort.MaxValue;
        }

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromRgba32(Rgba32 source)
        {
            this.R = ImageMaths.UpscaleFrom8BitTo16Bit(source.R);
            this.G = ImageMaths.UpscaleFrom8BitTo16Bit(source.G);
            this.B = ImageMaths.UpscaleFrom8BitTo16Bit(source.B);
            this.A = ImageMaths.UpscaleFrom8BitTo16Bit(source.A);
        }

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public void ToRgba32(ref Rgba32 dest)
        {
            dest.R = ImageMaths.DownScaleFrom16BitTo8Bit(this.R);
            dest.G = ImageMaths.DownScaleFrom16BitTo8Bit(this.G);
            dest.B = ImageMaths.DownScaleFrom16BitTo8Bit(this.B);
            dest.A = ImageMaths.DownScaleFrom16BitTo8Bit(this.A);
        }

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromRgb48(Rgb48 source)
        {
            this.Rgb = source;
            this.A = ushort.MaxValue;
        }

        /// <inheritdoc/>
        [MethodImpl(InliningOptions.ShortMethod)]
        public void FromRgba64(Rgba64 source) => this = source;

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Rgba64 rgba64 && this.Equals(rgba64);

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public bool Equals(Rgba64 other) => this.PackedValue.Equals(other.PackedValue);

        /// <inheritdoc />
        public override string ToString() => $"Rgba64({this.R}, {this.G}, {this.B}, {this.A})";

        /// <inheritdoc />
        [MethodImpl(InliningOptions.ShortMethod)]
        public override int GetHashCode() => this.PackedValue.GetHashCode();
    }
}