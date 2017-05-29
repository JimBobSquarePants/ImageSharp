﻿// <copyright file="Size.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Stores an ordered pair of integers, which specify a height and width.
    /// </summary>
    /// <remarks>
    /// This struct is fully mutable. This is done (against the guidelines) for the sake of performance,
    /// as it avoids the need to create new values for modification operations.
    /// </remarks>
    public struct Size : IEquatable<Size>
    {
        /// <summary>
        /// Represents a <see cref="Size"/> that has Width and Height values set to zero.
        /// </summary>
        public static readonly Size Empty = default(Size);

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> struct.
        /// </summary>
        /// <param name="value">The width and height of the size</param>
        public Size(int value)
            : this()
        {
            this.Width = value;
            this.Height = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> struct.
        /// </summary>
        /// <param name="width">The width of the size.</param>
        /// <param name="height">The height of the size.</param>
        public Size(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> struct.
        /// </summary>
        /// <param name="size">The size</param>
        public Size(Size size)
            : this()
        {
            this.Width = size.Width;
            this.Height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> struct from the given <see cref="Point"/>.
        /// </summary>
        /// <param name="point">The point</param>
        public Size(Point point)
        {
            this.Width = point.X;
            this.Height = point.Y;
        }

        /// <summary>
        /// Gets or sets the width of this <see cref="Size"/>.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of this <see cref="Size"/>.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Size"/> is empty.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsEmpty => this.Equals(Empty);

        /// <summary>
        /// Creates a <see cref="SizeF"/> with the dimensions of the specified <see cref="Size"/>.
        /// </summary>
        /// <param name="size">The point</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SizeF(Size size) => new SizeF(size.Width, size.Height);

        /// <summary>
        /// Converts the given <see cref="Size"/> into a <see cref="Point"/>.
        /// </summary>
        /// <param name="size">The size</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point(Size size) => new Point(size.Width, size.Height);

        /// <summary>
        /// Computes the sum of adding two sizes.
        /// </summary>
        /// <param name="left">The size on the left hand of the operand.</param>
        /// <param name="right">The size on the right hand of the operand.</param>
        /// <returns>
        /// The <see cref="Size"/>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size operator +(Size left, Size right) => Add(left, right);

        /// <summary>
        /// Computes the difference left by subtracting one size from another.
        /// </summary>
        /// <param name="left">The size on the left hand of the operand.</param>
        /// <param name="right">The size on the right hand of the operand.</param>
        /// <returns>
        /// The <see cref="Size"/>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size operator -(Size left, Size right) => Subtract(left, right);

        /// <summary>
        /// Compares two <see cref="Size"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Size"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="Size"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Size left, Size right) => left.Equals(right);

        /// <summary>
        /// Compares two <see cref="Size"/> objects for inequality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Size"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="Size"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is unequal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Size left, Size right) => !left.Equals(right);

        /// <summary>
        /// Performs vector addition of two <see cref="Size"/> objects.
        /// </summary>
        /// <param name="left">The size on the left hand of the operand.</param>
        /// <param name="right">The size on the right hand of the operand.</param>
        /// <returns>The <see cref="Size"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size Add(Size left, Size right) => new Size(unchecked(left.Width + right.Width), unchecked(left.Height + right.Height));

        /// <summary>
        /// Contracts a <see cref="Size"/> by another <see cref="Size"/>
        /// </summary>
        /// <param name="left">The size on the left hand of the operand.</param>
        /// <param name="right">The size on the right hand of the operand.</param>
        /// <returns>The <see cref="Size"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size Subtract(Size left, Size right) => new Size(unchecked(left.Width - right.Width), unchecked(left.Height - right.Height));

        /// <summary>
        /// Converts a <see cref="SizeF"/> to a <see cref="Size"/> by performing a ceiling operation on all the dimensions.
        /// </summary>
        /// <param name="size">The size</param>
        /// <returns>The <see cref="Size"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size Ceiling(SizeF size) => new Size(unchecked((int)MathF.Ceiling(size.Width)), unchecked((int)MathF.Ceiling(size.Height)));

        /// <summary>
        /// Converts a <see cref="SizeF"/> to a <see cref="Size"/> by performing a round operation on all the dimensions.
        /// </summary>
        /// <param name="size">The size</param>
        /// <returns>The <see cref="Size"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size Round(SizeF size) => new Size(unchecked((int)MathF.Round(size.Width)), unchecked((int)MathF.Round(size.Height)));

        /// <summary>
        /// Converts a <see cref="SizeF"/> to a <see cref="Size"/> by performing a round operation on all the dimensions.
        /// </summary>
        /// <param name="size">The size</param>
        /// <returns>The <see cref="Size"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size Truncate(SizeF size) => new Size(unchecked((int)size.Width), unchecked((int)size.Height));

        /// <inheritdoc/>
        public override int GetHashCode() => this.GetHashCode(this);

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.IsEmpty)
            {
                return "Size [ Empty ]";
            }

            return $"Size [ Width={this.Width}, Height={this.Height} ]";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Size && this.Equals((Size)obj);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Size other) => this.Width == other.Width && this.Height == other.Height;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <param name="size">
        /// The instance of <see cref="Size"/> to return the hash code for.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        private int GetHashCode(Size size) => size.Width ^ size.Height;
    }
}