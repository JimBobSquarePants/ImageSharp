// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.Numerics;
using SixLabors.ImageSharp.Metadata.Profiles.Icc;

namespace SixLabors.ImageSharp.Tests.TestDataIcc.Conversion
{
    public class IccConversionDataLutAB
    {
        private static readonly IccLutAToBTagDataEntry LutAtoBSingleCurve = new IccLutAToBTagDataEntry(
           new IccTagDataEntry[]
           {
               IccConversionDataTrc.IdentityCurve,
               IccConversionDataTrc.IdentityCurve,
               IccConversionDataTrc.IdentityCurve
           },
           null,
           null,
           null,
           null,
           null);

        // also need:
        // # CurveM + matrix
        // # CurveA + CLUT + CurveB
        // # CurveA + CLUT + CurveM + Matrix + CurveB
        private static readonly IccLutBToATagDataEntry LutBtoASingleCurve = new IccLutBToATagDataEntry(
           new IccTagDataEntry[]
           {
               IccConversionDataTrc.IdentityCurve,
               IccConversionDataTrc.IdentityCurve,
               IccConversionDataTrc.IdentityCurve
           },
           null,
           null,
           null,
           null,
           null);

        public static object[][] LutAToBConversionTestData =
        {
            new object[] { LutAtoBSingleCurve, new Vector4(0.2f, 0.3f, 0.4f, 0), new Vector4(0.2f, 0.3f, 0.4f, 0) },
        };

        public static object[][] LutBToAConversionTestData =
        {
            new object[] { LutBtoASingleCurve, new Vector4(0.2f, 0.3f, 0.4f, 0), new Vector4(0.2f, 0.3f, 0.4f, 0) },
        };
    }
}
