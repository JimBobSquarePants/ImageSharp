﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.PixelFormats.PixelBlenders;

namespace SixLabors.ImageSharp.PixelFormats
{
    /// <content>
    /// Provides access to pixel blenders
    /// </content>
    public partial class PixelOperations<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// Find an instance of the pixel blender.
        /// </summary>
        /// <param name="mode">The blending mode to apply</param>
        /// <returns>A <see cref="PixelBlender{TPixel}"/>.</returns>
        internal virtual PixelBlender<TPixel> GetPixelBlender(PixelBlenderMode mode)
        {
            switch (mode)
            {
                case PixelBlenderMode.Multiply: return DefaultPixelBlenders<TPixel>.Multiply_SrcOver.Instance;
                case PixelBlenderMode.Add: return DefaultPixelBlenders<TPixel>.Add_SrcOver.Instance;
                case PixelBlenderMode.Subtract: return DefaultPixelBlenders<TPixel>.Subtract_SrcOver.Instance;
                case PixelBlenderMode.Screen: return DefaultPixelBlenders<TPixel>.Screen_SrcOver.Instance;
                case PixelBlenderMode.Darken: return DefaultPixelBlenders<TPixel>.Darken_SrcOver.Instance;
                case PixelBlenderMode.Lighten: return DefaultPixelBlenders<TPixel>.Lighten_SrcOver.Instance;
                case PixelBlenderMode.Overlay: return DefaultPixelBlenders<TPixel>.Overlay_SrcOver.Instance;
                case PixelBlenderMode.HardLight: return DefaultPixelBlenders<TPixel>.HardLight_SrcOver.Instance;
                case PixelBlenderMode.Src: return DefaultPixelBlenders<TPixel>.Normal_Src.Instance;
                case PixelBlenderMode.Atop: return DefaultPixelBlenders<TPixel>.Normal_SrcAtop.Instance;
                case PixelBlenderMode.Over: return DefaultPixelBlenders<TPixel>.Normal_SrcOver.Instance;
                case PixelBlenderMode.In: return DefaultPixelBlenders<TPixel>.Normal_SrcIn.Instance;
                case PixelBlenderMode.Out: return DefaultPixelBlenders<TPixel>.Normal_SrcOut.Instance;
                case PixelBlenderMode.Dest: return DefaultPixelBlenders<TPixel>.Normal_Dest.Instance;
                case PixelBlenderMode.DestAtop: return DefaultPixelBlenders<TPixel>.Normal_DestAtop.Instance;
                case PixelBlenderMode.DestOver: return DefaultPixelBlenders<TPixel>.Normal_DestOver.Instance;
                case PixelBlenderMode.DestIn: return DefaultPixelBlenders<TPixel>.Normal_DestIn.Instance;
                case PixelBlenderMode.DestOut: return DefaultPixelBlenders<TPixel>.Normal_DestOut.Instance;
                case PixelBlenderMode.Clear: return DefaultPixelBlenders<TPixel>.Normal_Clear.Instance;
                case PixelBlenderMode.Xor: return DefaultPixelBlenders<TPixel>.Normal_Xor.Instance;

                case PixelBlenderMode.Normal:
                default:
                    return DefaultPixelBlenders<TPixel>.Normal_SrcOver.Instance;
            }
        }
    }
}