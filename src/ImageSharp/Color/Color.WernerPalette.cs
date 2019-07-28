// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;

namespace SixLabors.ImageSharp
{
    /// <content>
    /// Contains the definition of <see cref="WernerPalette"/>.
    /// </content>
    public partial struct Color
    {
        private static readonly Lazy<Color[]> WernerPaletteLazy = new Lazy<Color[]>(CreateWernerPalette, true);

        /// <summary>
        /// Gets a collection of colors as defined in the original second edition of Werner’s Nomenclature of Colours 1821.
        /// The hex codes were collected and defined by Nicholas Rougeux <see href="https://www.c82.net/werner"/>.
        /// </summary>
        public static ReadOnlyMemory<Color> WernerPalette => WernerPaletteLazy.Value;

        private static Color[] CreateWernerPalette() => new[]
        {
            FromHex("#f1e9cd"),
            FromHex("#f2e7cf"),
            FromHex("#ece6d0"),
            FromHex("#f2eacc"),
            FromHex("#f3e9ca"),
            FromHex("#f2ebcd"),
            FromHex("#e6e1c9"),
            FromHex("#e2ddc6"),
            FromHex("#cbc8b7"),
            FromHex("#bfbbb0"),
            FromHex("#bebeb3"),
            FromHex("#b7b5ac"),
            FromHex("#bab191"),
            FromHex("#9c9d9a"),
            FromHex("#8a8d84"),
            FromHex("#5b5c61"),
            FromHex("#555152"),
            FromHex("#413f44"),
            FromHex("#454445"),
            FromHex("#423937"),
            FromHex("#433635"),
            FromHex("#252024"),
            FromHex("#241f20"),
            FromHex("#281f3f"),
            FromHex("#1c1949"),
            FromHex("#4f638d"),
            FromHex("#383867"),
            FromHex("#5c6b8f"),
            FromHex("#657abb"),
            FromHex("#6f88af"),
            FromHex("#7994b5"),
            FromHex("#6fb5a8"),
            FromHex("#719ba2"),
            FromHex("#8aa1a6"),
            FromHex("#d0d5d3"),
            FromHex("#8590ae"),
            FromHex("#3a2f52"),
            FromHex("#39334a"),
            FromHex("#6c6d94"),
            FromHex("#584c77"),
            FromHex("#533552"),
            FromHex("#463759"),
            FromHex("#bfbac0"),
            FromHex("#77747f"),
            FromHex("#4a475c"),
            FromHex("#b8bfaf"),
            FromHex("#b2b599"),
            FromHex("#979c84"),
            FromHex("#5d6161"),
            FromHex("#61ac86"),
            FromHex("#a4b6a7"),
            FromHex("#adba98"),
            FromHex("#93b778"),
            FromHex("#7d8c55"),
            FromHex("#33431e"),
            FromHex("#7c8635"),
            FromHex("#8e9849"),
            FromHex("#c2c190"),
            FromHex("#67765b"),
            FromHex("#ab924b"),
            FromHex("#c8c76f"),
            FromHex("#ccc050"),
            FromHex("#ebdd99"),
            FromHex("#ab9649"),
            FromHex("#dbc364"),
            FromHex("#e6d058"),
            FromHex("#ead665"),
            FromHex("#d09b2c"),
            FromHex("#a36629"),
            FromHex("#a77d35"),
            FromHex("#f0d696"),
            FromHex("#d7c485"),
            FromHex("#f1d28c"),
            FromHex("#efcc83"),
            FromHex("#f3daa7"),
            FromHex("#dfa837"),
            FromHex("#ebbc71"),
            FromHex("#d17c3f"),
            FromHex("#92462f"),
            FromHex("#be7249"),
            FromHex("#bb603c"),
            FromHex("#c76b4a"),
            FromHex("#a75536"),
            FromHex("#b63e36"),
            FromHex("#b5493a"),
            FromHex("#cd6d57"),
            FromHex("#711518"),
            FromHex("#e9c49d"),
            FromHex("#eedac3"),
            FromHex("#eecfbf"),
            FromHex("#ce536b"),
            FromHex("#b74a70"),
            FromHex("#b7757c"),
            FromHex("#612741"),
            FromHex("#7a4848"),
            FromHex("#3f3033"),
            FromHex("#8d746f"),
            FromHex("#4d3635"),
            FromHex("#6e3b31"),
            FromHex("#864735"),
            FromHex("#553d3a"),
            FromHex("#613936"),
            FromHex("#7a4b3a"),
            FromHex("#946943"),
            FromHex("#c39e6d"),
            FromHex("#513e32"),
            FromHex("#8b7859"),
            FromHex("#9b856b"),
            FromHex("#766051"),
            FromHex("#453b32")
        };
    }
}