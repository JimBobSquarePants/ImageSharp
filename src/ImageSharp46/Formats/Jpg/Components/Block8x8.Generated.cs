﻿
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImageSharp.Formats
{
	public partial struct Block8x8
    {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TransposeInto(ref Block8x8 d)
        {
            d.V0L.X = V0L.X; d.V1L.X = V0L.Y; d.V2L.X = V0L.Z; d.V3L.X = V0L.W; d.V4L.X = V0R.X; d.V5L.X = V0R.Y; d.V6L.X = V0R.Z; d.V7L.X = V0R.W; 
            d.V0L.Y = V1L.X; d.V1L.Y = V1L.Y; d.V2L.Y = V1L.Z; d.V3L.Y = V1L.W; d.V4L.Y = V1R.X; d.V5L.Y = V1R.Y; d.V6L.Y = V1R.Z; d.V7L.Y = V1R.W; 
            d.V0L.Z = V2L.X; d.V1L.Z = V2L.Y; d.V2L.Z = V2L.Z; d.V3L.Z = V2L.W; d.V4L.Z = V2R.X; d.V5L.Z = V2R.Y; d.V6L.Z = V2R.Z; d.V7L.Z = V2R.W; 
            d.V0L.W = V3L.X; d.V1L.W = V3L.Y; d.V2L.W = V3L.Z; d.V3L.W = V3L.W; d.V4L.W = V3R.X; d.V5L.W = V3R.Y; d.V6L.W = V3R.Z; d.V7L.W = V3R.W; 
            d.V0R.X = V4L.X; d.V1R.X = V4L.Y; d.V2R.X = V4L.Z; d.V3R.X = V4L.W; d.V4R.X = V4R.X; d.V5R.X = V4R.Y; d.V6R.X = V4R.Z; d.V7R.X = V4R.W; 
            d.V0R.Y = V5L.X; d.V1R.Y = V5L.Y; d.V2R.Y = V5L.Z; d.V3R.Y = V5L.W; d.V4R.Y = V5R.X; d.V5R.Y = V5R.Y; d.V6R.Y = V5R.Z; d.V7R.Y = V5R.W; 
            d.V0R.Z = V6L.X; d.V1R.Z = V6L.Y; d.V2R.Z = V6L.Z; d.V3R.Z = V6L.W; d.V4R.Z = V6R.X; d.V5R.Z = V6R.Y; d.V6R.Z = V6R.Z; d.V7R.Z = V6R.W; 
            d.V0R.W = V7L.X; d.V1R.W = V7L.Y; d.V2R.W = V7L.Z; d.V3R.W = V7L.W; d.V4R.W = V7R.X; d.V5R.W = V7R.Y; d.V6R.W = V7R.Z; d.V7R.W = V7R.W; 
        }
	}
}
