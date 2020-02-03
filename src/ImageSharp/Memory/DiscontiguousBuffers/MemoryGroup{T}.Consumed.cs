// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;

namespace SixLabors.ImageSharp.Memory
{
    internal abstract partial class MemoryGroup<T>
    {
        // Analogous to the "consumed" variant of MemorySource
        private class Consumed : MemoryGroup<T>
        {
            private readonly ReadOnlyMemory<Memory<T>> source;

            public Consumed(ReadOnlyMemory<Memory<T>> source, int bufferLength, long totalLength)
                : base(bufferLength, totalLength)
            {
                this.source = source;
                this.View = new MemoryGroupView<T>(this);
            }

            public override int Count => this.source.Length;

            public override Memory<T> this[int index] => this.source.Span[index];

            public override IEnumerator<Memory<T>> GetEnumerator()
            {
                for (int i = 0; i < this.source.Length; i++)
                {
                    yield return this.source.Span[i];
                }
            }

            public override void Dispose()
            {
                this.View.Invalidate();
            }
        }
    }
}
