﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System.Linq;
// ReSharper disable InconsistentNaming

// ReSharper disable MemberHidesStaticFromOuterClass
namespace SixLabors.ImageSharp.Tests
{
    /// <summary>
    /// Class that contains all the relative test image paths in the TestImages/Formats directory.
    /// Use with <see cref="WithFileAttribute"/>, <see cref="WithFileCollectionAttribute"/> or <see cref="FileTestBase"/>.
    /// </summary>
    public static class TestImages
    {
        public static class Png
        {
            public const string P1 = "Png/pl.png";
            public const string Pd = "Png/pd.png";
            public const string Blur = "Png/blur.png";
            public const string Indexed = "Png/indexed.png";
            public const string Splash = "Png/splash.png";
            public const string Cross = "Png/cross.png";
            public const string Powerpoint = "Png/pp.png";
            public const string SplashInterlaced = "Png/splash-interlaced.png";
            public const string Interlaced = "Png/interlaced.png";
            public const string Palette8Bpp = "Png/palette-8bpp.png";
            public const string Bpp1 = "Png/bpp1.png";
            public const string Gray4Bpp = "Png/gray_4bpp.png";
            public const string Gray16Bit = "Png/gray-16.png";
            public const string GrayAlpha8Bit = "Png/gray-alpha-8.png";
            public const string GrayAlpha16Bit = "Png/gray-alpha-16.png";
            public const string GrayTrns16BitInterlaced = "Png/gray-16-tRNS-interlaced.png";
            public const string Rgb24BppTrans = "Png/rgb-8-tRNS.png";
            public const string Rgb48Bpp = "Png/rgb-48bpp.png";
            public const string Rgb48BppInterlaced = "Png/rgb-48bpp-interlaced.png";
            public const string Rgb48BppTrans = "Png/rgb-16-tRNS.png";
            public const string Rgba64Bpp = "Png/rgb-16-alpha.png";
            public const string CalliphoraPartial = "Png/CalliphoraPartial.png";
            public const string CalliphoraPartialGrayscale = "Png/CalliphoraPartialGrayscale.png";
            public const string Bike = "Png/Bike.png";
            public const string BikeGrayscale = "Png/BikeGrayscale.png";
            public const string SnakeGame = "Png/SnakeGame.png";
            public const string Icon = "Png/icon.png";
            public const string Kaboom = "Png/kaboom.png";
            public const string PDSrc = "Png/pd-source.png";
            public const string PDDest = "Png/pd-dest.png";
            public const string Gray1BitTrans = "Png/gray-1-trns.png";
            public const string Gray2BitTrans = "Png/gray-2-tRNS.png";
            public const string Gray4BitTrans = "Png/gray-4-tRNS.png";
            public const string Gray8BitTrans = "Png/gray-8-tRNS.png";

            // Filtered test images from http://www.schaik.com/pngsuite/pngsuite_fil_png.html
            public const string Filter0 = "Png/filter0.png";
            public const string Filter1 = "Png/filter1.png";
            public const string Filter2 = "Png/filter2.png";
            public const string Filter3 = "Png/filter3.png";
            public const string Filter4 = "Png/filter4.png";

            // Filter changing per scanline
            public const string FilterVar = "Png/filterVar.png";

            public const string VimImage1 = "Png/vim16x16_1.png";
            public const string VimImage2 = "Png/vim16x16_2.png";

            public const string VersioningImage1 = "Png/versioning-1_1.png";
            public const string VersioningImage2 = "Png/versioning-1_2.png";

            public const string Banner7Adam7InterlaceMode = "Png/banner7-adam.png";
            public const string Banner8Index = "Png/banner8-index.png";

            public const string Ratio1x4 = "Png/ratio-1x4.png";
            public const string Ratio4x1 = "Png/ratio-4x1.png";

            public const string Ducky = "Png/ducky.png";
            public const string Rainbow = "Png/rainbow.png";

            public static class Bad
            {
                // Odd chunk lengths
                public const string ChunkLength1 = "Png/chunklength1.png";
                public const string ChunkLength2 = "Png/chunklength2.png";
                public const string CorruptedChunk = "Png/big-corrupted-chunk.png";
            }

            public static readonly string[] All =
            {
                P1, Pd, Blur, Splash, Cross,
                Powerpoint, SplashInterlaced, Interlaced,
                Filter0, Filter1, Filter2, Filter3, Filter4,
                FilterVar, VimImage1, VimImage2, VersioningImage1,
                VersioningImage2, Ratio4x1, Ratio1x4
            };
        }

        public static class Jpeg
        {
            public static class Progressive
            {
                public const string Fb = "Jpg/progressive/fb.jpg";
                public const string Progress = "Jpg/progressive/progress.jpg";
                public const string Festzug = "Jpg/progressive/Festzug.jpg";

                public static class Bad
                {
                    public const string BadEOF = "Jpg/progressive/BadEofProgressive.jpg";
                    public const string ExifUndefType = "Jpg/progressive/ExifUndefType.jpg";
                }

                public static readonly string[] All = { Fb, Progress, Festzug };
            }

            public static class Baseline
            {
                public static class Bad
                {
                    public const string BadEOF = "Jpg/baseline/badeof.jpg";
                    public const string BadRST = "Jpg/baseline/badrst.jpg";
                }

                public const string Cmyk = "Jpg/baseline/cmyk.jpg";
                public const string Exif = "Jpg/baseline/exif.jpg";
                public const string Floorplan = "Jpg/baseline/Floorplan.jpg";
                public const string Calliphora = "Jpg/baseline/Calliphora.jpg";
                public const string Ycck = "Jpg/baseline/ycck.jpg";
                public const string Turtle = "Jpg/baseline/turtle.jpg";
                public const string GammaDalaiLamaGray = "Jpg/baseline/gamma_dalai_lama_gray.jpg";
                public const string Hiyamugi = "Jpg/baseline/Hiyamugi.jpg";
                public const string Snake = "Jpg/baseline/Snake.jpg";
                public const string Lake = "Jpg/baseline/Lake.jpg";
                public const string Jpeg400 = "Jpg/baseline/jpeg400jfif.jpg";
                public const string Jpeg420Exif = "Jpg/baseline/jpeg420exif.jpg";
                public const string Jpeg444 = "Jpg/baseline/jpeg444.jpg";
                public const string Jpeg420Small = "Jpg/baseline/jpeg420small.jpg";
                public const string Testorig420 = "Jpg/baseline/testorig.jpg";
                public const string MultiScanBaselineCMYK = "Jpg/baseline/MultiScanBaselineCMYK.jpg";
                public const string Ratio1x1 = "Jpg/baseline/ratio-1x1.jpg";
                public const string Testorig12bit = "Jpg/baseline/testorig12.jpg";

                public static readonly string[] All =
                {
                    Cmyk, Ycck, Exif, Floorplan,
                    Calliphora, Turtle, GammaDalaiLamaGray,
                    Hiyamugi, Jpeg400, Jpeg420Exif, Jpeg444,
                    Ratio1x1, Testorig12bit
                };
            }

            public static class Issues
            {
                public const string CriticalEOF214 = "Jpg/issues/Issue214-CriticalEOF.jpg";
                public const string MissingFF00ProgressiveGirl159 = "Jpg/issues/Issue159-MissingFF00-Progressive-Girl.jpg";
                public const string MissingFF00ProgressiveBedroom159 = "Jpg/issues/Issue159-MissingFF00-Progressive-Bedroom.jpg";
                public const string BadCoeffsProgressive178 = "Jpg/issues/Issue178-BadCoeffsProgressive-Lemon.jpg";
                public const string BadZigZagProgressive385 = "Jpg/issues/Issue385-BadZigZag-Progressive.jpg";
                public const string MultiHuffmanBaseline394 = "Jpg/issues/Issue394-MultiHuffmanBaseline-Speakers.jpg";
                public const string NoEoiProgressive517 = "Jpg/issues/Issue517-No-EOI-Progressive.jpg";
                public const string BadRstProgressive518 = "Jpg/issues/Issue518-Bad-RST-Progressive.jpg";
                public const string InvalidCast520 = "Jpg/issues/Issue520-InvalidCast.jpg";
                public const string DhtHasWrongLength624 = "Jpg/issues/Issue624-DhtHasWrongLength-Progressive-N.jpg";
                public const string ExifDecodeOutOfRange694 = "Jpg/issues/Issue694-Decode-Exif-OutOfRange.jpg";
                public const string InvalidEOI695 = "Jpg/issues/Issue695-Invalid-EOI.jpg";
                public const string ExifResizeOutOfRange696 = "Jpg/issues/Issue696-Resize-Exif-OutOfRange.jpg";
                public const string InvalidAPP0721 = "Jpg/issues/Issue721-InvalidAPP0.jpg";
                public const string OrderedInterleavedProgressive723A = "Jpg/issues/Issue723-Ordered-Interleaved-Progressive-A.jpg";
                public const string OrderedInterleavedProgressive723B = "Jpg/issues/Issue723-Ordered-Interleaved-Progressive-B.jpg";
                public const string OrderedInterleavedProgressive723C = "Jpg/issues/Issue723-Ordered-Interleaved-Progressive-C.jpg";
                public const string ExifGetString750Transform = "Jpg/issues/issue750-exif-tranform.jpg";
                public const string ExifGetString750Load = "Jpg/issues/issue750-exif-load.jpg";
                public const string InvalidJpegThrowsWrongException797 = "Jpg/issues/Issue797-InvalidImage.jpg";
            }

            public static readonly string[] All = Baseline.All.Concat(Progressive.All).ToArray();

            public static class BenchmarkSuite
            {
                public const string Jpeg400_SmallMonochrome = Baseline.Jpeg400;
                public const string Jpeg420Exif_MidSizeYCbCr = Baseline.Jpeg420Exif;
                public const string Lake_Small444YCbCr = Baseline.Lake;

                // A few large images from the "issues" set are actually very useful for benchmarking:
                public const string MissingFF00ProgressiveBedroom159_MidSize420YCbCr = Issues.MissingFF00ProgressiveBedroom159;
                public const string BadRstProgressive518_Large444YCbCr = Issues.BadRstProgressive518;
                public const string ExifGetString750Transform_Huge420YCbCr = Issues.ExifGetString750Transform;
            }
        }

        public static class Bmp
        {
            // Note: The inverted images have been generated by altering the BitmapInfoHeader using a hex editor.
            // As such, the expected pixel output will be the reverse of the unaltered equivalent images.
            public const string Car = "Bmp/Car.bmp";
            public const string F = "Bmp/F.bmp";
            public const string NegHeight = "Bmp/neg_height.bmp";
            public const string CoreHeader = "Bmp/BitmapCoreHeaderQR.bmp";
            public const string V5Header = "Bmp/BITMAPV5HEADER.bmp";
            public const string RLE = "Bmp/RunLengthEncoded.bmp";
            public const string RLEInverted = "Bmp/RunLengthEncoded-inverted.bmp";
            public const string Bit1 = "Bmp/pal1.bmp";
            public const string Bit1Pal1 = "Bmp/pal1p1.bmp";
            public const string Bit4 = "Bmp/pal4.bmp";
            public const string Bit8 = "Bmp/test8.bmp";
            public const string Bit8Inverted = "Bmp/test8-inverted.bmp";
            public const string Bit16 = "Bmp/test16.bmp";
            public const string Bit16Inverted = "Bmp/test16-inverted.bmp";
            public const string Bit32Rgb = "Bmp/rgb32.bmp";
            public const string Bit32Rgba = "Bmp/rgba32.bmp";

            // Note: This format can be called OS/2 BMPv1, or Windows BMPv2
            public const string WinBmpv2 = "Bmp/pal8os2v1_winv2.bmp";
            public const string WinBmpv3 = "Bmp/rgb24.bmp";
            public const string WinBmpv4 = "Bmp/pal8v4.bmp";
            public const string WinBmpv5 = "Bmp/pal8v5.bmp";
            public const string Bit8Palette4 = "Bmp/pal8-0.bmp";
            public const string Os2v2Short = "Bmp/pal8os2v2-16.bmp";

            // Bitmap images with compression type BITFIELDS
            public const string Rgb32bfdef = "Bmp/rgb32bfdef.bmp";
            public const string Rgb32bf = "Bmp/rgb32bf.bmp";
            public const string Rgb16565 = "Bmp/rgb16-565.bmp";
            public const string Rgb16bfdef = "Bmp/rgb16bfdef.bmp";
            public const string Rgb16565pal = "Bmp/rgb16-565pal.bmp";
            public const string Issue735 = "Bmp/issue735.bmp";
            public const string Rgba32bf56 = "Bmp/rgba32h56.bmp";
            public const string Rgba321010102 = "Bmp/rgba32-1010102.bmp";

            public static readonly string[] BitFields 
            = {
                  Rgb32bfdef,
                  Rgb32bf,
                  Rgb16565,
                  Rgb16bfdef,
                  Rgb16565pal,
                  Issue735,
            };

            public static readonly string[] All
            = {
                Car,
                F,
                NegHeight,
                CoreHeader,
                V5Header, RLE,
                RLEInverted,
                Bit1,
                Bit1Pal1,
                Bit4,
                Bit8,
                Bit8Inverted,
                Bit16,
                Bit16Inverted,

                // TODO: Disabled because, alpha channel is not correctly decoded.
                // This is a bitmap v3 header without an alpha channel (it is 32 bpp, but alpha values are all 0).
                // Re-Enable this, once #732 is fixed.
                // Bit32Rgb
            };
        }

        public static class Gif
        {
            public const string Rings = "Gif/rings.gif";
            public const string Giphy = "Gif/giphy.gif";
            public const string Cheers = "Gif/cheers.gif";
            public const string Trans = "Gif/trans.gif";
            public const string Kumin = "Gif/kumin.gif";
            public const string Leo = "Gif/leo.gif";
            public const string Ratio4x1 = "Gif/base_4x1.gif";
            public const string Ratio1x4 = "Gif/base_1x4.gif";

            public static class Issues
            {
                public const string BadAppExtLength = "Gif/issues/issue405_badappextlength252.gif";
                public const string BadAppExtLength_2 = "Gif/issues/issue405_badappextlength252-2.gif";
                public const string BadDescriptorWidth = "Gif/issues/issue403_baddescriptorwidth.gif";
            }

            public static readonly string[] All = { Rings, Giphy, Cheers, Trans, Kumin, Leo, Ratio4x1, Ratio1x4 };
        }
    }
}
