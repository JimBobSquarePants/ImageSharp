﻿// <copyright file="Bits.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Runtime.CompilerServices;

namespace ImageSharp.Formats
{
    /// <summary>
    /// Holds the unprocessed bits that have been taken from the byte-stream.
    /// The n least significant bits of a form the unread bits, to be read in MSB to
    /// LSB order.
    /// </summary>
    internal struct Bits
    {
        /// <summary>
        /// Gets or sets the accumulator.
        /// </summary>
        public uint Accumulator;

        /// <summary>
        /// Gets or sets the mask.
        /// <![CDATA[mask==1<<(unreadbits-1) when unreadbits>0, with mask==0 when unreadbits==0.]]>
        /// </summary>
        public uint Mask;

        /// <summary>
        /// Gets or sets the  number of unread bits in the accumulator.
        /// </summary>
        public int UnreadBits;


        /// <summary>
        /// Reads bytes from the byte buffer to ensure that bits.UnreadBits is at
        /// least n. For best performance (avoiding function calls inside hot loops),
        /// the caller is the one responsible for first checking that bits.UnreadBits &lt; n.
        /// </summary>
        /// <param name="n">The number of bits to ensure.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal JpegDecoderCore.ErrorCodes EnsureNBits(int n, JpegDecoderCore decoder)
        {
            while (true)
            {
                JpegDecoderCore.ErrorCodes errorCode;
                
                byte c = decoder.bytes.ReadByteStuffedByte(decoder.inputStream, out errorCode);

                if (errorCode != JpegDecoderCore.ErrorCodes.NoError)
                {
                    return errorCode;
                }

                Accumulator = (Accumulator << 8) | c;
                UnreadBits += 8;
                if (Mask == 0)
                {
                    Mask = 1 << 7;
                }
                else
                {
                    Mask <<= 8;
                }

                if (UnreadBits >= n)
                {
                    return JpegDecoderCore.ErrorCodes.NoError;
                    //break;
                }
            }
        }
        
        internal int ReceiveExtend(byte t, JpegDecoderCore decoder)
        {
            if (UnreadBits < t)
            {
                var errorCode = EnsureNBits(t, decoder);
                if (errorCode != JpegDecoderCore.ErrorCodes.NoError)
                {
                    throw new JpegDecoderCore.MissingFF00Exception();
                }
            }

            UnreadBits -= t;
            Mask >>= t;
            int s = 1 << t;
            int x = (int)((Accumulator >> UnreadBits) & (s - 1));

            if (x < (s >> 1))
            {
                x += ((-1) << t) + 1;
            }

            return x;
        }


    }
}