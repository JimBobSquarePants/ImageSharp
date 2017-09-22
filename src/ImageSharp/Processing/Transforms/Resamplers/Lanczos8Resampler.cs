﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

namespace SixLabors.ImageSharp.Processing
{
    /// <summary>
    /// The function implements the Lanczos kernel algorithm as described on
    /// <seealso href="https://en.wikipedia.org/wiki/Lanczos_resampling#Algorithm">Wikipedia</seealso>
    /// with a radius of 8 pixels.
    /// </summary>
    public class Lanczos8Resampler : IResampler
    {
        /// <inheritdoc/>
        public float Radius => 8;

        /// <inheritdoc/>
        public float GetValue(float x)
        {
            if (x < 0F)
            {
                x = -x;
            }

            if (x < 8F)
            {
                return MathF.SinC(x) * MathF.SinC(x / 8F);
            }

            return 0F;
        }
    }
}
