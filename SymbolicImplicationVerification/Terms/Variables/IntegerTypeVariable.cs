using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Variables
{
    public class IntegerTypeVariable : Variable<IntegerType>
    {
        #region Constructors

        public IntegerTypeVariable(IntegerTypeVariable variable) 
            : this(variable.identifier, variable.termType) { }

        public IntegerTypeVariable(string identifier, IntegerType termType) : base(identifier, termType) { }

        #endregion

        #region Public static operators

        /// <summary>
        /// Creates a new addition between a variable left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(IntegerTypeVariable leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new addition between a variable left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the addition.</param>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The created addition instance.</returns>
        public static Addition operator +(IntegerTypeVariable leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Addition(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between a variable left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(IntegerTypeVariable leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new subtraction between a variable left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the subtraction.</param>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The created subtraction instance.</returns>
        public static Subtraction operator -(IntegerTypeVariable leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Subtraction(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new multiplication between a variable left and a constant right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(IntegerTypeVariable leftOperand, IntegerTypeConstant rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        /// <summary>
        /// Creates a new multiplication between a variable left and an IntegerTypeTerm right operand.
        /// </summary>
        /// <param name="leftOperand">The left operand of the multiplication.</param>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The created multiplication instance.</returns>
        public static Multiplication operator *(IntegerTypeVariable leftOperand, IntegerTypeTerm rightOperand)
        {
            return new Multiplication(leftOperand, rightOperand);
        }

        #endregion
    }
}
