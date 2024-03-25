using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Terms;
using System;

namespace SymbolicImplicationVerification.Types
{
    public abstract class IntegerType : Type, IValueValidator<int>
    {
        #region Public methods

        /// <summary>
        /// Determines whether the assigned type is compatible with the given type.
        /// </summary>
        /// <param name="assignType">The type to assign.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the assign type is an <see cref="IntegerType"/>.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool TypeCompatible(Type assignType)
        {
            return assignType is IntegerType;
        }

        /// <summary>
        /// Determines whether the given <see cref="object"/>? value is out of range for the given type.
        /// </summary>
        /// <param name="value">The <see cref="object"/>? value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is out of range.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool IsValueOutOfRange(object? value)
        {
            return value is null ||
                   value is not int intValue ||
                   IsValueOutOfRange(intValue);
        }

        /// <summary>
        /// Determines whether the given <see cref="object"/>? value is valid for the given type.
        /// </summary>
        /// <param name="value">The <see cref="object"/>? value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is valid.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool IsValueValid(object? value)
        {
            return value is not null &&
                   value is int intValue &&
                   IsValueValid(intValue);
        }

        /// <summary>
        /// Creates a formula, that represents the type constraint on the given term.
        /// </summary>
        /// <param name="term">The term to formulate on.</param>
        /// <returns>The formulated constraint on the term.</returns>
        public override Formula TypeConstraintOn(TypeTerm term)
        {
            object objectTerm = term;

            return objectTerm is IntegerTypeTerm integerTypeTerm ?
                   TypeConstraintOn(integerTypeTerm) : FALSE.Instance();
        }

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

        /// <summary>
        /// Creates a deep copy of the current integer type.
        /// </summary>
        /// <returns>The created deep copy of the integer type.</returns>
        public abstract override IntegerType DeepCopy();

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is out of range for the given type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is out of range.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public abstract bool IsValueOutOfRange(int value);

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is valid for the given type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is valid.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public abstract bool IsValueValid(int value);

        /// <summary>
        /// Creates a formula, that represents the type constraint on the given term.
        /// </summary>
        /// <param name="term">The term to formulate the constraint on.</param>
        /// <returns>The formulated constraint on the term.</returns>
        public abstract Formula TypeConstraintOn(IntegerTypeTerm term);

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
