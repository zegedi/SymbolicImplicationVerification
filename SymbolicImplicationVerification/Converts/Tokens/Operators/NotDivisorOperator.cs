using SymbolicImplicationVerification.Converts.Tokens.Operands;
using SymbolicImplicationVerification.Formulas.Relations;

namespace SymbolicImplicationVerification.Converts.Tokens.Operators
{
    internal class NotDivisorOperator : BinaryOperator
    {
        #region Fields

        /// <summary>
        /// The singular instance of the NotDivisorOperator class.
        /// </summary>
        private static NotDivisorOperator? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private NotDivisorOperator() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular NotDivisorOperator instance.</returns>
        public static NotDivisorOperator Instance()
        {
            if (instance is null)
            {
                instance = new NotDivisorOperator();
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
            return "\\nmid";
        }

        /// <summary>
        /// Returns the precedence of the not divisor operator.
        /// </summary>
        /// <returns>The precedence of the not divisor operator.</returns>
        public override int Precedence()
        {
            return relationalPrecedence;
        }

        /// <summary>
        /// Determines whether the not divisor operator is left associative.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> - since the not divisor operator is left associative.
        /// </returns>
        public override bool LeftAssociative()
        {
            return true;
        }

        /// <summary>
        /// Determines whether the not divisor operator is right associative.
        /// </summary>
        /// <returns>
        /// <see langword="false"/> - since the not divisor operator is left associative.
        /// </returns>
        public override bool RightAssociative()
        {
            return false;
        }

        /// <summary>
        /// Creates an evaluated token from the given left and right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the binary operation.</param>
        /// <param name="rightOperand">The right operand of the binary operation.</param>
        /// <returns>The token that represents the evaluation.</returns>
        public override FormulaOperand Evaluated(Operand leftOperand, Operand rightOperand)
        {
            return FormulaEvaluated(
                leftOperand, rightOperand, (left, right) => new NotDivisor(left.DeepCopy(), right.DeepCopy())
            );
        }

        #endregion
    }
}
