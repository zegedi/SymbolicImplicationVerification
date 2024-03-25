using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;

namespace SymbolicImplicationVerification.Formulas.Operations
{
    public class NegationFormula : UnaryOperationFormula
    {
        #region Constructors

        public NegationFormula(NegationFormula negation)
            : base(negation.identifier, negation.operand.DeepCopy()) { }

        public NegationFormula(Formula operand) : base(null, operand) { }

        public NegationFormula(string? identifier, Formula operand) : base(identifier, operand) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated()
        {
            return operand.Evaluated().Negated();
        }

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return operand.DeepCopy();
        }

        /// <summary>
        /// Creates a deep copy of the current negation formula.
        /// </summary>
        /// <returns>The created deep copy of the negation formula.</returns>
        public override NegationFormula DeepCopy()
        {
            return new NegationFormula(this);
        }

        #endregion
    }
}
