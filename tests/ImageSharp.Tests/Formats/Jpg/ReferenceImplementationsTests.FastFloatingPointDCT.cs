// ReSharper disable InconsistentNaming
namespace SixLabors.ImageSharp.Tests.Formats.Jpg
{
    using System;

    using SixLabors.ImageSharp.Formats.Jpeg.Common;
    using SixLabors.ImageSharp.Tests.Formats.Jpg.Utils;

    using Xunit;
    using Xunit.Abstractions;

    public partial class ReferenceImplementationsTests
    {
        public class FastFloatingPointDCT : JpegUtilityTestFixture
        {
            public FastFloatingPointDCT(ITestOutputHelper output)
                : base(output)
            {
            }

            [Theory]
            [InlineData(42, 0)]
            [InlineData(1, 0)]
            [InlineData(2, 0)]
            public void LLM_ForwardThenInverse(int seed, int startAt)
            {
                int[] data = JpegUtilityTestFixture.Create8x8RandomIntData(-1000, 1000, seed);
                float[] original = data.ConvertAllToFloat();
                float[] src = data.ConvertAllToFloat();
                float[] dest = new float[64];
                float[] temp = new float[64];

                ReferenceImplementations.LLM_FloatingPoint_DCT.fDCT2D_llm(src, dest, temp, true);
                ReferenceImplementations.LLM_FloatingPoint_DCT.iDCT2D_llm(dest, src, temp);

                this.CompareBlocks(original, src, 0.1f);
            }
            
            // [Fact]
            public void LLM_CalcConstants()
            {
                ReferenceImplementations.LLM_FloatingPoint_DCT.PrintConstants(this.Output);
            }
            
            [Theory]
            [InlineData(42, 1000)]
            [InlineData(1, 1000)]
            [InlineData(2, 1000)]
            [InlineData(42, 200)]
            [InlineData(1, 200)]
            [InlineData(2, 200)]
            public void LLM_IDCT_IsEquivalentTo_AccurateImplementation(int seed, int range)
            {
                float[] sourceArray = JpegUtilityTestFixture.Create8x8RoundedRandomFloatData(-1000, 1000, seed);

                var source = Block8x8F.Load(sourceArray);

                Block8x8F expected = ReferenceImplementations.AccurateDCT.TransformIDCT(ref source);

                Block8x8F actual = ReferenceImplementations.LLM_FloatingPoint_DCT.TransformIDCT(ref source);

                this.CompareBlocks(expected, actual, 0.1f);
            }
            
            [Theory]
            [InlineData(42)]
            [InlineData(1)]
            [InlineData(2)]
            public void LLM_FDCT_IsEquivalentTo_AccurateImplementation(int seed)
            {
                float[] floatData = JpegUtilityTestFixture.Create8x8RandomFloatData(-1000, 1000);

                Block8x8F source = default(Block8x8F);
                source.LoadFrom(floatData);

                Block8x8F expected = ReferenceImplementations.AccurateDCT.TransformFDCT(ref source);
                Block8x8F actual = ReferenceImplementations.LLM_FloatingPoint_DCT.TransformFDCT_UpscaleBy8(ref source);
                actual /= 8;

                this.CompareBlocks(expected, actual, 1f);
            }

            [Theory]
            [InlineData(42, 1000)]
            [InlineData(1, 1000)]
            [InlineData(2, 1000)]
            [InlineData(42, 200)]
            [InlineData(1, 200)]
            [InlineData(2, 200)]
            public void GT_IDCT_IsEquivalentTo_AccurateImplementation(int seed, int range)
            {
                int[] intData = JpegUtilityTestFixture.Create8x8RandomIntData(-range, range, seed);
                float[] floatSrc = intData.ConvertAllToFloat();

                ReferenceImplementations.AccurateDCT.TransformIDCTInplace(intData);

                float[] dest = new float[64];
                
                ReferenceImplementations.GT_FloatingPoint_DCT.iDCT8x8GT(floatSrc, dest);

                this.CompareBlocks(intData.ConvertAllToFloat(), dest, 1f);
            }
        }
    }
}