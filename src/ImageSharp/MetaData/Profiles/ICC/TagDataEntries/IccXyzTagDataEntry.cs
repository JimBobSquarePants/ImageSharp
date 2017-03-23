﻿// <copyright file="IccXyzTagDataEntry.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp
{
    using System.Linq;
    using System.Numerics;

    /// <summary>
    /// The XYZType contains an array of XYZ values.
    /// </summary>
    internal sealed class IccXyzTagDataEntry : IccTagDataEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IccXyzTagDataEntry"/> class.
        /// </summary>
        /// <param name="data">The XYZ numbers</param>
        public IccXyzTagDataEntry(Vector3[] data)
            : this(data, IccProfileTag.Unknown)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IccXyzTagDataEntry"/> class.
        /// </summary>
        /// <param name="data">The XYZ numbers</param>
        /// <param name="tagSignature">Tag Signature</param>
        public IccXyzTagDataEntry(Vector3[] data, IccProfileTag tagSignature)
            : base(IccTypeSignature.Xyz, tagSignature)
        {
            Guard.NotNull(data, nameof(data));
            this.Data = data;
        }

        /// <summary>
        /// Gets the XYZ numbers
        /// </summary>
        public Vector3[] Data { get; }

        /// <inheritdoc />
        public override bool Equals(IccTagDataEntry other)
        {
            if (base.Equals(other) && other is IccXyzTagDataEntry entry)
            {
                return this.Data.SequenceEqual(entry.Data);
            }

            return false;
        }
    }
}
