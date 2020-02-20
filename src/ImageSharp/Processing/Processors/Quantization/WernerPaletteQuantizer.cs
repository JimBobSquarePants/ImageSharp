// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

namespace SixLabors.ImageSharp.Processing.Processors.Quantization
{
    /// <summary>
    /// A palette quantizer consisting of colors as defined in the original second edition of Werner’s Nomenclature of Colours 1821.
    /// The hex codes were collected and defined by Nicholas Rougeux <see href="https://www.c82.net/werner"/>
    /// </summary>
    public class WernerPaletteQuantizer : PaletteQuantizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WernerPaletteQuantizer" /> class.
        /// </summary>
        public WernerPaletteQuantizer()
            : this(new QuantizerOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WernerPaletteQuantizer" /> class.
        /// </summary>
        /// <param name="options">The quantizer options defining quantization rules.</param>
        public WernerPaletteQuantizer(QuantizerOptions options)
            : base(Color.WernerPalette, options)
        {
        }
    }
}
