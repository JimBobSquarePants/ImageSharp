﻿namespace ImageSharp
{
    /// <summary>
    /// A formula based curve segment
    /// </summary>
    internal sealed class IccFormulaCurveElement : IccCurveSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IccFormulaCurveElement"/> class.
        /// </summary>
        /// <param name="type">The type of this segment</param>
        /// <param name="gamma">Gamma segment parameter</param>
        /// <param name="a">A segment parameter</param>
        /// <param name="b">B segment parameter</param>
        /// <param name="c">C segment parameter</param>
        /// <param name="d">D segment parameter</param>
        /// <param name="e">E segment parameter</param>
        public IccFormulaCurveElement(IccFormulaCurveType type, double gamma, double a, double b, double c, double d, double e)
            : base(IccCurveSegmentSignature.FormulaCurve)
        {
            this.Type = type;
            this.Gamma = gamma;
            this.A = a;
            this.B = b;
            this.C = c;
            this.D = d;
            this.E = e;
        }

        /// <summary>
        /// Gets the type of this curve
        /// </summary>
        public IccFormulaCurveType Type { get; }

        /// <summary>
        /// Gets the gamma curve parameter
        /// </summary>
        public double Gamma { get; }

        /// <summary>
        /// Gets the A curve parameter
        /// </summary>
        public double A { get; }

        /// <summary>
        /// Gets the B curve parameter
        /// </summary>
        public double B { get; }

        /// <summary>
        /// Gets the C curve parameter
        /// </summary>
        public double C { get; }

        /// <summary>
        /// Gets the D curve parameter
        /// </summary>
        public double D { get; }

        /// <summary>
        /// Gets the E curve parameter
        /// </summary>
        public double E { get; }

        /// <inheritdoc />
        public override bool Equals(IccCurveSegment other)
        {
            if (base.Equals(other) && other is IccFormulaCurveElement segment)
            {
                return this.Type == segment.Type
                    && this.Gamma == segment.Gamma
                    && this.A == segment.A
                    && this.B == segment.B
                    && this.C == segment.C
                    && this.D == segment.D
                    && this.E == segment.E;
            }

            return false;
        }
    }
}
