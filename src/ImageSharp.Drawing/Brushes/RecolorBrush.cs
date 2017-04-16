﻿// <copyright file="RecolorBrush.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Drawing.Brushes
{
    /// <summary>
    /// Provides an implementation of a recolor brush for painting color changes.
    /// </summary>
    public class RecolorBrush : RecolorBrush<Color32>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecolorBrush" /> class.
        /// </summary>
        /// <param name="sourceColor">Color of the source.</param>
        /// <param name="targetColor">Color of the target.</param>
        /// <param name="threshold">The threshold.</param>
        public RecolorBrush(Color32 sourceColor, Color32 targetColor, float threshold)
            : base(sourceColor, targetColor, threshold)
        {
        }
    }
}
