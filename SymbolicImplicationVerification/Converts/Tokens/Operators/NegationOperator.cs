using SymbolicImplicationVerification.Converts.Tokens.Operands;
using SymbolicImplicationVerification.Formulas.Operations;

namespace SymbolicImplicationVerification.Converts.Tokens.Operators
{
    internal class NegationOperator : UnaryOperator
    {
        #region Fields

        /// <summary>
        /// The singular instance of the NegationOperator class.
        /// </summary>
        private static NegationOperator? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private NegationOperator() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular NegationOperator instance.</returns>
        public static NegationOperator Instance()
        {
            if (instance is null)
            {
                instance = new NegationOperator();
            }

            return instance;
        }

        /// <summary>
        /// Destroy method for the singular instance.
        /// </summary>
        public static void Destroy()
        {
            if (instance is not null)
            {
                instance = null;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return "\\neg";
        }

        /// <summary>
        /// Returns the precedence of the negation operator.
        /// </summary>
        /// <returns>The precedence of the negation operator.</returns>
        public override int Precedence()
        {
            return unaryPrecedence;
        }

        /// <summary>
        /// Determines whether the negation operator is left associative.
        /// </summary>
        /// <returns>
        /// <see langword="false"/> - since the negation operator is right associative.
        /// </returns>
        public override bool LeftAssociative()
        {
            return false;
        }

        /// <summary>
        /// Determines whether the negation operator is right associative.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> - since the negation operator is right associative.
        /// </returns>
        public override bool RightAssociative()
        {
            return true;
        }

        /// <summary>
        /// Creates an evaluated token from the operand.
        /// </summary>
        /// <param name="operand">The operand of the unary operation.</param>
        /// <returns>The token that represents the evaluation.</returns>
        public override FormulaOperand Evaluated(Operand operand)
        {
            return FormulaEvaluated(operand, operand => new NegationFormula(operand.DeepCopy()));
        }

        #endregion
    }
}
