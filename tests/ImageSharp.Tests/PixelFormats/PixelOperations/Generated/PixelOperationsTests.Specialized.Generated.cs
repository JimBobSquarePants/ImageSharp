// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

// <auto-generated />

using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Xunit.Abstractions;


namespace SixLabors.ImageSharp.Tests.PixelFormats.PixelOperations
{
    public partial class PixelOperationsTests
    {
        
        public partial class A8_OperationsTests : PixelOperationsTests<A8>
        {
            public A8_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  A8.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<A8.PixelOperations>(PixelOperations<A8>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = A8.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<A8>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = A8.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class Argb32_OperationsTests : PixelOperationsTests<Argb32>
        {
            public Argb32_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Argb32.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Argb32.PixelOperations>(PixelOperations<Argb32>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Argb32.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Argb32>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Argb32.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class Bgr24_OperationsTests : PixelOperationsTests<Bgr24>
        {
            public Bgr24_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Bgr24.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Bgr24.PixelOperations>(PixelOperations<Bgr24>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Bgr24.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Bgr24>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Bgr24.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class Bgr565_OperationsTests : PixelOperationsTests<Bgr565>
        {
            public Bgr565_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Bgr565.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Bgr565.PixelOperations>(PixelOperations<Bgr565>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Bgr565.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Bgr565>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Bgr565.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class Bgra32_OperationsTests : PixelOperationsTests<Bgra32>
        {
            public Bgra32_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Bgra32.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Bgra32.PixelOperations>(PixelOperations<Bgra32>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Bgra32.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Bgra32>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Bgra32.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class Bgra4444_OperationsTests : PixelOperationsTests<Bgra4444>
        {
            public Bgra4444_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Bgra4444.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Bgra4444.PixelOperations>(PixelOperations<Bgra4444>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Bgra4444.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Bgra4444>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Bgra4444.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class Bgra5551_OperationsTests : PixelOperationsTests<Bgra5551>
        {
            public Bgra5551_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Bgra5551.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Bgra5551.PixelOperations>(PixelOperations<Bgra5551>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Bgra5551.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Bgra5551>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Bgra5551.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class Byte4_OperationsTests : PixelOperationsTests<Byte4>
        {
            public Byte4_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Byte4.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Byte4.PixelOperations>(PixelOperations<Byte4>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Byte4.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Byte4>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Byte4.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class HalfSingle_OperationsTests : PixelOperationsTests<HalfSingle>
        {
            public HalfSingle_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  HalfSingle.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<HalfSingle.PixelOperations>(PixelOperations<HalfSingle>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = HalfSingle.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<HalfSingle>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = HalfSingle.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class HalfVector2_OperationsTests : PixelOperationsTests<HalfVector2>
        {
            public HalfVector2_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  HalfVector2.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<HalfVector2.PixelOperations>(PixelOperations<HalfVector2>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = HalfVector2.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<HalfVector2>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = HalfVector2.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class HalfVector4_OperationsTests : PixelOperationsTests<HalfVector4>
        {
            public HalfVector4_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  HalfVector4.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<HalfVector4.PixelOperations>(PixelOperations<HalfVector4>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = HalfVector4.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<HalfVector4>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = HalfVector4.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class L16_OperationsTests : PixelOperationsTests<L16>
        {
            public L16_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  L16.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<L16.PixelOperations>(PixelOperations<L16>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = L16.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<L16>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = L16.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class L8_OperationsTests : PixelOperationsTests<L8>
        {
            public L8_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  L8.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<L8.PixelOperations>(PixelOperations<L8>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = L8.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<L8>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = L8.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class La16_OperationsTests : PixelOperationsTests<La16>
        {
            public La16_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  La16.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<La16.PixelOperations>(PixelOperations<La16>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = La16.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<La16>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = La16.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class La32_OperationsTests : PixelOperationsTests<La32>
        {
            public La32_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  La32.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<La32.PixelOperations>(PixelOperations<La32>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = La32.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<La32>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = La32.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class NormalizedByte2_OperationsTests : PixelOperationsTests<NormalizedByte2>
        {
            public NormalizedByte2_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  NormalizedByte2.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<NormalizedByte2.PixelOperations>(PixelOperations<NormalizedByte2>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = NormalizedByte2.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<NormalizedByte2>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = NormalizedByte2.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class NormalizedByte4_OperationsTests : PixelOperationsTests<NormalizedByte4>
        {
            public NormalizedByte4_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  NormalizedByte4.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<NormalizedByte4.PixelOperations>(PixelOperations<NormalizedByte4>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = NormalizedByte4.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<NormalizedByte4>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = NormalizedByte4.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class NormalizedShort2_OperationsTests : PixelOperationsTests<NormalizedShort2>
        {
            public NormalizedShort2_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  NormalizedShort2.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<NormalizedShort2.PixelOperations>(PixelOperations<NormalizedShort2>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = NormalizedShort2.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<NormalizedShort2>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = NormalizedShort2.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class NormalizedShort4_OperationsTests : PixelOperationsTests<NormalizedShort4>
        {
            public NormalizedShort4_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  NormalizedShort4.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<NormalizedShort4.PixelOperations>(PixelOperations<NormalizedShort4>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = NormalizedShort4.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<NormalizedShort4>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = NormalizedShort4.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class Rg32_OperationsTests : PixelOperationsTests<Rg32>
        {
            public Rg32_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Rg32.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Rg32.PixelOperations>(PixelOperations<Rg32>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Rg32.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Rg32>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Rg32.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class Rgb24_OperationsTests : PixelOperationsTests<Rgb24>
        {
            public Rgb24_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Rgb24.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Rgb24.PixelOperations>(PixelOperations<Rgb24>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Rgb24.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Rgb24>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Rgb24.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class Rgb48_OperationsTests : PixelOperationsTests<Rgb48>
        {
            public Rgb48_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Rgb48.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Rgb48.PixelOperations>(PixelOperations<Rgb48>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Rgb48.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Rgb48>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Rgb48.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class Rgba1010102_OperationsTests : PixelOperationsTests<Rgba1010102>
        {
            public Rgba1010102_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Rgba1010102.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Rgba1010102.PixelOperations>(PixelOperations<Rgba1010102>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Rgba1010102.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Rgba1010102>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Rgba1010102.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class Rgba32_OperationsTests : PixelOperationsTests<Rgba32>
        {
            public Rgba32_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Rgba32.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Rgba32.PixelOperations>(PixelOperations<Rgba32>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Rgba32.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Rgba32>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Rgba32.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class Rgba64_OperationsTests : PixelOperationsTests<Rgba64>
        {
            public Rgba64_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Rgba64.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Rgba64.PixelOperations>(PixelOperations<Rgba64>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Rgba64.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Rgba64>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Rgba64.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class RgbaVector_OperationsTests : PixelOperationsTests<RgbaVector>
        {
            public RgbaVector_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  RgbaVector.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<RgbaVector.PixelOperations>(PixelOperations<RgbaVector>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = RgbaVector.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<RgbaVector>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = RgbaVector.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }

        public partial class Short2_OperationsTests : PixelOperationsTests<Short2>
        {
            public Short2_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Short2.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Short2.PixelOperations>(PixelOperations<Short2>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Short2.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Short2>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Short2.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.None, alphaRepresentation);
            }
        }

        public partial class Short4_OperationsTests : PixelOperationsTests<Short4>
        {
            public Short4_OperationsTests(ITestOutputHelper output)
                : base(output)
            {
                var alphaRepresentation =  Short4.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                this.HasUnassociatedAlpha = alphaRepresentation == PixelAlphaRepresentation.Unassociated;
            }

            [Fact]
            public void IsSpecialImplementation() => Assert.IsType<Short4.PixelOperations>(PixelOperations<Short4>.Instance);

            [Fact]
            public void PixelTypeInfoHasCorrectBitsPerPixel()
            {
                var bits = Short4.PixelOperations.Instance.GetPixelTypeInfo().BitsPerPixel;
                Assert.Equal(Unsafe.SizeOf<Short4>() * 8, bits);
            }

            [Fact]
            public void PixelTypeInfoHasCorrectAlphaRepresentation()
            {
                var alphaRepresentation = Short4.PixelOperations.Instance.GetPixelTypeInfo().AlphaRepresentation;
                Assert.Equal(PixelAlphaRepresentation.Unassociated, alphaRepresentation);
            }
        }
    }
}
