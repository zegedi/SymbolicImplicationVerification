using SymImply.Converts.Tokens.Operands;
using SymImply.Formulas.Relations;

namespace SymImply.Converts.Tokens.Operators
{
    internal class NotEqualOperator : BinaryOperator
    {
        #region Fields

        /// <summary>
        /// The singular instance of the NotEqualOperator class.
        /// </summary>
        private static NotEqualOperator? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private NotEqualOperator() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular NotEqualOperator instance.</returns>
        public static NotEqualOperator Instance()
        {
            if (instance is null)
            {
                instance = new NotEqualOperator();
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
            return "\\neq";
        }

        /// <summary>
        /// Returns the precedence of the not equal operator.
        /// </summary>
        /// <returns>The precedence of the not equal operator.</returns>
        public override int Precedence()
        {
            return equalityPrecedence;
        }

        /// <summary>
        /// Determines whether the not equal operator is left associative.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> - since the not equal operator is left associative.
        /// </returns>
        public override bool LeftAssociative()
        {
            return true;
        }

        /// <summary>
        /// Determines whether the not equal operator is right associative.
        /// </summary>
        /// <returns>
        /// <see langword="false"/> - since the not equal operator is left associative.
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
                leftOperand, rightOperand,
                (left, right) => new IntegerTypeNotEqual(left.DeepCopy(), right.DeepCopy()),
                (left, right) => new LogicalNotEqual(left.DeepCopy(), right.DeepCopy())
            );
        }

        #endregion
    }
}
