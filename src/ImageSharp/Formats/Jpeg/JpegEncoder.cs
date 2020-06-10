// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Formats.Jpeg
{
    /// <summary>
    /// Encoder for writing the data image to a stream in jpeg format.
    /// </summary>
    public sealed class JpegEncoder : IImageEncoder, IJpegEncoderOptions
    {
        /// <summary>
        /// Gets or sets the quality, that will be used to encode the image. Quality
        /// index must be between 0 and 100 (compression from max to min).
        /// Defaults to <value>75</value>.
        /// </summary>
        public int? Quality { get; set; }

        /// <summary>
        /// Gets or sets the subsample ration, that will be used to encode the image.
        /// </summary>
        public JpegSubsample? Subsample { get; set; }

        /// <summary>
        /// Encodes the image to the specified stream from the <see cref="Image{TPixel}"/>.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="image">The <see cref="Image{TPixel}"/> to encode from.</param>
        /// <param name="stream">The <see cref="Stream"/> to encode the image data to.</param>
        public void Encode<TPixel>(Image<TPixel> image, Stream stream)
        where TPixel : unmanaged, IPixel<TPixel>
        {
            var encoder = new JpegEncoderCore(this);
            encoder.Encode(image, stream);
        }

        /// <summary>
        /// Encodes the image to the specified stream from the <see cref="Image{TPixel}"/>.
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="image">The <see cref="Image{TPixel}"/> to encode from.</param>
        /// <param name="stream">The <see cref="Stream"/> to encode the image data to.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var encoder = new JpegEncoderCore(this);

            if (stream.CanSeek)
            {
                encoder.Encode(image, stream);
            }
            else
            {
                // this hack has to be be here because JpegEncoderCore is unsafe
                using (var ms = new MemoryStream())
                {
                    encoder.Encode(image, ms);
                    ms.Position = 0;
                    await ms.CopyToAsync(stream).ConfigureAwait(false);
                }
            }
        }
    }
}
