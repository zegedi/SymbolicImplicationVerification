using SymbolicImplicationVerification.Formulas.Operations;
using System;

namespace SymbolicImplicationVerification.Formulas
{
    public abstract class UnaryOperationFormula : Formula
    {
        #region Fields

        /// <summary>
        /// The operand of the unary operation.
        /// </summary>
        protected Formula operand;

        #endregion

        #region Constructors

        public UnaryOperationFormula(Formula operand) : this(null, operand) { }

        public UnaryOperationFormula(string? identifier, Formula operand) : base(identifier)
        {
            this.operand = operand;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the operand of the unary operation.
        /// </summary>
        public Formula Operand
        {
            get { return operand; }
            set { operand = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Creates a deep copy of the current unary operation program.
        /// </summary>
        /// <returns>The created deep copy of the unary operation program.</returns>
        public override abstract UnaryOperationFormula DeepCopy();

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the specified program is equivalent to the current program.
        /// </summary>
        /// <param name="other">The program to compare with the current program.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the formulas are the equivalent.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Equivalent(Formula other)
        {
            return Evaluated().Equals(other.Evaluated());
        }

        #endregion
    }
}
