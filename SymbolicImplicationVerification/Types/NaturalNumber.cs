using System;

namespace SymbolicImplicationVerification.Types
{
    public class NaturalNumber : IntegerType, IValueValidator<int>, ISingleton<NaturalNumber>
    {
        #region Fields

        /// <summary>
        /// The singular instance of the <see cref="NaturalNumber"/> class.
        /// </summary>
        private static NaturalNumber? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private NaturalNumber() : base() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular <see cref="NaturalNumber"/> instance.
        /// </summary>
        /// <returns>The singular <see cref="NaturalNumber"/> instance.</returns>
        public static NaturalNumber Instance()
        {
            if (instance is null)
            {
                instance = new NaturalNumber();
            }

            return instance;
        }

        /// <summary>
        /// Destroy method for the singular <see cref="NaturalNumber"/> instance.
        /// </summary>
        public static void Destroy()
        {
            if (instance is not null)
            {
                instance = null;
            }
        }

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is out of range for the <see cref="NaturalNumber"/> type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is less than zero.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public static bool IsValueOutOfRange(int value)
        {
            return value < 0;
        }

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is valid for the <see cref="NaturalNumber"/> type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is greather than or equal to zero.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public static bool IsValueValid(int value)
        {
            return value >= 0;
        }

        #endregion

        #region Public methods

        /*========================= Addition result type selection ==========================*/

        /// <summary>
        /// Determines the result type of an addition, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public override IntegerType AdditionWithType(int rightOperandValue) => rightOperandValue switch
        {
            > 0 => PositiveInteger.Instance(),  // The right operand is a positive integer.
              0 => NaturalNumber.Instance(),    // The right operand is zero.
              _ => Integer.Instance()           // The right operand is a negativ integer.
        };

        /// <summary>
        /// Determines the result type of an addition, with an <see cref="Integer"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public override IntegerType AdditionWithType(Integer rightOperand)
        {
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="ConstantBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        /// <exception cref="InvalidOperationException">If the right operand's value set is empty.</exception>
        public override IntegerType AdditionWithType(ConstantBoundedInteger rightOperand)
        {
            if (rightOperand.IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return rightOperand.LowerBoundValue switch
            {
                > 0 => PositiveInteger.Instance(),    // The right operand is a positive integer.
                  0 => NaturalNumber.Instance(),      // The right operand is a natural number.
                  _ => Integer.Instance(),            // The right operand could be a negative integer.
            };
        }


        /// <summary>
        /// Determines the result type of an addition, with a <see cref="TermBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public override IntegerType AdditionWithType(TermBoundedInteger rightOperand)
        {
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="NaturalNumber"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public override IntegerType AdditionWithType(NaturalNumber rightOperand)
        {
            return NaturalNumber.Instance();
        }

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="PositiveInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public override IntegerType AdditionWithType(PositiveInteger rightOperand)
        {
            return PositiveInteger.Instance();
        }

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="ZeroOrOne"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public override IntegerType AdditionWithType(ZeroOrOne rightOperand)
        {
            return NaturalNumber.Instance();
        }


        /*========================= Subtraction result type selection ==========================*/

        /// <summary>
        /// Determines the result type of a subtraction, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public override IntegerType SubtractionWithType(int rightOperandValue) => rightOperandValue switch
        {
            > 0 => Integer.Instance(),          // The right operand is a positive integer.
              0 => NaturalNumber.Instance(),    // The right operand is zero.
              _ => PositiveInteger.Instance(),  // The right operand is a negativ integer.
        };

        /// <summary>
        /// Determines the result type of a subtraction, with an <see cref="Integer"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public override IntegerType SubtractionWithType(Integer rightOperand)
        {
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="ConstantBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        /// <exception cref="InvalidOperationException">If the right operand's value set is empty.</exception>
        public override IntegerType SubtractionWithType(ConstantBoundedInteger rightOperand)
        {
            if (rightOperand.IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return rightOperand.UpperBoundValue switch
            {
                > 0 => Integer.Instance(),          // The right operand could a positive integer.
                  0 => NaturalNumber.Instance(),    // The right operand is a non positive integer.
                  _ => PositiveInteger.Instance(),  // The right operand a negative integer.
            };
        }

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="TermBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public override IntegerType SubtractionWithType(TermBoundedInteger rightOperand)
        {
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="NaturalNumber"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public override IntegerType SubtractionWithType(NaturalNumber rightOperand)
        {
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="PositiveInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public override IntegerType SubtractionWithType(PositiveInteger rightOperand)
        {
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="ZeroOrOne"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public override IntegerType SubtractionWithType(ZeroOrOne rightOperand)
        {
            return Integer.Instance();
        }


        /*========================= Multiplication result type selection ==========================*/

        /// <summary>
        /// Determines the result type of a multiplication, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public override IntegerType MultiplicationWithType(int rightOperandValue) => rightOperandValue switch
        {
            > 0 => NaturalNumber.Instance(),  // The right operand is a positive integer.
              0 => ZeroOrOne.Instance(),      // The right operand is zero.
              _ => Integer.Instance()         // The right operand is a negativ integer.
        };

        /// <summary>
        /// Determines the result type of a multiplication, with an <see cref="Integer"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public override IntegerType MultiplicationWithType(Integer rightOperand)
        {
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="ConstantBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        /// <exception cref="InvalidOperationException">If the right operand's value set is empty.</exception>
        public override IntegerType MultiplicationWithType(ConstantBoundedInteger rightOperand)
        {
            if (rightOperand.IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return (rightOperand.LowerBoundValue, rightOperand.UpperBoundValue) switch
            {
                (   0, 0) => ZeroOrOne.Instance(),      // The right operand can only be zero.
                (>= 0, _) => NaturalNumber.Instance(),  // The right operand is a natural number.
                (   _, _) => Integer.Instance()         // The right operand is an integer.
            };
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="TermBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public override IntegerType MultiplicationWithType(TermBoundedInteger rightOperand)
        {
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="NaturalNumber"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public override IntegerType MultiplicationWithType(NaturalNumber rightOperand)
        {
            return NaturalNumber.Instance();
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="PositiveInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public override IntegerType MultiplicationWithType(PositiveInteger rightOperand)
        {
            return NaturalNumber.Instance();
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="ZeroOrOne"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public override IntegerType MultiplicationWithType(ZeroOrOne rightOperand)
        {
            return NaturalNumber.Instance();
        }

        #endregion
    }
}
