using SymImply.Converts.Tokens.Operands;
using SymImply.Terms.Operations.Binary;

namespace SymImply.Converts.Tokens.Operators
{
    internal class MultiplicationOperator : BinaryOperator
    {
        #region Fields

        /// <summary>
        /// The singular instance of the MultiplicationOperator class.
        /// </summary>
        private static MultiplicationOperator? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private MultiplicationOperator() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular MultiplicationOperator instance.</returns>
        public static MultiplicationOperator Instance()
        {
            if (instance is null)
            {
                instance = new MultiplicationOperator();
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
            return "\\cdot";
        }

        /// <summary>
        /// Returns the precedence of the multiplication operator.
        /// </summary>
        /// <returns>The precedence of the multiplication operator.</returns>
        public override int Precedence()
        {
            return multiplicativePrecedence;
        }

        /// <summary>
        /// Determines whether the multiplication operator is left associative.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> - since the multiplication operator is left associative.
        /// </returns>
        public override bool LeftAssociative()
        {
            return true;
        }

        /// <summary>
        /// Determines whether the multiplication operator is right associative.
        /// </summary>
        /// <returns>
        /// <see langword="false"/> - since the multiplication operator is left associative.
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
        public override TermOperand Evaluated(Operand leftOperand, Operand rightOperand)
        {
            return TermEvaluated(
                leftOperand, rightOperand, (left, right) => new Multiplication(left.DeepCopy(), right.DeepCopy())
            );
        }

        #endregion
    }
}
