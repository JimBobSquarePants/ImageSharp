﻿// <copyright file="PointF.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp
{
    using System;
    using System.ComponentModel;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents an ordered pair of single precision floating point x- and y-coordinates that defines a point in
    /// a two-dimensional plane.
    /// </summary>
    /// <remarks>
    /// This struct is fully mutable. This is done (against the guidelines) for the sake of performance,
    /// as it avoids the need to create new values for modification operations.
    /// </remarks>
    public struct PointF : IEquatable<PointF>
    {
        /// <summary>
        /// Represents a <see cref="PointF"/> that has X and Y values set to zero.
        /// </summary>
        public static readonly PointF Empty = default(PointF);

        /// <summary>
        /// Initializes a new instance of the <see cref="PointF"/> struct.
        /// </summary>
        /// <param name="x">The horizontal position of the point.</param>
        /// <param name="y">The vertical position of the point.</param>
        public PointF(float x, float y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointF"/> struct from the given <see cref="SizeF"/>.
        /// </summary>
        /// <param name="size">The size</param>
        public PointF(SizeF size)
        {
            this.X = size.Width;
            this.Y = size.Height;
        }

        /// <summary>
        /// Gets or sets the x-coordinate of this <see cref="PointF"/>.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of this <see cref="PointF"/>.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PointF"/> is empty.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsEmpty => this.Equals(Empty);

        /// <summary>
        /// Creates a <see cref="Vector2"/> with the coordinates of the specified <see cref="PointF"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>
        /// The <see cref="Vector2"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PointF(Vector2 vector)
        {
            return new PointF(vector.X, vector.Y);
        }

        /// <summary>
        /// Creates a <see cref="Vector2"/> with the coordinates of the specified <see cref="PointF"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        /// The <see cref="Vector2"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2(PointF point)
        {
            return new Vector2(point.X, point.Y);
        }

        /// <summary>
        /// Creates a <see cref="Point"/> with the coordinates of the specified <see cref="PointF"/> by truncating each of the coordinates.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        /// The <see cref="Point"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point(PointF point)
        {
            return new Point(unchecked((int)MathF.Round(point.X)), unchecked((int)MathF.Round(point.Y)));
        }

        /// <summary>
        /// Translates a <see cref="PointF"/> by a given <see cref="SizeF"/>.
        /// </summary>
        /// <param name="point">The point on the left hand of the operand.</param>
        /// <param name="size">The size on the right hand of the operand.</param>
        /// <returns>
        /// The <see cref="PointF"/>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF operator +(PointF point, SizeF size)
        {
            return Add(point, size);
        }

        /// <summary>
        /// Translates a <see cref="PointF"/> by the negative of a given <see cref="SizeF"/>.
        /// </summary>
        /// <param name="point">The point on the left hand of the operand.</param>
        /// <param name="size">The size on the right hand of the operand.</param>
        /// <returns>The <see cref="PointF"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF operator -(PointF point, SizeF size)
        {
            return Subtract(point, size);
        }

        /// <summary>
        /// Compares two <see cref="PointF"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="PointF"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="PointF"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PointF left, PointF right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="PointF"/> objects for inequality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="PointF"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="PointF"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is unequal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PointF left, PointF right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Translates a <see cref="PointF"/> by the negative of a given <see cref="SizeF"/>.
        /// </summary>
        /// <param name="point">The point on the left hand of the operand.</param>
        /// <param name="size">The size on the right hand of the operand.</param>
        /// <returns>The <see cref="PointF"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF Add(PointF point, SizeF size)
        {
            return new PointF(point.X + size.Width, point.Y + size.Height);
        }

        /// <summary>
        /// Translates a <see cref="PointF"/> by the negative of a given <see cref="SizeF"/>.
        /// </summary>
        /// <param name="point">The point on the left hand of the operand.</param>
        /// <param name="size">The size on the right hand of the operand.</param>
        /// <returns>The <see cref="PointF"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF Subtract(PointF point, SizeF size)
        {
            return new PointF(point.X - size.Width, point.Y - size.Height);
        }

        /// <summary>
        /// Rotates a point around the given rotation matrix.
        /// </summary>
        /// <param name="point">The point to rotate</param>
        /// <param name="rotation">Rotation matrix used</param>
        /// <returns>The rotated <see cref="PointF"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF Rotate(PointF point, Matrix3x2 rotation)
        {
            return Vector2.Transform(new Vector2(point.X, point.Y), rotation);
        }

        /// <summary>
        /// Skews a point using the given skew matrix.
        /// </summary>
        /// <param name="point">The point to rotate</param>
        /// <param name="skew">Rotation matrix used</param>
        /// <returns>The rotated <see cref="PointF"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF Skew(PointF point, Matrix3x2 skew)
        {
            return Vector2.Transform(new Vector2(point.X, point.Y), skew);
        }

        /// <summary>
        /// Translates this <see cref="PointF"/> by the specified amount.
        /// </summary>
        /// <param name="dx">The amount to offset the x-coordinate.</param>
        /// <param name="dy">The amount to offset the y-coordinate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(float dx, float dy)
        {
            this.X += dx;
            this.Y += dy;
        }

        /// <summary>
        /// Translates this <see cref="PointF"/> by the specified amount.
        /// </summary>
        /// <param name="point">The <see cref="PointF"/> used offset this <see cref="PointF"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(PointF point)
        {
            this.Offset(point.X, point.Y);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.GetHashCode(this);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.IsEmpty)
            {
                return "PointF [ Empty ]";
            }

            return $"PointF [ X={this.X}, Y={this.Y} ]";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is PointF)
            {
                return this.Equals((PointF)obj);
            }

            return false;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(PointF other)
        {
            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <param name="point">
        /// The instance of <see cref="PointF"/> to return the hash code for.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        private int GetHashCode(PointF point)
        {
            unchecked
            {
                return point.X.GetHashCode() ^ point.Y.GetHashCode();
            }
        }
    }
}