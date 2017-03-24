﻿// <copyright file="IccDataWriter.Primitives.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp
{
    using System;
    using System.Text;

    /// <summary>
    /// Provides methods to write ICC data types
    /// </summary>
    internal sealed partial class IccDataWriter
    {
        /// <summary>
        /// Writes a byte
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public int WriteByte(byte value)
        {
            this.dataStream.WriteByte(value);
            return 1;
        }

        /// <summary>
        /// Writes an ushort
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public unsafe int WriteUInt16(ushort value)
        {
            return this.WriteBytes((byte*)&value, 2);
        }

        /// <summary>
        /// Writes a short
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public unsafe int WriteInt16(short value)
        {
            return this.WriteBytes((byte*)&value, 2);
        }

        /// <summary>
        /// Writes an uint
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public unsafe int WriteUInt32(uint value)
        {
            return this.WriteBytes((byte*)&value, 4);
        }

        /// <summary>
        /// Writes an int
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public unsafe int WriteInt32(int value)
        {
            return this.WriteBytes((byte*)&value, 4);
        }

        /// <summary>
        /// Writes an ulong
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public unsafe int WriteUInt64(ulong value)
        {
            return this.WriteBytes((byte*)&value, 8);
        }

        /// <summary>
        /// Writes a long
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public unsafe int WriteInt64(long value)
        {
            return this.WriteBytes((byte*)&value, 8);
        }

        /// <summary>
        /// Writes a float
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public unsafe int WriteSingle(float value)
        {
            return this.WriteBytes((byte*)&value, 4);
        }

        /// <summary>
        /// Writes a double
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public unsafe int WriteDouble(double value)
        {
            return this.WriteBytes((byte*)&value, 8);
        }

        /// <summary>
        /// Writes a signed 32bit number with 1 sign bit, 15 value bits and 16 fractional bits
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public int WriteFix16(double value)
        {
            const double max = short.MaxValue + (65535d / 65536d);
            const double min = short.MinValue;

            value = value.Clamp(min, max);
            value *= 65536d;

            return this.WriteInt32((int)Math.Round(value, MidpointRounding.AwayFromZero));
        }

        /// <summary>
        /// Writes an unsigned 32bit number with 16 value bits and 16 fractional bits
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public int WriteUFix16(double value)
        {
            const double max = ushort.MaxValue + (65535d / 65536d);
            const double min = ushort.MinValue;

            value = value.Clamp(min, max);
            value *= 65536d;

            return this.WriteUInt32((uint)Math.Round(value, MidpointRounding.AwayFromZero));
        }

        /// <summary>
        /// Writes an unsigned 16bit number with 1 value bit and 15 fractional bits
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public int WriteU1Fix15(double value)
        {
            const double max = 1 + (32767d / 32768d);
            const double min = 0;

            value = value.Clamp(min, max);
            value *= 32768d;

            return this.WriteUInt16((ushort)Math.Round(value, MidpointRounding.AwayFromZero));
        }

        /// <summary>
        /// Writes an unsigned 16bit number with 8 value bits and 8 fractional bits
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <returns>the number of bytes written</returns>
        public int WriteUFix8(double value)
        {
            const double max = byte.MaxValue + (255d / 256d);
            const double min = byte.MinValue;

            value = value.Clamp(min, max);
            value *= 256d;

            return this.WriteUInt16((ushort)Math.Round(value, MidpointRounding.AwayFromZero));
        }

        /// <summary>
        /// Writes an ASCII encoded string
        /// </summary>
        /// <param name="value">the string to write</param>
        /// <returns>the number of bytes written</returns>
        public int WriteASCIIString(string value)
        {
            byte[] data = AsciiEncoding.GetBytes(value);
            this.dataStream.Write(data, 0, data.Length);
            return data.Length;
        }

        /// <summary>
        /// Writes an ASCII encoded string resizes it to the given length
        /// </summary>
        /// <param name="value">The string to write</param>
        /// <param name="length">The desired length of the string including 1 padding character</param>
        /// <param name="paddingChar">The character to pad to the given length</param>
        /// <returns>the number of bytes written</returns>
        public int WriteASCIIString(string value, int length, char paddingChar)
        {
            value = value.Substring(0, Math.Min(length - 1, value.Length));

            byte[] textData = AsciiEncoding.GetBytes(value);
            int actualLength = Math.Min(length - 1, textData.Length);
            this.dataStream.Write(textData, 0, actualLength);
            for (int i = 0; i < length - actualLength; i++)
            {
                this.dataStream.WriteByte((byte)paddingChar);
            }

            return length;
        }

        /// <summary>
        /// Writes an UTF-16 big-endian encoded string
        /// </summary>
        /// <param name="value">the string to write</param>
        /// <returns>the number of bytes written</returns>
        public int WriteUnicodeString(string value)
        {
            byte[] data = Encoding.BigEndianUnicode.GetBytes(value);
            this.dataStream.Write(data, 0, data.Length);
            return data.Length;
        }
    }
}
