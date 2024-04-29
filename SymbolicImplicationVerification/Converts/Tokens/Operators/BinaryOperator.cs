using SymbolicImplicationVerification.Converts.Tokens.Operands;
using SymbolicImplicationVerification.Formulas;

namespace SymbolicImplicationVerification.Converts.Tokens.Operators
{
    internal abstract class BinaryOperator : Operator
    {
        #region Fields

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Creates an evaluated token from the given integerTypeLeft and integerTypeRight operand.
        /// </summary>
        /// <param name="leftOperand">The integerTypeLeft operand of the binary operation.</param>
        /// <param name="rightOperand">The integerTypeRight operand of the binary operation.</param>
        /// <returns>The token that represents the evaluation.</returns>
        public abstract Operand Evaluated(Operand leftOperand, Operand rightOperand);

        #endregion

        #region Protected methods

        protected TermOperand TermEvaluated(
            Operand leftOperand, Operand rightOperand, 
            Func<IntegerTypeTerm, IntegerTypeTerm, IntegerTypeTerm> createTerm)
        {
            leftOperand .TryGetOperand(out IntegerTypeTerm? left);
            rightOperand.TryGetOperand(out IntegerTypeTerm? right);

            if (left is not null && right is not null)
            {
                return new TermOperand(createTerm(left, right));
            }

            throw new ConvertException($"Nem elvégezhető művelet: \"{leftOperand} {ToString()} {rightOperand}\"");
        }

        protected FormulaOperand FormulaEvaluated(
            Operand leftOperand, Operand rightOperand, 
            Func<IntegerTypeTerm, IntegerTypeTerm, Formula> createFormula)
        {
            leftOperand .TryGetOperand(out IntegerTypeTerm? left);
            rightOperand.TryGetOperand(out IntegerTypeTerm? right);

            if (left is not null && right is not null)
            {
                return new FormulaOperand(createFormula(left, right));
            }

            throw new ConvertException($"Nem elvégezhető művelet: \"{leftOperand} {ToString()} {rightOperand}\"");
        }

        protected FormulaOperand FormulaEvaluated(
            Operand leftOperand, Operand rightOperand, Func<LogicalTerm, LogicalTerm, Formula> createFormula)
        {
            leftOperand .TryGetOperand(out LogicalTerm? left);
            rightOperand.TryGetOperand(out LogicalTerm? right);

            if (left is not null && right is not null)
            {
                return new FormulaOperand(createFormula(left, right));
            }

            throw new ConvertException($"Nem elvégezhető művelet: \"{leftOperand} {ToString()} {rightOperand}\"");
        }

        protected FormulaOperand FormulaEvaluated(
            Operand leftOperand, Operand rightOperand, Func<Formula, Formula, Formula> createFormula)
        {
            leftOperand .TryGetOperand(out Formula? left);
            rightOperand.TryGetOperand(out Formula? right);

            if (left is not null && right is not null)
            {
                return new FormulaOperand(createFormula(left, right));
            }

            throw new ConvertException($"Nem elvégezhető művelet: \"{leftOperand} {ToString()} {rightOperand}\"");
        }

        protected FormulaOperand FormulaEvaluated(
            Operand leftOperand, Operand rightOperand,
            Func<IntegerTypeTerm, IntegerTypeTerm, Formula> createIntegerFormula,
            Func<LogicalTerm, LogicalTerm, Formula> createLogicalFormula)
        {
            leftOperand .TryGetOperand(out IntegerTypeTerm? integerTypeLeft);
            rightOperand.TryGetOperand(out IntegerTypeTerm? integerTypeRight);

            if (integerTypeLeft is not null && integerTypeRight is not null)
            {
                return new FormulaOperand(createIntegerFormula(integerTypeLeft, integerTypeRight));
            }

            leftOperand .TryGetOperand(out LogicalTerm? logicalLeft);
            rightOperand.TryGetOperand(out LogicalTerm? logicalRight);

            if (logicalLeft is not null && logicalRight is not null)
            {
                return new FormulaOperand(createLogicalFormula(logicalLeft, logicalRight));
            }

            throw new ConvertException($"Nem elvégezhető művelet: \"{leftOperand} {ToString()} {rightOperand}\"");
        }

        #endregion
    }
}
