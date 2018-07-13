﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable CompareOfFloatsByEqualityOperator
namespace SixLabors.ImageSharp.ColorSpaces
{
    /// <summary>
    /// Represents the coordinates of CIEXY chromaticity space.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct CieXyChromaticityCoordinates
        : IEquatable<CieXyChromaticityCoordinates>, IAlmostEquatable<CieXyChromaticityCoordinates, float>
    {
        // NOTE: We don't implement a backing vector on this class to avoid runtime bugs.
        // SEE: https://github.com/dotnet/coreclr/issues/16443

        /// <summary>
        /// Initializes a new instance of the <see cref="CieXyChromaticityCoordinates"/> struct.
        /// </summary>
        /// <param name="x">Chromaticity coordinate x (usually from 0 to 1)</param>
        /// <param name="y">Chromaticity coordinate y (usually from 0 to 1)</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CieXyChromaticityCoordinates(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets the chromaticity X-coordinate.
        /// </summary>
        /// <remarks>
        /// Ranges usually from 0 to 1.
        /// </remarks>
        public float X { get; }

        /// <summary>
        /// Gets the chromaticity Y-coordinate.
        /// </summary>
        /// <remarks>
        /// Ranges usually from 0 to 1.
        /// </remarks>
        public float Y { get; }

        /// <summary>
        /// Compares two <see cref="CieXyChromaticityCoordinates"/> objects for equality.
        /// </summary>
        /// <param name="left">
        /// The <see cref="CieXyChromaticityCoordinates"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="CieXyChromaticityCoordinates"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(CieXyChromaticityCoordinates left, CieXyChromaticityCoordinates right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="CieXyChromaticityCoordinates"/> objects for inequality
        /// </summary>
        /// <param name="left">
        /// The <see cref="CieXyChromaticityCoordinates"/> on the left side of the operand.
        /// </param>
        /// <param name="right">
        /// The <see cref="CieXyChromaticityCoordinates"/> on the right side of the operand.
        /// </param>
        /// <returns>
        /// True if the current left is unequal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(CieXyChromaticityCoordinates left, CieXyChromaticityCoordinates right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashHelpers.Combine(this.X.GetHashCode(), this.Y.GetHashCode());

        /// <inheritdoc/>
        public override string ToString() => $"CieXyChromaticityCoordinates({this.X},{this.Y})";

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is CieXyChromaticityCoordinates other && this.Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CieXyChromaticityCoordinates other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AlmostEquals(CieXyChromaticityCoordinates other, float precision)
        {
            return MathF.Abs(this.X) <= precision
                && Math.Abs(this.Y) <= precision;
        }
    }
}