﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace SixLabors.ImageSharp.Primitives
{
    /// <summary>
    /// A vector with 4 values of type <see cref="Complex64"/>.
    /// </summary>
    internal struct ComplexVector4
    {
        /// <summary>
        /// The real part of the complex vector
        /// </summary>
        public Vector4 Real;

        /// <summary>
        /// The imaginary part of the complex number
        /// </summary>
        public Vector4 Imaginary;

        /// <summary>
        /// Sums the values in the input <see cref="ComplexVector4"/> to the current instance
        /// </summary>
        /// <param name="value">The input <see cref="ComplexVector4"/> to sum</param>
        [MethodImpl(InliningOptions.ShortMethod)]
        public void Sum(in ComplexVector4 value)
        {
            this.Real += value.Real;
            this.Imaginary += value.Imaginary;
        }

        /// <summary>
        /// Performs a weighted sum on the current instance according to the given parameters
        /// </summary>
        /// <param name="a">The 'a' parameter, for the real component</param>
        /// <param name="b">The 'b' parameter, for the imaginary component</param>
        /// <returns>The resulting <see cref="Vector4"/> value</returns>
        [MethodImpl(InliningOptions.ShortMethod)]
        public Vector4 WeightedSum(float a, float b) => (this.Real * a) + (this.Imaginary * b);
    }
}
