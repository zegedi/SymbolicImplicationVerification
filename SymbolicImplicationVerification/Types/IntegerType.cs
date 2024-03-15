using SymbolicImplicationVerification.Terms;
using System;

namespace SymbolicImplicationVerification.Types
{
    public abstract class IntegerType : Type
    {
        #region Public methods

        /// <summary>
        /// Determines the result type of an addition, with an <see cref="IntegerType"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public IntegerType AdditionWithType(IntegerType rightOperand)
        {
            return AdditionWithType((dynamic) rightOperand);
        }

        /// <summary>
        /// Determines the result type of an addition, with a constant left and right <see cref="int"/> operand.
        /// </summary>
        /// <param name="leftOperandValue">The constant left operand of the addition.</param>
        /// <param name="rightOperandValue">The constant right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public IntegerType AdditionWithType(int leftOperandValue, int rightOperandValue)
        {
            return IntegerTypeSelector(leftOperandValue + rightOperandValue);
        }

        /// <summary>
        /// Determines the result type of a subtraction, with an <see cref="IntegerType"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public IntegerType SubtractionWithType(IntegerType rightOperand)
        {
            return SubtractionWithType((dynamic) rightOperand);
        }

        /// <summary>
        /// Determines the result type of a subtraction, with a constant left and right <see cref="int"/> operand.
        /// </summary>
        /// <param name="leftOperandValue">The constant left operand of the subtraction.</param>
        /// <param name="rightOperandValue">The constant right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public IntegerType SubtractionWithType(int leftOperandValue, int rightOperandValue)
        {
            return IntegerTypeSelector(leftOperandValue - rightOperandValue);
        }

        /// <summary>
        /// Determines the result type of a multiplication, with an <see cref="IntegerType"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public IntegerType MultiplicationWithType(IntegerType rightOperand)
        {
            return MultiplicationWithType((dynamic) rightOperand);
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a constant left and right <see cref="int"/> operand.
        /// </summary>
        /// <param name="leftOperandValue">The constant left operand of the multiplication.</param>
        /// <param name="rightOperandValue">The constant right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public IntegerType MultiplicationWithType(int leftOperandValue, int rightOperandValue)
        {
            return IntegerTypeSelector(leftOperandValue * rightOperandValue);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Determines the <see cref="IntegerType"/> of the given <see cref="int"/> value.
        /// </summary>
        /// <param name="value">The value to operate on.</param>
        /// <returns>The <see cref="IntegerType"/> of the value.</returns>
        private IntegerType IntegerTypeSelector(int value) => value switch
        {
            0 or 1 => ZeroOrOne.Instance(),
              >= 2 => PositiveInteger.Instance(),
                 _ => Integer.Instance()
        };

        #endregion

        #region Public abstract methods

        /*========================= Addition result type selection ==========================*/

        /// <summary>
        /// Determines the result type of an addition, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public abstract IntegerType AdditionWithType(int rightOperandValue);

        /// <summary>
        /// Determines the result type of an addition, with an <see cref="Integer"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public abstract IntegerType AdditionWithType(Integer rightOperand);

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="ConstantBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public abstract IntegerType AdditionWithType(ConstantBoundedInteger rightOperand);

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="TermBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public abstract IntegerType AdditionWithType(TermBoundedInteger rightOperand);

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="NaturalNumber"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public abstract IntegerType AdditionWithType(NaturalNumber rightOperand);

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="PositiveInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public abstract IntegerType AdditionWithType(PositiveInteger rightOperand);

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="ZeroOrOne"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public abstract IntegerType AdditionWithType(ZeroOrOne rightOperand);


        /*========================= Subtraction result type selection ==========================*/

        /// <summary>
        /// Determines the result type of a subtraction, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public abstract IntegerType SubtractionWithType(int rightOperandValue);

        /// <summary>
        /// Determines the result type of a subtraction, with an <see cref="Integer"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public abstract IntegerType SubtractionWithType(Integer rightOperand);

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="ConstantBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public abstract IntegerType SubtractionWithType(ConstantBoundedInteger rightOperand);

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="TermBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public abstract IntegerType SubtractionWithType(TermBoundedInteger rightOperand);

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="NaturalNumber"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public abstract IntegerType SubtractionWithType(NaturalNumber rightOperand);

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="PositiveInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public abstract IntegerType SubtractionWithType(PositiveInteger rightOperand);

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="ZeroOrOne"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public abstract IntegerType SubtractionWithType(ZeroOrOne rightOperand);


        /*========================= Multiplication result type selection ==========================*/

        /// <summary>
        /// Determines the result type of a multiplication, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public abstract IntegerType MultiplicationWithType(int rightOperandValue);

        /// <summary>
        /// Determines the result type of a multiplication, with an <see cref="Integer"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public abstract IntegerType MultiplicationWithType(Integer rightOperand);

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="ConstantBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public abstract IntegerType MultiplicationWithType(ConstantBoundedInteger rightOperand);

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="TermBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public abstract IntegerType MultiplicationWithType(TermBoundedInteger rightOperand);

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="NaturalNumber"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public abstract IntegerType MultiplicationWithType(NaturalNumber rightOperand);

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="PositiveInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public abstract IntegerType MultiplicationWithType(PositiveInteger rightOperand);

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="ZeroOrOne"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public abstract IntegerType MultiplicationWithType(ZeroOrOne rightOperand);

        #endregion
    }
}
