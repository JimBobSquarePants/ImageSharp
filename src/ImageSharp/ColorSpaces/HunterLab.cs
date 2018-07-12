﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SixLabors.ImageSharp.ColorSpaces
{
    /// <summary>
    /// Represents an Hunter LAB color.
    /// <see href="https://en.wikipedia.org/wiki/Lab_color_space"/>
    /// </summary>
    internal readonly struct HunterLab : IColorVector, IEquatable<HunterLab>, IAlmostEquatable<HunterLab, float>
    {
        /// <summary>
        /// D50 standard illuminant.
        /// Used when reference white is not specified explicitly.
        /// </summary>
        public static readonly CieXyz DefaultWhitePoint = Illuminants.C;

        /// <summary>
        /// The backing vector for SIMD support.
        /// </summary>
        private readonly Vector3 backingVector;

        /// <summary>
        /// Initializes a new instance of the <see cref="HunterLab"/> struct.
        /// </summary>
        /// <param name="l">The lightness dimension.</param>
        /// <param name="a">The a (green - magenta) component.</param>
        /// <param name="b">The b (blue - yellow) component.</param>
        /// <remarks>Uses <see cref="DefaultWhitePoint"/> as white point.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HunterLab(float l, float a, float b)
            : this(new Vector3(l, a, b), DefaultWhitePoint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HunterLab"/> struct.
        /// </summary>
        /// <param name="l">The lightness dimension.</param>
        /// <param name="a">The a (green - magenta) component.</param>
        /// <param name="b">The b (blue - yellow) component.</param>
        /// <param name="whitePoint">The reference white point. <see cref="Illuminants"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HunterLab(float l, float a, float b, CieXyz whitePoint)
            : this(new Vector3(l, a, b), whitePoint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HunterLab"/> struct.
        /// </summary>
        /// <param name="vector">The vector representing the l, a, b components.</param>
        /// <remarks>Uses <see cref="DefaultWhitePoint"/> as white point.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HunterLab(Vector3 vector)
            : this(vector, DefaultWhitePoint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HunterLab"/> struct.
        /// </summary>
        /// <param name="vector">The vector representing the l a b components.</param>
        /// <param name="whitePoint">The reference white point. <see cref="Illuminants"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HunterLab(Vector3 vector, CieXyz whitePoint)
            : this()
        {
            this.backingVector = vector;
            this.WhitePoint = whitePoint;
        }

        /// <summary>
        /// Gets the reference white point of this color
        /// </summary>
        public CieXyz WhitePoint { get; }

        /// <summary>
        /// Gets the lightness dimension.
        /// <remarks>A value ranging between 0 (black), 100 (diffuse white) or higher (specular white).</remarks>
        /// </summary>
        public float L
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.backingVector.X;
        }

        /// <summary>
        /// Gets the a color component.
        /// <remarks>A value ranging from -100 to 100. Negative is green, positive magenta.</remarks>
        /// </summary>
        public float A
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.backingVector.Y;
        }

        /// <summary>
        /// Gets the b color component.
        /// <remarks>A value ranging from -100 to 100. Negative is blue, positive is yellow</remarks>
        /// </summary>
        public float B
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.backingVector.Z;
        }

        /// <inheritdoc />
        public Vector3 Vector => this.backingVector;

        /// <summary>
        /// Compares two <see cref="HunterLab"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="HunterLab"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="HunterLab"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        public static bool operator ==(HunterLab left, HunterLab right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="HunterLab"/> objects for inequality
        /// </summary>
        /// <param name="left">
        /// The <see cref="HunterLab"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="HunterLab"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is unequal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(HunterLab left, HunterLab right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashHelpers.Combine(this.WhitePoint.GetHashCode(), this.backingVector.GetHashCode());
        }

        /// <inheritdoc/>
        public override string ToString() => $"HunterLab({this.L:#0.##},{this.A:#0.##},{this.B:#0.##})";

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is HunterLab other && this.Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(HunterLab other) =>
            this.backingVector.Equals(other.backingVector) &&
            this.WhitePoint.Equals(other.WhitePoint);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AlmostEquals(HunterLab other, float precision)
        {
            var result = Vector3.Abs(this.backingVector - other.backingVector);

            return this.WhitePoint.Equals(other.WhitePoint)
                   && result.X <= precision
                   && result.Y <= precision
                   && result.Z <= precision;
        }
    }
}