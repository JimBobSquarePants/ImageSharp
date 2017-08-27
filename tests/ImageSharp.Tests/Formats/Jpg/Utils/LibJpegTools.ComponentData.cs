namespace SixLabors.ImageSharp.Tests.Formats.Jpg.Utils
{
    using System;
    using System.Linq;
    using System.Numerics;

    using SixLabors.ImageSharp.Formats.Jpeg.Common;
    using SixLabors.ImageSharp.Formats.Jpeg.GolangPort.Components.Decoder;
    using SixLabors.ImageSharp.Formats.Jpeg.PdfJsPort.Components;
    using SixLabors.ImageSharp.Memory;
    using SixLabors.Primitives;

    internal static partial class LibJpegTools
    {
        public class ComponentData : IEquatable<ComponentData>, IJpegComponent
        {
            public ComponentData(int heightInBlocks, int widthInBlocks, int index)
            {
                this.HeightInBlocks = heightInBlocks;
                this.WidthInBlocks = widthInBlocks;
                this.Index = index;
                this.Blocks = new Buffer2D<Block8x8>(this.WidthInBlocks, this.HeightInBlocks);
            }

            public Size Size => new Size(this.WidthInBlocks, this.HeightInBlocks);

            public int Index { get; }

            public int HeightInBlocks { get; }
            
            public int WidthInBlocks { get; }

            public int HorizontalSamplingFactor => throw new NotSupportedException();

            public int VerticalSamplingFactor => throw new NotSupportedException();

            public Buffer2D<Block8x8> Blocks { get; private set; }

            public short MinVal { get; private set; } = short.MaxValue;

            public short MaxVal { get; private set; } = short.MinValue;
            
            internal void MakeBlock(short[] data, int y, int x)
            {
                this.MinVal = Math.Min((short)this.MinVal, data.Min());
                this.MaxVal = Math.Max((short)this.MaxVal, data.Max());
                this.Blocks[x, y] = new Block8x8(data);
            }

            public static ComponentData Load(PdfJsFrameComponent c, int index)
            {
                var result = new ComponentData(
                    c.BlocksPerColumnForMcu,
                    c.BlocksPerLineForMcu,
                    index
                );

                for (int y = 0; y < result.HeightInBlocks; y++)
                {
                    for (int x = 0; x < result.WidthInBlocks; x++)
                    {
                        short[] data = c.GetBlockBuffer(y, x).ToArray();
                        result.MakeBlock(data, y, x);
                    }
                }

                return result;
            }

            public static ComponentData Load(OrigComponent c)
            {
                var result = new ComponentData(
                    c.HeightInBlocks,
                    c.WidthInBlocks,
                    c.Index
                );

                for (int y = 0; y < result.HeightInBlocks; y++)
                {
                    for (int x = 0; x < result.WidthInBlocks; x++)
                    {
                        short[] data = c.GetBlockReference(x, y).ToArray();
                        result.MakeBlock(data, y, x);
                    }
                }

                return result;
            }

            public Image<Rgba32> CreateGrayScaleImage()
            {
                Image<Rgba32> result = new Image<Rgba32>(this.WidthInBlocks * 8, this.HeightInBlocks * 8);
                
                for (int by = 0; by < this.HeightInBlocks; by++)
                {
                    for (int bx = 0; bx < this.WidthInBlocks; bx++)
                    {
                        this.WriteToImage(bx, by, result);
                    }
                }
                return result;
            }

            internal void WriteToImage(int bx, int by, Image<Rgba32> image)
            {
                Block8x8 block = this.Blocks[bx, by];
                
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        var val = this.GetBlockValue(block, x, y);

                        Vector4 v = new Vector4(val, val, val, 1);
                        Rgba32 color = default(Rgba32);
                        color.PackFromVector4(v);

                        int yy = by * 8 + y;
                        int xx = bx * 8 + x;
                        image[xx, yy] = color;
                    }
                }
            }

            internal float GetBlockValue(Block8x8 block, int x, int y)
            {
                float d = (this.MaxVal - this.MinVal);
                float val = block[y, x];
                val -= this.MinVal;
                val /= d;
                return val;
            }

            public bool Equals(ComponentData other)
            {
                if (Object.ReferenceEquals(null, other)) return false;
                if (Object.ReferenceEquals(this, other)) return true;
                bool ok = this.Index == other.Index && this.HeightInBlocks == other.HeightInBlocks
                          && this.WidthInBlocks == other.WidthInBlocks;
                //&& this.MinVal == other.MinVal
                //&& this.MaxVal == other.MaxVal;
                if (!ok) return false;

                for (int y = 0; y < this.HeightInBlocks; y++)
                {
                    for (int x = 0; x < this.WidthInBlocks; x++)
                    {
                        Block8x8 a = this.Blocks[x, y];
                        Block8x8 b = other.Blocks[x, y];
                        if (!a.Equals(b)) return false;
                    }
                }
                return true;
            }

            public override bool Equals(object obj)
            {
                if (Object.ReferenceEquals(null, obj)) return false;
                if (Object.ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return this.Equals((ComponentData)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = this.Index;
                    hashCode = (hashCode * 397) ^ this.HeightInBlocks;
                    hashCode = (hashCode * 397) ^ this.WidthInBlocks;
                    hashCode = (hashCode * 397) ^ this.MinVal.GetHashCode();
                    hashCode = (hashCode * 397) ^ this.MaxVal.GetHashCode();
                    return hashCode;
                }
            }

            public static bool operator ==(ComponentData left, ComponentData right)
            {
                return Object.Equals(left, right);
            }

            public static bool operator !=(ComponentData left, ComponentData right)
            {
                return !Object.Equals(left, right);
            }
        }
    }
}