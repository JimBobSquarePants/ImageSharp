﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.ColorSpaces.Conversion.Implementation.RgbColorSapce;

namespace SixLabors.ImageSharp.ColorSpaces
{
    /// <summary>
    /// Represents an linear Rgb color with specified <see cref="RgbWorkingSpace"/> working space
    /// </summary>
    internal readonly struct LinearRgb : IEquatable<LinearRgb>
    {
        /// <summary>
        /// The default LinearRgb working space.
        /// </summary>
        public static readonly RgbWorkingSpace DefaultWorkingSpace = RgbWorkingSpaces.SRgb;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearRgb"/> struct.
        /// </summary>
        /// <param name="r">The red component ranging between 0 and 1.</param>
        /// <param name="g">The green component ranging between 0 and 1.</param>
        /// <param name="b">The blue component ranging between 0 and 1.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LinearRgb(float r, float g, float b)
            : this(new Vector3(r, g, b))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearRgb"/> struct.
        /// </summary>
        /// <param name="r">The red component ranging between 0 and 1.</param>
        /// <param name="g">The green component ranging between 0 and 1.</param>
        /// <param name="b">The blue component ranging between 0 and 1.</param>
        /// <param name="workingSpace">The rgb working space.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LinearRgb(float r, float g, float b, RgbWorkingSpace workingSpace)
            : this(new Vector3(r, g, b), workingSpace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearRgb"/> struct.
        /// </summary>
        /// <param name="vector">The vector representing the r, g, b components.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LinearRgb(Vector3 vector)
            : this(vector, DefaultWorkingSpace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearRgb"/> struct.
        /// </summary>
        /// <param name="vector">The vector representing the r, g, b components.</param>
        /// <param name="workingSpace">The LinearRgb working space.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LinearRgb(Vector3 vector, RgbWorkingSpace workingSpace)
            : this()
        {
            // Clamp to 0-1 range.
            vector = Vector3.Clamp(vector, Vector3.Zero, Vector3.One);

            this.R = vector.X;
            this.G = vector.Y;
            this.B = vector.Z;
            this.WorkingSpace = workingSpace;
        }

        /// <summary>
        /// Gets the red component.
        /// <remarks>A value usually ranging between 0 and 1.</remarks>
        /// </summary>
        public float R { get; }

        /// <summary>
        /// Gets the green component.
        /// <remarks>A value usually ranging between 0 and 1.</remarks>
        /// </summary>
        public float G { get; }

        /// <summary>
        /// Gets the blue component.
        /// <remarks>A value usually ranging between 0 and 1.</remarks>
        /// </summary>
        public float B { get; }

        /// <summary>
        /// Gets the LinearRgb color space <seealso cref="RgbWorkingSpaces"/>
        /// </summary>
        public RgbWorkingSpace WorkingSpace { get; }

        /// <summary>
        /// Returns a new <see cref="Vector3"/> representing this instance.
        /// </summary>
        /// <returns>The <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 ToVector3() => new Vector3(this.R, this.G, this.B);

        /// <summary>
        /// Compares two <see cref="LinearRgb"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="LinearRgb"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="LinearRgb"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(LinearRgb left, LinearRgb right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="LinearRgb"/> objects for inequality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="LinearRgb"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="LinearRgb"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is unequal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(LinearRgb left, LinearRgb right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => (this.R, this.G, this.B).GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => $"LinearRgb({this.R:#0.##},{this.G:#0.##},{this.B:#0.##})";

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is LinearRgb other && this.Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(LinearRgb other) =>
            this.R.Equals(other.R) &&
            this.G.Equals(other.G) &&
            this.B.Equals(other.B);
    }
}