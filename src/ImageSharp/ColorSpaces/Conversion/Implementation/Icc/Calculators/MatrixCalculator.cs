﻿// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace SixLabors.ImageSharp.ColorSpaces.Conversion.Icc
{
    internal class MatrixCalculator : IVector4Calculator
    {
        private Matrix4x4 matrix2D;
        private Vector4 matrix1D;

        public MatrixCalculator(Matrix4x4 matrix3x3, Vector3 matrix3x1)
        {
            this.matrix2D = matrix3x3;
            this.matrix1D = new Vector4(matrix3x1, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4 Calculate(Vector4 value)
        {
            var transformed = Vector4.Transform(value, this.matrix2D);
            return Vector4.Add(this.matrix1D, transformed);
        }
    }
}
