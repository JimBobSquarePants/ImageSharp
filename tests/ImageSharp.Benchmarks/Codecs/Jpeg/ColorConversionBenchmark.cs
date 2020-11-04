﻿using System;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using SixLabors.ImageSharp.Memory;

namespace SixLabors.ImageSharp.Benchmarks.Codecs.Jpeg
{
    public abstract class ColorConversionBenchmark
    {
        private readonly int componentCount;
        protected Buffer2D<float>[] input;
        protected Vector4[] output;

        protected ColorConversionBenchmark(int componentCount)
        {
            this.componentCount = componentCount;
        }

        public const int Count = 128;

        [GlobalSetup]
        public void Setup()
        {
            this.input = CreateRandomValues(componentCount, Count);
            this.output = new Vector4[Count];
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            foreach (Buffer2D<float> buffer in this.input)
            {
                buffer.Dispose();
            }
        }

        private static Buffer2D<float>[] CreateRandomValues(
            int componentCount,
            int inputBufferLength,
            float minVal = 0f,
            float maxVal = 255f)
        {
            var rnd = new Random(42);
            var buffers = new Buffer2D<float>[componentCount];
            for (int i = 0; i < componentCount; i++)
            {
                var values = new float[inputBufferLength];

                for (int j = 0; j < inputBufferLength; j++)
                {
                    values[j] = ((float)rnd.NextDouble() * (maxVal - minVal)) + minVal;
                }

                // no need to dispose when buffer is not array owner
                buffers[i] = Configuration.Default.MemoryAllocator.Allocate2D<float>(values.Length, 1);
            }

            return buffers;
        }
    }
}
