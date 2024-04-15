using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

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
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            bool noParenthesis =
                operand is LogicalTermFormula logicalTerm &&
                logicalTerm.Argumentum is Variable<Logical> or LogicalConstant;

            return string.Format(noParenthesis ? "\\neg {0}" : "\\neg ({0})", operand);
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated()
        {
            Formula result = operand.Negated();

            if (result is NegationFormula)
            {
                result = new NegationFormula(operand.Evaluated());
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is equal to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is NegationFormula other && operand.Equals(other.operand);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
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
