using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp
{
    internal static partial class SimdUtils
    {
        [MethodImpl(InliningOptions.ShortMethod)]
        internal static void PackFromRgbPlanes(
            Configuration configuration,
            ReadOnlySpan<byte> redChannel,
            ReadOnlySpan<byte> greenChannel,
            ReadOnlySpan<byte> blueChannel,
            Span<Rgb24> destination)
        {
            PackFromRgbPlanesScalarBatchedReduce(ref redChannel, ref greenChannel, ref blueChannel, ref destination);
            PackFromRgbPlanesRemainder(redChannel, greenChannel, blueChannel, destination);
        }

        [MethodImpl(InliningOptions.ShortMethod)]
        internal static void PackFromRgbPlanes(
            Configuration configuration,
            ReadOnlySpan<byte> redChannel,
            ReadOnlySpan<byte> greenChannel,
            ReadOnlySpan<byte> blueChannel,
            Span<Rgba32> destination)
        {
        }

        private static void PackFromRgbPlanesScalarBatchedReduce(
            ref ReadOnlySpan<byte> redChannel,
            ref ReadOnlySpan<byte> greenChannel,
            ref ReadOnlySpan<byte> blueChannel,
            ref Span<Rgb24> destination)
        {
            ref ByteTuple4 r = ref Unsafe.As<byte, ByteTuple4>(ref MemoryMarshal.GetReference(redChannel));
            ref ByteTuple4 g = ref Unsafe.As<byte, ByteTuple4>(ref MemoryMarshal.GetReference(greenChannel));
            ref ByteTuple4 b = ref Unsafe.As<byte, ByteTuple4>(ref MemoryMarshal.GetReference(blueChannel));
            ref Rgb24 rgb = ref MemoryMarshal.GetReference(destination);

            int count = destination.Length / 4;
            for (int i = 0; i < count; i++)
            {
                ref Rgb24 d0 = ref Unsafe.Add(ref rgb, i * 4);
                ref Rgb24 d1 = ref Unsafe.Add(ref d0, 1);
                ref Rgb24 d2 = ref Unsafe.Add(ref d0, 2);
                ref Rgb24 d3 = ref Unsafe.Add(ref d0, 3);

                ref ByteTuple4 rr = ref Unsafe.Add(ref r, i);
                ref ByteTuple4 gg = ref Unsafe.Add(ref g, i);
                ref ByteTuple4 bb = ref Unsafe.Add(ref b, i);

                d0.R = rr.V0;
                d0.G = gg.V0;
                d0.B = bb.V0;

                d1.R = rr.V1;
                d1.G = gg.V1;
                d1.B = bb.V1;

                d2.R = rr.V2;
                d2.G = gg.V2;
                d2.B = bb.V2;

                d3.R = rr.V3;
                d3.G = gg.V3;
                d3.B = bb.V3;
            }

            int finished = count * 4;
            redChannel = redChannel.Slice(finished);
            greenChannel = greenChannel.Slice(finished);
            blueChannel = blueChannel.Slice(finished);
            destination = destination.Slice(finished);
        }

        private static void PackFromRgbPlanesRemainder(
            ReadOnlySpan<byte> redChannel,
            ReadOnlySpan<byte> greenChannel,
            ReadOnlySpan<byte> blueChannel,
            Span<Rgb24> destination)
        {
            ref byte r = ref MemoryMarshal.GetReference(redChannel);
            ref byte g = ref MemoryMarshal.GetReference(greenChannel);
            ref byte b = ref MemoryMarshal.GetReference(blueChannel);
            ref Rgb24 rgb = ref MemoryMarshal.GetReference(destination);

            for (int i = 0; i < destination.Length; i++)
            {
                ref Rgb24 d = ref Unsafe.Add(ref rgb, i);
                d.R = Unsafe.Add(ref r, i);
                d.G = Unsafe.Add(ref g, i);
                d.B = Unsafe.Add(ref b, i);
            }
        }
    }
}