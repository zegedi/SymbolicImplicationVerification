using SymbolicImplicationVerification.Formulas.Operations;
using System;

namespace SymbolicImplicationVerification.Formulas
{
    public abstract class UnaryOperationFormula : Formula
    {
        #region Fields

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

        public Formula Operand
        {
            get { return operand; }
            set { operand = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Creates a deep copy of the current unary operation formula.
        /// </summary>
        /// <returns>The created deep copy of the unary operation formula.</returns>
        public override abstract UnaryOperationFormula DeepCopy();

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the specified formula is equivalent to the current formula.
        /// </summary>
        /// <param name="other">The formula to compare with the current formula.</param>
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
