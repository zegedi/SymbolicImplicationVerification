using SymbolicImplicationVerification.Converts.Tokens.Operands;
using SymbolicImplicationVerification.Formulas.Relations;

namespace SymbolicImplicationVerification.Converts.Tokens.Operators
{
    internal class LessThanOperator : BinaryOperator
    {
        #region Fields

        /// <summary>
        /// The singular instance of the LessThanOperator class.
        /// </summary>
        private static LessThanOperator? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private LessThanOperator() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular LessThanOperator instance.</returns>
        public static LessThanOperator Instance()
        {
            if (instance is null)
            {
                instance = new LessThanOperator();
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
            return "<";
        }

        /// <summary>
        /// Returns the precedence of the less than operator.
        /// </summary>
        /// <returns>The precedence of the less than operator.</returns>
        public override int Precedence()
        {
            return relationalPrecedence;
        }

        /// <summary>
        /// Determines whether the less than operator is left associative.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> - since the less than operator is left associative.
        /// </returns>
        public override bool LeftAssociative()
        {
            return true;
        }

        /// <summary>
        /// Determines whether the less than operator is right associative.
        /// </summary>
        /// <returns>
        /// <see langword="false"/> - since the less than operator is left associative.
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
                leftOperand, rightOperand, (left, right) => new LessThan(left.DeepCopy(), right.DeepCopy())
            );
        }

        #endregion
    }
}
