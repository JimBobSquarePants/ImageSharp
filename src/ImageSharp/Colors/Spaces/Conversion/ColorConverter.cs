﻿// <copyright file="ColorConverter.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Colors.Conversion
{
    using System.Numerics;
    using Implementation;
    using Spaces;

    /// <summary>
    /// Converts between color spaces ensuring that the color is adapted using chromatic adaptation.
    /// </summary>
    public partial class ColorConverter
    {
        private Matrix4x4 transformationMatrix;
        private CieXyzAndLmsConverter cachedCieXyzAndLmsConverter;

        /// <summary>
        /// The default whitepoint used for converting to CieLab
        /// </summary>
        public static readonly CieXyz DefaultWhitePoint = Illuminants.D65;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorConverter"/> class.
        /// </summary>
        public ColorConverter()
        {
            // Note the order here this is important.
            this.WhitePoint = DefaultWhitePoint;
            this.LmsAdaptationMatrix = CieXyzAndLmsConverter.DefaultTransformationMatrix;
            this.ChromaticAdaptation = new VonKriesChromaticAdaptation(this.cachedCieXyzAndLmsConverter, this.cachedCieXyzAndLmsConverter);
            this.TargetLabWhitePoint = CieLab.DefaultWhitePoint;
        }

        /// <summary>
        /// Gets or sets the white point used for chromatic adaptation in conversions from/to XYZ color space.
        /// When null, no adaptation will be performed.
        /// </summary>
        public CieXyz WhitePoint { get; set; }

        /// <summary>
        /// Gets or sets the white point used *when creating* Lab/LChab colors. (Lab/LChab colors on the input already contain the white point information)
        /// Defaults to: <see cref="CieLab.DefaultWhitePoint"/>.
        /// </summary>
        public CieXyz TargetLabWhitePoint { get; set; }

        /// <summary>
        /// Gets or sets the chromatic adaptation method used. When null, no adaptation will be performed.
        /// </summary>
        public IChromaticAdaptation ChromaticAdaptation { get; set; }

        /// <summary>
        /// Gets or sets transformation matrix used in conversion to <see cref="Lms"/>,
        /// also used in the default Von Kries Chromatic Adaptation method.
        /// </summary>
        public Matrix4x4 LmsAdaptationMatrix
        {
            get { return this.transformationMatrix; }
            set
            {
                this.transformationMatrix = value;

                if (this.cachedCieXyzAndLmsConverter == null)
                {
                    this.cachedCieXyzAndLmsConverter = new CieXyzAndLmsConverter(value);
                }
                else
                {
                    this.cachedCieXyzAndLmsConverter.TransformationMatrix = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether chromatic adaptation has been performed.
        /// </summary>
        private bool IsChromaticAdaptationPerformed => this.ChromaticAdaptation != null;
    }
}