using SymbolicImplicationVerification.Term;

namespace SymbolicImplicationVerification.Formula
{
    public class NegationFormula : UnaryOperationFormula
    {
        #region Constructors

        public NegationFormula(Formula operand) : this(null, operand) { }

        public NegationFormula(string? identifier, Formula operand) : base(identifier, operand) { }

        #endregion
    }
}
