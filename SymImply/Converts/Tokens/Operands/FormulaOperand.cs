using SymImply.Formulas;
using SymImply.Programs;
using SymImply.Terms;

namespace SymImply.Converts.Tokens.Operands
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

        public override bool TryGetOperand(out IntegerTypeTerm? result)
        {
            result = null;

            return false;
        }

        public override bool TryGetOperand(out LogicalTerm? result)
        {
            result = new FormulaTerm(formula.DeepCopy());

            return true;
        }

        public override bool TryGetOperand(out Formula? result)
        {
            result = formula.DeepCopy();

            return true;
        }

        public override bool TryGetOperand(out Program? result)
        {
            result = null;

            return false;
        }

        #endregion
    }
}
