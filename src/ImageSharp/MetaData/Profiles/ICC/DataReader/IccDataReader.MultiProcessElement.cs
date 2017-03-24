﻿// <copyright file="IccDataReader.MultiProcessElement.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp
{
    /// <summary>
    /// Provides methods to read ICC data types
    /// </summary>
    internal sealed partial class IccDataReader
    {
        /// <summary>
        /// Reads a <see cref="IccMultiProcessElement"/>
        /// </summary>
        /// <returns>The read <see cref="IccMultiProcessElement"/></returns>
        public IccMultiProcessElement ReadMultiProcessElement()
        {
            IccMultiProcessElementSignature signature = (IccMultiProcessElementSignature)this.ReadUInt32();
            ushort inChannelCount = this.ReadUInt16();
            ushort outChannelCount = this.ReadUInt16();

            switch (signature)
            {
                case IccMultiProcessElementSignature.CurveSet:
                    return this.ReadCurveSetProcessElement(inChannelCount, outChannelCount);
                case IccMultiProcessElementSignature.Matrix:
                    return this.ReadMatrixProcessElement(inChannelCount, outChannelCount);
                case IccMultiProcessElementSignature.Clut:
                    return this.ReadCLUTProcessElement(inChannelCount, outChannelCount);

                // Currently just placeholders for future ICC expansion
                case IccMultiProcessElementSignature.BAcs:
                    this.AddIndex(8);
                    return new IccBAcsProcessElement(inChannelCount, outChannelCount);
                case IccMultiProcessElementSignature.EAcs:
                    this.AddIndex(8);
                    return new IccEAcsProcessElement(inChannelCount, outChannelCount);

                default:
                    throw new InvalidIccProfileException($"Invalid MultiProcessElement type of {signature}");
            }
        }

        /// <summary>
        /// Reads a CurveSet <see cref="IccMultiProcessElement"/>
        /// </summary>
        /// <param name="inChannelCount">Number of input channels</param>
        /// <param name="outChannelCount">Number of output channels</param>
        /// <returns>The read <see cref="IccCurveSetProcessElement"/></returns>
        public IccCurveSetProcessElement ReadCurveSetProcessElement(int inChannelCount, int outChannelCount)
        {
            IccOneDimensionalCurve[] curves = new IccOneDimensionalCurve[inChannelCount];
            for (int i = 0; i < inChannelCount; i++)
            {
                curves[i] = this.ReadOneDimensionalCurve();
                this.AddPadding();
            }

            return new IccCurveSetProcessElement(curves);
        }

        /// <summary>
        /// Reads a Matrix <see cref="IccMultiProcessElement"/>
        /// </summary>
        /// <param name="inChannelCount">Number of input channels</param>
        /// <param name="outChannelCount">Number of output channels</param>
        /// <returns>The read <see cref="IccMatrixProcessElement"/></returns>
        public IccMatrixProcessElement ReadMatrixProcessElement(int inChannelCount, int outChannelCount)
        {
            return new IccMatrixProcessElement(
                this.ReadMatrix(inChannelCount, outChannelCount, true),
                this.ReadMatrix(outChannelCount, true));
        }

        /// <summary>
        /// Reads a CLUT <see cref="IccMultiProcessElement"/>
        /// </summary>
        /// <param name="inChCount">Number of input channels</param>
        /// <param name="outChCount">Number of output channels</param>
        /// <returns>The read <see cref="IccClutProcessElement"/></returns>
        public IccClutProcessElement ReadCLUTProcessElement(int inChCount, int outChCount)
        {
            return new IccClutProcessElement(this.ReadCLUT(inChCount, outChCount, true));
        }
    }
}
