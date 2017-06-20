﻿// <copyright file="Image.FromStream.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp
{
#if !NETSTANDARD1_1
    using System;
    using System.IO;
    using Formats;
    using ImageSharp.PixelFormats;

    /// <content>
    /// Adds static methods allowing the creation of new image from a given file.
    /// </content>
    public static partial class Image
    {
        /// <summary>
        /// Create a new instance of the <see cref="Image{Rgba32}"/> class from the given file.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <returns>A new <see cref="Image{Rgba32}"/>.</returns>
        public static Image<Rgba32> Load(string path) => Load<Rgba32>(path);

        /// <summary>
        /// Create a new instance of the <see cref="Image{Rgba32}"/> class from the given file.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <param name="mimeType">the mime type of the decoded image.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <returns>A new <see cref="Image{Rgba32}"/>.</returns>
        public static Image<Rgba32> Load(string path, out string mimeType) => Load<Rgba32>(path, out mimeType);

        /// <summary>
        /// Create a new instance of the <see cref="Image{Rgba32}"/> class from the given file.
        /// </summary>
        /// <param name="config">The config for the decoder.</param>
        /// <param name="path">The file path to the image.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <returns>A new <see cref="Image{Rgba32}"/>.</returns>
        public static Image<Rgba32> Load(Configuration config, string path) => Load<Rgba32>(config, path);

        /// <summary>
        /// Create a new instance of the <see cref="Image{Rgba32}"/> class from the given file.
        /// </summary>
        /// <param name="config">The config for the decoder.</param>
        /// <param name="path">The file path to the image.</param>
        /// <param name="mimeType">the mime type of the decoded image.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <returns>A new <see cref="Image{Rgba32}"/>.</returns>
        public static Image<Rgba32> Load(Configuration config, string path, out string mimeType) => Load<Rgba32>(config, path, out mimeType);

        /// <summary>
        /// Create a new instance of the <see cref="Image{Rgba32}"/> class from the given file.
        /// </summary>
        /// <param name="config">The Configuration.</param>
        /// <param name="path">The file path to the image.</param>
        /// <param name="decoder">The decoder.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <returns>A new <see cref="Image{Rgba32}"/>.</returns>
        public static Image<Rgba32> Load(Configuration config, string path, IImageDecoder decoder) => Load<Rgba32>(config, path, decoder);

        /// <summary>
        /// Create a new instance of the <see cref="Image{Rgba32}"/> class from the given file.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <param name="decoder">The decoder.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <returns>A new <see cref="Image{Rgba32}"/>.</returns>
        public static Image<Rgba32> Load(string path, IImageDecoder decoder) => Load<Rgba32>(path, decoder);

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the given file.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> Load<TPixel>(string path)
            where TPixel : struct, IPixel<TPixel>
        {
            return Load<TPixel>(null, path);
        }

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the given file.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <param name="mimeType">the mime type of the decoded image.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> Load<TPixel>(string path, out string mimeType)
            where TPixel : struct, IPixel<TPixel>
        {
            return Load<TPixel>(null, path, out mimeType);
        }

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the given file.
        /// </summary>
        /// <param name="config">The configuration options.</param>
        /// <param name="path">The file path to the image.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> Load<TPixel>(Configuration config, string path)
            where TPixel : struct, IPixel<TPixel>
        {
            config = config ?? Configuration.Default;
            using (Stream s = config.FileSystem.OpenRead(path))
            {
                return Load<TPixel>(config, s);
            }
        }

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the given file.
        /// </summary>
        /// <param name="config">The configuration options.</param>
        /// <param name="path">The file path to the image.</param>
        /// <param name="mimeType">the mime type of the decoded image.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> Load<TPixel>(Configuration config, string path, out string mimeType)
            where TPixel : struct, IPixel<TPixel>
        {
            config = config ?? Configuration.Default;
            using (Stream s = config.FileSystem.OpenRead(path))
            {
                return Load<TPixel>(config, s, out mimeType);
            }
        }

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the given file.
        /// </summary>
        /// <param name="path">The file path to the image.</param>
        /// <param name="decoder">The decoder.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> Load<TPixel>(string path, IImageDecoder decoder)
            where TPixel : struct, IPixel<TPixel>
        {
            return Load<TPixel>(null, path, decoder);
        }

        /// <summary>
        /// Create a new instance of the <see cref="Image{TPixel}"/> class from the given file.
        /// </summary>
        /// <param name="config">The Configuration.</param>
        /// <param name="path">The file path to the image.</param>
        /// <param name="decoder">The decoder.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the stream is not readable nor seekable.
        /// </exception>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <returns>A new <see cref="Image{TPixel}"/>.</returns>
        public static Image<TPixel> Load<TPixel>(Configuration config, string path, IImageDecoder decoder)
            where TPixel : struct, IPixel<TPixel>
        {
            config = config ?? Configuration.Default;
            using (Stream s = config.FileSystem.OpenRead(path))
            {
                return Load<TPixel>(config, s, decoder);
            }
        }
    }
#endif
}