using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification
{
    public interface IArithmetic<LTerm, RTerm>
        where LTerm : Term<IntegerType>
        where RTerm : Term<IntegerType>
    {
        /// <summary>
        /// Default addition implementation for two <see cref="IntegerType"/> terms.
        /// </summary>
        /// <param name="leftOperand">The left operand term of the addition.</param>
        /// <param name="rightOperand">The right operand term of the addition.</param>
        /// <returns>The result term of the addition.</returns>
        public Addition Add(LTerm leftOperand, RTerm rightOperand)
        {
            // Get the result type of the addition.
            IntegerType resultType = leftOperand.TermType.AdditionWithType((dynamic) rightOperand.TermType);

            return new Addition(leftOperand, rightOperand, resultType);
        }

        /// <summary>
        /// Default subtraction implementation for two <see cref="IntegerType"/> terms.
        /// </summary>
        /// <param name="leftOperand">The left operand term of the subtraction.</param>
        /// <param name="rightOperand">The right operand term of the subtraction.</param>
        /// <returns>The result term of the subtraction.</returns>
        public Subtraction Subtract(LTerm leftOperand, RTerm rightOperand)
        {
            // Get the result type of the subtraction.
            IntegerType resultType = leftOperand.TermType.SubtractionWithType((dynamic) rightOperand.TermType);

            return new Subtraction(leftOperand, rightOperand, resultType);
        }

        /// <summary>
        /// Default multiplication implementation for two <see cref="IntegerType"/> terms.
        /// </summary>
        /// <param name="leftOperand">The left operand term of the multiplication.</param>
        /// <param name="rightOperand">The right operand term of the multiplication.</param>
        /// <returns>The result term of the multiplication.</returns>
        public Multiplication Multiply(LTerm leftOperand, RTerm rightOperand)
        {
            // Get the result type of the multiplication.
            IntegerType resultType = leftOperand.TermType.MultiplicationWithType((dynamic) rightOperand.TermType);

            return new Multiplication(leftOperand, rightOperand, resultType);
        }
    }
}
