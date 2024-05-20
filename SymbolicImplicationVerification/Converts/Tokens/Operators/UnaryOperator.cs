using SymbolicImplicationVerification.Converts.Tokens.Operands;
using SymbolicImplicationVerification.Formulas;

namespace SymbolicImplicationVerification.Converts.Tokens.Operators
{
    internal abstract class UnaryOperator : Operator
    {
        #region Public abstract methods

        /// <summary>
        /// Creates an evaluated token from the operand.
        /// </summary>
        /// <param name="operand">The operand of the unary operation.</param>
        /// <returns>The token that represents the evaluation.</returns>
        public abstract Operand Evaluated(Operand operand);

        #endregion

        #region Protected methods

        protected FormulaOperand FormulaEvaluated(Operand operand, Func<Formula, Formula> createFormula)
        {
            operand.TryGetOperand(out Formula? formula);

            if (formula is not null)
            {
                return new FormulaOperand(createFormula(formula));
            }

            throw new ConvertException($"Nem elvégezhető művelet: \"{ToString()} {operand}\".");
        }

        #endregion
    }
}
