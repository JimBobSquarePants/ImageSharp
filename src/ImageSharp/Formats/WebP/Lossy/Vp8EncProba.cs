// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;

namespace SixLabors.ImageSharp.Formats.WebP.Lossy
{
    internal class Vp8EncProba
    {
        /// <summary>
        /// Last (inclusive) level with variable cost.
        /// </summary>
        private const int MaxVariableLevel = 67;

        /// <summary>
        /// Value below which using skipProba is OK.
        /// </summary>
        private const int SkipProbaThreshold = 250;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vp8EncProba"/> class.
        /// </summary>
        public Vp8EncProba()
        {
            this.Dirty = true;
            this.UseSkipProba = false;
            this.Segments = new byte[3];
            this.Coeffs = new Vp8BandProbas[WebPConstants.NumTypes][];
            for (int i = 0; i < this.Coeffs.Length; i++)
            {
                this.Coeffs[i] = new Vp8BandProbas[WebPConstants.NumBands];
                for (int j = 0; j < this.Coeffs[i].Length; j++)
                {
                    this.Coeffs[i][j] = new Vp8BandProbas();
                }
            }

            this.Stats = new Vp8Stats[WebPConstants.NumTypes][];
            for (int i = 0; i < this.Coeffs.Length; i++)
            {
                this.Stats[i] = new Vp8Stats[WebPConstants.NumBands];
                for (int j = 0; j < this.Stats[i].Length; j++)
                {
                    this.Stats[i][j] = new Vp8Stats();
                }
            }

            this.LevelCost = new Vp8CostArray[WebPConstants.NumTypes][];
            for (int i = 0; i < this.LevelCost.Length; i++)
            {
                this.LevelCost[i] = new Vp8CostArray[WebPConstants.NumBands];
                for (int j = 0; j < this.LevelCost[i].Length; j++)
                {
                    this.LevelCost[i][j] = new Vp8CostArray();
                }
            }

            this.RemappedCosts = new Vp8CostArray[WebPConstants.NumTypes][];
            for (int i = 0; i < this.RemappedCosts.Length; i++)
            {
                this.RemappedCosts[i] = new Vp8CostArray[16];
                for (int j = 0; j < this.RemappedCosts[i].Length; j++)
                {
                    this.RemappedCosts[i][j] = new Vp8CostArray();
                }
            }

            // Initialize with default probabilities.
            this.Segments.AsSpan().Fill(255);
            for (int t = 0; t < WebPConstants.NumTypes; ++t)
            {
                for (int b = 0; b < WebPConstants.NumBands; ++b)
                {
                    for (int c = 0; c < WebPConstants.NumCtx; ++c)
                    {
                        Vp8ProbaArray dst = this.Coeffs[t][b].Probabilities[c];
                        for (int p = 0; p < WebPConstants.NumProbas; ++p)
                        {
                            dst.Probabilities[p] = WebPLookupTables.DefaultCoeffsProba[t, b, c, p];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the probabilities for segment tree.
        /// </summary>
        public byte[] Segments { get; }

        /// <summary>
        /// Gets or sets the final probability of being skipped.
        /// </summary>
        public byte SkipProba { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the skip probability. Note: we always use SkipProba for now.
        /// </summary>
        public bool UseSkipProba { get; set; }

        public Vp8BandProbas[][] Coeffs { get; }

        public Vp8Stats[][] Stats { get; }

        public Vp8CostArray[][] LevelCost { get; }

        public Vp8CostArray[][] RemappedCosts { get; }

        /// <summary>
        /// Gets or sets the number of skipped blocks.
        /// </summary>
        public int NbSkip { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether CalculateLevelCosts() needs to be called.
        /// </summary>
        public bool Dirty { get; set; }

        public void CalculateLevelCosts()
        {
            if (!this.Dirty)
            {
                return; // nothing to do.
            }

            for (int ctype = 0; ctype < WebPConstants.NumTypes; ++ctype)
            {
                for (int band = 0; band < WebPConstants.NumBands; ++band)
                {
                    for (int ctx = 0; ctx < WebPConstants.NumCtx; ++ctx)
                    {
                        Vp8ProbaArray p = this.Coeffs[ctype][band].Probabilities[ctx];
                        Span<ushort> table = this.LevelCost[ctype][band].Costs.AsSpan(ctx * MaxVariableLevel);
                        int cost0 = (ctx > 0) ? this.BitCost(1, p.Probabilities[0]) : 0;
                        int costBase = this.BitCost(1, p.Probabilities[1]) + cost0;
                        int v;
                        table[0] = (ushort)(this.BitCost(0, p.Probabilities[1]) + cost0);
                        for (v = 1; v <= MaxVariableLevel; ++v)
                        {
                            table[v] = (ushort)(costBase + this.VariableLevelCost(v, p.Probabilities));
                        }

                        // Starting at level 67 and up, the variable part of the cost is actually constant.
                    }
                }

                for (int n = 0; n < 16; ++n)
                {
                    for (int ctx = 0; ctx < WebPConstants.NumCtx; ++ctx)
                    {
                        Span<ushort> dst = this.RemappedCosts[ctype][n].Costs.AsSpan(ctx * MaxVariableLevel, MaxVariableLevel);
                        Span<ushort> src = this.LevelCost[ctype][WebPConstants.Vp8EncBands[n]].Costs.AsSpan(ctx * MaxVariableLevel, MaxVariableLevel);
                        src.CopyTo(dst);
                    }
                }
            }

            this.Dirty = false;
        }

        public int FinalizeTokenProbas()
        {
            bool hasChanged = false;
            int size = 0;
            for (int t = 0; t < WebPConstants.NumTypes; ++t)
            {
                for (int b = 0; b < WebPConstants.NumBands; ++b)
                {
                    for (int c = 0; c < WebPConstants.NumCtx; ++c)
                    {
                        for (int p = 0; p < WebPConstants.NumProbas; ++p)
                        {
                            var stats = this.Stats[t][b].Stats[c].Stats[p];
                            int nb = (int)((stats >> 0) & 0xffff);
                            int total = (int)((stats >> 16) & 0xffff);
                            int updateProba = WebPLookupTables.CoeffsUpdateProba[t, b, c, p];
                            int oldP = WebPLookupTables.DefaultCoeffsProba[t, b, c, p];
                            int newP = this.CalcTokenProba(nb, total);
                            int oldCost = this.BranchCost(nb, total, oldP) + this.BitCost(0, (byte)updateProba);
                            int newCost = this.BranchCost(nb, total, newP) + this.BitCost(1, (byte)updateProba) + (8 * 256);
                            bool useNewP = oldCost > newCost;
                            size += this.BitCost(useNewP ? 1 : 0, (byte)updateProba);
                            if (useNewP)
                            {
                                // Only use proba that seem meaningful enough.
                                this.Coeffs[t][b].Probabilities[c].Probabilities[p] = (byte)newP;
                                hasChanged |= newP != oldP;
                                size += 8 * 256;
                            }
                            else
                            {
                                this.Coeffs[t][b].Probabilities[c].Probabilities[p] = (byte)oldP;
                            }
                        }
                    }
                }
            }

            this.Dirty = hasChanged;
            return size;
        }

        public int FinalizeSkipProba(int mbw, int mbh)
        {
            int nbMbs = mbw * mbh;
            int nbEvents = this.NbSkip;
            this.SkipProba = (byte)this.CalcSkipProba(nbEvents, nbMbs);
            this.UseSkipProba = this.SkipProba < SkipProbaThreshold;

            int size = 256;
            if (this.UseSkipProba)
            {
                size += (nbEvents * this.BitCost(1, this.SkipProba)) + ((nbMbs - nbEvents) * this.BitCost(0, this.SkipProba));
                size += 8 * 256;   // cost of signaling the skipProba itself.
            }

            return size;
        }

        public void ResetTokenStats()
        {
            for (int t = 0; t < WebPConstants.NumTypes; ++t)
            {
                for (int b = 0; b < WebPConstants.NumBands; ++b)
                {
                    for (int c = 0; c < WebPConstants.NumCtx; ++c)
                    {
                        for (int p = 0; p < WebPConstants.NumProbas; ++p)
                        {
                            this.Stats[t][b].Stats[c].Stats[p] = 0;
                        }
                    }
                }
            }
        }

        private int CalcSkipProba(long nb, long total)
        {
            return (int)(total != 0 ? (total - nb) * 255 / total : 255);
        }

        private int VariableLevelCost(int level, Span<byte> probas)
        {
            int pattern = WebPLookupTables.Vp8LevelCodes[level - 1][0];
            int bits = WebPLookupTables.Vp8LevelCodes[level - 1][1];
            int cost = 0;
            for (int i = 2; pattern != 0; ++i)
            {
                if ((pattern & 1) != 0)
                {
                    cost += this.BitCost(bits & 1, probas[i]);
                }

                bits >>= 1;
                pattern >>= 1;
            }

            return cost;
        }

        // Collect statistics and deduce probabilities for next coding pass.
        // Return the total bit-cost for coding the probability updates.
        private int CalcTokenProba(int nb, int total)
        {
            return nb != 0 ? (255 - (nb * 255 / total)) : 255;
        }

        // Cost of coding 'nb' 1's and 'total-nb' 0's using 'proba' probability.
        private int BranchCost(int nb, int total, int proba)
        {
            return (nb * this.BitCost(1, (byte)proba)) + ((total - nb) * this.BitCost(0, (byte)proba));
        }

        // Cost of coding one event with probability 'proba'.
        private int BitCost(int bit, byte proba)
        {
            return bit == 0 ? WebPLookupTables.Vp8EntropyCost[proba] : WebPLookupTables.Vp8EntropyCost[255 - proba];
        }
    }
}
