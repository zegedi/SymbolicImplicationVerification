using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Programs;
using SymbolicImplicationVerification.Terms;

namespace SymbolicImplicationVerification.Converts.Tokens.Operands
{
    internal class FormulaOperand : Operand
    {
        #region Fields

        private readonly Formula formula;

        #endregion

        #region Constructors

        public FormulaOperand(Formula formula)
        {
            this.formula = formula.DeepCopy();
        }

        #endregion

        #region Public properties

        public Formula Formula
        {
            get { return formula; }
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            return formula.ToLatex();
        }

        public override void TryGetOperand(out IntegerTypeTerm? result)
        {
            result = null;
        }

        public override void TryGetOperand(out LogicalTerm? result)
        {
            result = new FormulaTerm(formula.DeepCopy());
        }

        public override void TryGetOperand(out Formula? result)
        {
            result = formula.DeepCopy();
        }

        public override void TryGetOperand(out Program? result)
        {
            result = null;
        }

        #endregion
    }
}
