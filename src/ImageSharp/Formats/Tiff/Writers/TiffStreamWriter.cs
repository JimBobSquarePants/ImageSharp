// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.IO;
using SixLabors.ImageSharp.Memory;

namespace SixLabors.ImageSharp.Formats.Experimental.Tiff.Writers
{
    /// <summary>
    /// Utility class for writing TIFF data to a <see cref="Stream"/>.
    /// </summary>
    internal class TiffStreamWriter : IDisposable
    {
        private static readonly byte[] PaddingBytes = new byte[4];

        /// <summary>
        /// Initializes a new instance of the <see cref="TiffStreamWriter"/> class.
        /// </summary>
        /// <param name="output">The output stream.</param>
        public TiffStreamWriter(Stream output) => this.BaseStream = output;

        /// <summary>
        /// Gets a value indicating whether the architecture is little-endian.
        /// </summary>
        public bool IsLittleEndian => BitConverter.IsLittleEndian;

        /// <summary>
        /// Gets the current position within the stream.
        /// </summary>
        public long Position => this.BaseStream.Position;

        /// <summary>
        /// Gets the base stream.
        /// </summary>
        public Stream BaseStream { get; }

        /// <summary>
        /// Writes an empty four bytes to the stream, returning the offset to be written later.
        /// </summary>
        /// <returns>The offset to be written later</returns>
        public long PlaceMarker()
        {
            long offset = this.BaseStream.Position;
            this.Write(0u);
            return offset;
        }

        /// <summary>
        /// Writes an array of bytes to the current stream.
        /// </summary>
        /// <param name="value">The bytes to write.</param>
        public void Write(byte[] value) => this.BaseStream.Write(value, 0, value.Length);

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The bytes to write.</param>
        public void Write(ReadOnlySpan<byte> value) => this.BaseStream.Write(value);

        /// <summary>
        /// Writes a byte to the current stream.
        /// </summary>
        /// <param name="value">The byte to write.</param>
        public void Write(byte value) => this.BaseStream.Write(new byte[] { value }, 0, 1);

        /// <summary>
        /// Writes a two-byte unsigned integer to the current stream.
        /// </summary>
        /// <param name="value">The two-byte unsigned integer to write.</param>
        public void Write(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.BaseStream.Write(bytes, 0, 2);
        }

        /// <summary>
        /// Writes a four-byte unsigned integer to the current stream.
        /// </summary>
        /// <param name="value">The four-byte unsigned integer to write.</param>
        public void Write(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.BaseStream.Write(bytes, 0, 4);
        }

        /// <summary>
        /// Writes an array of bytes to the current stream, padded to four-bytes.
        /// </summary>
        /// <param name="value">The bytes to write.</param>
        public void WritePadded(byte[] value)
        {
            this.BaseStream.Write(value, 0, value.Length);

            if (value.Length < 4)
            {
                this.BaseStream.Write(PaddingBytes, 0, 4 - value.Length);
            }
        }

        /// <summary>
        /// Writes a four-byte unsigned integer to the specified marker in the stream.
        /// </summary>
        /// <param name="offset">The offset returned when placing the marker</param>
        /// <param name="value">The four-byte unsigned integer to write.</param>
        public void WriteMarker(long offset, uint value)
        {
            long currentOffset = this.BaseStream.Position;
            this.BaseStream.Seek(offset, SeekOrigin.Begin);
            this.Write(value);
            this.BaseStream.Seek(currentOffset, SeekOrigin.Begin);
        }

        /// <summary>
        /// Disposes <see cref="TiffStreamWriter"/> instance, ensuring any unwritten data is flushed.
        /// </summary>
        public void Dispose() => this.BaseStream.Flush();
    }
}
