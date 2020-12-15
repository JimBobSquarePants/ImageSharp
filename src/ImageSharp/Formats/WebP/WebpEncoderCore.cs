// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.IO;
using System.Threading.Tasks;

using SixLabors.ImageSharp.Formats.Experimental.Webp.Lossless;
using SixLabors.ImageSharp.Formats.Experimental.Webp.Lossy;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Formats.Experimental.Webp
{
    /// <summary>
    /// Image encoder for writing an image to a stream in the WebP format.
    /// </summary>
    internal sealed class WebpEncoderCore
    {
        /// <summary>
        /// Used for allocating memory during processing operations.
        /// </summary>
        private readonly MemoryAllocator memoryAllocator;

        /// <summary>
        /// TODO: not used at the moment.
        /// Indicating whether the alpha plane should be compressed with WebP lossless format.
        /// </summary>
        private readonly bool alphaCompression;

        /// <summary>
        /// Indicating whether lossy compression should be used. If false, lossless compression will be used.
        /// </summary>
        private readonly bool lossy;

        /// <summary>
        /// Compression quality. Between 0 and 100.
        /// </summary>
        private readonly int quality;

        /// <summary>
        /// Quality/speed trade-off (0=fast, 6=slower-better).
        /// </summary>
        private readonly int method;

        /// <summary>
        /// The number of entropy-analysis passes (in [1..10]).
        /// </summary>
        private readonly int entropyPasses;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebpEncoderCore"/> class.
        /// </summary>
        /// <param name="options">The encoder options.</param>
        /// <param name="memoryAllocator">The memory manager.</param>
        public WebpEncoderCore(IWebPEncoderOptions options, MemoryAllocator memoryAllocator)
        {
            this.memoryAllocator = memoryAllocator;
            this.alphaCompression = options.AlphaCompression;
            this.lossy = options.Lossy;
            this.quality = options.Quality;
            this.method = options.Method;
            this.entropyPasses = options.EntropyPasses;
        }

        /// <summary>
        /// Encodes the image to the specified stream from the <see cref="ImageFrame{TPixel}"/>.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="image">The <see cref="ImageFrame{TPixel}"/> to encode from.</param>
        /// <param name="stream">The <see cref="Stream"/> to encode the image data to.</param>
        public void Encode<TPixel>(Image<TPixel> image, Stream stream)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            Guard.NotNull(image, nameof(image));
            Guard.NotNull(stream, nameof(stream));

            if (this.lossy)
            {
                var enc = new Vp8Encoder(this.memoryAllocator, image.Width, image.Height, this.quality, this.method, this.entropyPasses);
                enc.Encode(image, stream);
            }
            else
            {
                var enc = new Vp8LEncoder(this.memoryAllocator, image.Width, image.Height, this.quality, this.method);
                enc.Encode(image, stream);
            }
        }

        /// <summary>
        /// Encodes the image to the specified stream from the <see cref="ImageFrame{TPixel}"/>.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="image">The <see cref="ImageFrame{TPixel}"/> to encode from.</param>
        /// <param name="stream">The <see cref="Stream"/> to encode the image data to.</param>
        public async Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (stream.CanSeek)
            {
                this.Encode(image, stream);
            }
            else
            {
                using (var ms = new MemoryStream())
                {
                    this.Encode(image, ms);
                    ms.Position = 0;
                    await ms.CopyToAsync(stream).ConfigureAwait(false);
                }
            }
        }
    }
}
