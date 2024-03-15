using System;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;

namespace SymbolicImplicationVerification.Types
{
    public class ConstantBoundedInteger : BoundedInteger<IntegerConstant, IntegerType, IntegerConstant, IntegerType>
    {
        #region Constructors

        public ConstantBoundedInteger(IntegerConstant lowerBound, IntegerConstant upperBound)
            : base(new IntegerConstant(lowerBound), new IntegerConstant(upperBound)) { }

        public ConstantBoundedInteger(int lowerBound, int upperBound)
            : base(new IntegerConstant(lowerBound), new IntegerConstant(upperBound)) { }

        public ConstantBoundedInteger(IntegerConstant lowerBound, int upperBound)
            : base(new IntegerConstant(lowerBound), new IntegerConstant(upperBound)) { }

        public ConstantBoundedInteger(int lowerBound, IntegerConstant upperBound)
            : base(new IntegerConstant(lowerBound), new IntegerConstant(upperBound)) { }

        public ConstantBoundedInteger(ConstantBoundedInteger constantBoundend)
            : base(new IntegerConstant(constantBoundend.lowerBound), 
                   new IntegerConstant(constantBoundend.upperBound)) { }

        #endregion

        #region Public properties

        public override IntegerConstant LowerBound
        {
            get { return new IntegerConstant(lowerBound); }
            set { lowerBound = new IntegerConstant(value); }
        }

        public override IntegerConstant UpperBound
        {
            get { return new IntegerConstant(upperBound); }
            set { upperBound = new IntegerConstant(value); }
        }

        public int LowerBoundValue
        {
            get { return lowerBound.Value; }
            set { lowerBound.Value =  value; }
        }

        public int UpperBoundValue
        {
            get { return upperBound.Value; }
            set { upperBound.Value =  value; }
        }

        public bool IsEmpty
        {
            get { return lowerBound.Value > upperBound.Value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is out of range for the <see cref="ConstantBoundedInteger"/> type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is outside the bounds.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public bool IsValueOutOfRange(int value)
        {
            return value < LowerBoundValue || UpperBoundValue < value;
        }

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is valid for the <see cref="ConstantBoundedInteger"/> type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is inclusively within the bounds.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public bool IsValueValid(int value)
        {
            return LowerBoundValue <= value && value <= UpperBoundValue;
        }


        /*========================= Addition result type selection ==========================*/

        /// <summary>
        /// Determines the result type of an addition, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType AdditionWithType(int rightOperandValue)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return new ConstantBoundedInteger(LowerBoundValue + rightOperandValue, UpperBoundValue + rightOperandValue);
        }

        /// <summary>
        /// Determines the result type of an addition, with an <see cref="Integer"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType AdditionWithType(Integer rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="ConstantBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        /// <exception cref="InvalidOperationException">
        ///   If the left or right operand's <see cref="ConstantBoundedInteger"/> value set is empty.
        /// </exception>
        public override IntegerType AdditionWithType(ConstantBoundedInteger rightOperand)
        {
            if (IsEmpty || rightOperand.IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return new ConstantBoundedInteger(
                LowerBoundValue + rightOperand.LowerBoundValue,
                UpperBoundValue + rightOperand.UpperBoundValue
            );
        }

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="TermBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType AdditionWithType(TermBoundedInteger rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="NaturalNumber"/> type right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType AdditionWithType(NaturalNumber rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return LowerBoundValue switch
            {
                > 0 => PositiveInteger.Instance(),    // The left operand is a positive integer.
                  0 => NaturalNumber.Instance(),      // The left operand is a natural number.
                  _ => Integer.Instance(),            // The left operand could be a negative integer.
            };
        }

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="PositiveInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType AdditionWithType(PositiveInteger rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return LowerBoundValue switch
            {
                > -1 => PositiveInteger.Instance(),    // The left operand is a natural number.
                  -1 => NaturalNumber.Instance(),      // The left operand is an integer greater than or equal to -1.
                   _ => Integer.Instance(),            // The left operand can be a negative integer less than -1.
            };
        }

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="ZeroOrOne"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType AdditionWithType(ZeroOrOne rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return (LowerBoundValue, UpperBoundValue) switch
            {
                (   0, 0) => ZeroOrOne.Instance(),        // The left operand can only be zero.
                (>= 1, _) => PositiveInteger.Instance(),  // The left operand is a positive integer.
                (   0, _) => NaturalNumber.Instance(),    // The left operand is a natural number.
                (   _, _) => Integer.Instance()           // The left operand is an integer (could be negative).
            };
        }


        /*========================= Subtraction result type selection ==========================*/

        /// <summary>
        /// Determines the result type of a subtraction, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType SubtractionWithType(int rightOperandValue)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return new ConstantBoundedInteger(LowerBoundValue - rightOperandValue, UpperBoundValue - rightOperandValue);
        }

        /// <summary>
        /// Determines the result type of a subtraction, with an <see cref="Integer"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType SubtractionWithType(Integer rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="ConstantBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        /// <exception cref="InvalidOperationException">
        ///   If the left or right operand's <see cref="ConstantBoundedInteger"/> value set is empty.
        /// </exception>
        public override IntegerType SubtractionWithType(ConstantBoundedInteger rightOperand)
        {
            if (IsEmpty || rightOperand.IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return new ConstantBoundedInteger(
                LowerBoundValue - rightOperand.UpperBoundValue,
                UpperBoundValue - rightOperand.LowerBoundValue
            );
        }

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="TermBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType SubtractionWithType(TermBoundedInteger rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="NaturalNumber"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType SubtractionWithType(NaturalNumber rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="PositiveInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType SubtractionWithType(PositiveInteger rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a subtraction, with a <see cref="ZeroOrOne"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType SubtractionWithType(ZeroOrOne rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return new ConstantBoundedInteger(LowerBoundValue - 1, UpperBoundValue);
        }


        /*========================= Multiplication result type selection ==========================*/

        /// <summary>
        /// Determines the result type of a multiplication, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType MultiplicationWithType(int rightOperandValue)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            bool rightOperandNegative = rightOperandValue < 0;

            int newLowerBound = (rightOperandNegative ? UpperBoundValue : LowerBoundValue) * rightOperandValue;
            int newUpperBound = (rightOperandNegative ? LowerBoundValue : UpperBoundValue) * rightOperandValue;

            return new ConstantBoundedInteger(newLowerBound, newUpperBound);
        }

        /// <summary>
        /// Determines the result type of a multiplication, with an <see cref="Integer"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType MultiplicationWithType(Integer rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="ConstantBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        /// <exception cref="InvalidOperationException">
        ///   If the left or right operand's <see cref="ConstantBoundedInteger"/> value set is empty.
        /// </exception>
        public override IntegerType MultiplicationWithType(ConstantBoundedInteger rightOperand)
        {
            if (IsEmpty || rightOperand.IsEmpty)
            {
                throw new InvalidOperationException();
            }

            int[] possibleBounds =
            {
                LowerBoundValue * rightOperand.LowerBoundValue,
                LowerBoundValue * rightOperand.UpperBoundValue,
                UpperBoundValue * rightOperand.LowerBoundValue,
                UpperBoundValue * rightOperand.UpperBoundValue
            };

            return new ConstantBoundedInteger(possibleBounds.Min(), possibleBounds.Max());
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="TermBoundedInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType MultiplicationWithType(TermBoundedInteger rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="NaturalNumber"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType MultiplicationWithType(NaturalNumber rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return (LowerBoundValue, UpperBoundValue) switch
            {
                (   0, 0) => ZeroOrOne.Instance(),      // The left operand can only be zero.
                (>= 0, _) => NaturalNumber.Instance(),  // The left operand is a natural number.
                (   _, _) => Integer.Instance()         // The left operand is an integer.
            };
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="PositiveInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType MultiplicationWithType(PositiveInteger rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return (LowerBoundValue, UpperBoundValue) switch
            {
                (   0, 0) => ZeroOrOne.Instance(),          // The left operand can only be zero.
                (>= 1, _) => PositiveInteger.Instance(),    // The left operand is a positive integer.
                (   0, _) => NaturalNumber.Instance(),      // The left operand is a natural number.
                (   _, _) => Integer.Instance()             // The left operand is an integer.
            };
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="ZeroOrOne"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        /// <exception cref="InvalidOperationException">If the <see cref="ConstantBoundedInteger"/> value set is empty.</exception>
        public override IntegerType MultiplicationWithType(ZeroOrOne rightOperand)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            return (LowerBoundValue, UpperBoundValue) switch
            {
                (>= 0, <= 1) => ZeroOrOne.Instance(),      // The left operand is zero or one.
                (>= 0,    _) => NaturalNumber.Instance(),  // The left operand is a natural number.
                (   _,    _) => Integer.Instance()         // The left operand is an integer.
            };
        }

        #endregion
    }
}
