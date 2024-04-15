using System;
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Formulas;
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

        public override bool IsEmpty
        {
            get { return lowerBound.Value > upperBound.Value; }
        }

        #endregion

        #region Implicit conversions

        public static implicit operator BoundedIntegerType(ConstantBoundedInteger boundedInteger)
        {
            return new TermBoundedInteger(boundedInteger.lowerBound.DeepCopy(), boundedInteger.upperBound.DeepCopy());
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a deep copy of the current type.
        /// </summary>
        /// <returns>The created deep copy of the type.</returns>
        public override ConstantBoundedInteger DeepCopy()
        {
            return new ConstantBoundedInteger(this);
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override ConstantBoundedInteger Evaluated()
        {
            return new ConstantBoundedInteger(this);
        }

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
        public override bool IsValueOutOfRange(int value)
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
        public override bool IsValueValid(int value)
        {
            return LowerBoundValue <= value && value <= UpperBoundValue;
        }

        /// <summary>
        /// Determines whether the assigned type is directly assignable to the given type.
        /// </summary>
        /// <param name="assignedType">The type to assign.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the assigned type is directly assignable.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool TypeAssignable(Type assignedType) => assignedType switch
        {
            ConstantBoundedInteger bounded => bounded.LowerBoundValue >= LowerBoundValue &&
                                              bounded.UpperBoundValue <= UpperBoundValue,

            _ => assignedType is ZeroOrOne && 0 <= LowerBoundValue && UpperBoundValue <= 1
        };

        /// <summary>
        /// Creates a formula, that represents the type constraint on the given term.
        /// </summary>
        /// <param name="term">The term to formulate the constraint on.</param>
        /// <returns>The formulated constraint on the term.</returns>
        public override Formula TypeConstraintOn(IntegerTypeTerm term)
        {
            IntegerConstant lower = new IntegerConstant(lowerBound);
            IntegerConstant upper = new IntegerConstant(upperBound);

            IntegerTypeTerm firstCopy  = term.DeepCopy();
            IntegerTypeTerm secondCopy = term.DeepCopy();

            LessThanOrEqualTo lowerBoundConstraint = new LessThanOrEqualTo(lower, firstCopy);
            LessThanOrEqualTo upperBoundConstraint = new LessThanOrEqualTo(secondCopy, upper);

            return new ConjunctionFormula(lowerBoundConstraint, upperBoundConstraint);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is equal to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is ConstantBoundedInteger other &&
                   lowerBound.Equals(other.lowerBound) &&
                   upperBound.Equals(other.upperBound);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Calculates the intersection of the two integer types.
        /// </summary>
        /// <param name="other">The other argument of the intersection.</param>
        /// <returns>The intersection of the types.</returns>
        public override IntegerType? Intersection(IntegerType other)
        {
            const int naturalNumberLowerBound   = 0;
            const int positiveIntegerLowerBound = 1;

            if (other is Integer)
            {
                return DeepCopy();
            }
            
            if (other is NaturalNumber or PositiveInteger or ZeroOrOne or ConstantBoundedInteger)
            {
                int numbersetLowerBound =
                    other is PositiveInteger or ZeroOrOne ? positiveIntegerLowerBound :
                    other is ConstantBoundedInteger bound1 ? bound1.LowerBoundValue : naturalNumberLowerBound;

                int numbersetUpperBound =
                    other is ZeroOrOne ? positiveIntegerLowerBound :
                    other is ConstantBoundedInteger bound2 ? bound2.UpperBoundValue : int.MaxValue;

                int lowerBound = Math.Max(LowerBoundValue, numbersetLowerBound);
                int upperBound = Math.Min(UpperBoundValue, numbersetUpperBound);

                bool emptyIntersection =
                    UpperBoundValue < numbersetLowerBound || LowerBoundValue > numbersetUpperBound;

                return emptyIntersection ? null : new ConstantBoundedInteger(lowerBound, upperBound);
            }

            if (other is TermBoundedInteger bounded)
            {
                IntegerTypeTerm otherLowerBound = bounded.LowerBound.DeepCopy();
                IntegerTypeTerm otherUpperBound = bounded.UpperBound.DeepCopy();

                Formula chooseOtherLower = new LessThanOrEqualTo(lowerBound.DeepCopy(), otherLowerBound).Evaluated();
                Formula chooseOtherUpper = new LessThanOrEqualTo(otherUpperBound, upperBound.DeepCopy()).Evaluated();
                Formula chooseThisLower  = new LessThanOrEqualTo(otherLowerBound, lowerBound.DeepCopy()).Evaluated();
                Formula chooseThisUpper  = new LessThanOrEqualTo(upperBound.DeepCopy(), otherUpperBound).Evaluated();

                var possibleBounds = new List<(IntegerTypeTerm, IntegerTypeTerm, Formula, Formula)>
                {
                    (lowerBound.DeepCopy(), upperBound.DeepCopy(), chooseThisLower , chooseThisUpper ),
                    (otherLowerBound      , upperBound.DeepCopy(), chooseOtherLower, chooseThisUpper ),
                    (lowerBound.DeepCopy(), otherUpperBound      , chooseThisLower , chooseOtherUpper),
                    (otherLowerBound      , otherUpperBound      , chooseOtherLower, chooseOtherUpper)
                };

                foreach ((IntegerTypeTerm lower, IntegerTypeTerm upper, Formula lowerResult, Formula upperResult) bounds in possibleBounds)
                {
                    bool chooseTheseBounds = bounds.lowerResult is TRUE && bounds.upperResult is TRUE;

                    if (chooseTheseBounds)
                    {
                        return new TermBoundedInteger(bounds.lower, bounds.upper);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Calculates the intersection of the two integer types.
        /// </summary>
        /// <param name="other">The other argument of the intersection.</param>
        /// <returns>The intersection of the types.</returns>
        public override IntegerType? Union(IntegerType other)
        {
            const int naturalNumberLowerBound   = 0;
            const int positiveIntegerLowerBound = 1;

            if (other is Integer)
            {
                return Integer.Instance();
            }

            IntegerTypeTerm otherLowerBound;
            IntegerTypeTerm otherUpperBound;

            if (other is BoundedIntegerType bounded)
            {
                otherLowerBound = bounded.LowerBound.DeepCopy();
                otherUpperBound = bounded.UpperBound.DeepCopy();
            }
            else if (other is ZeroOrOne)
            {
                otherLowerBound = new IntegerConstant(naturalNumberLowerBound);
                otherUpperBound = new IntegerConstant(positiveIntegerLowerBound);
            }
            else
            {
                return null;
            }

            return UnionBounds(lowerBound.DeepCopy(), upperBound.DeepCopy(), otherLowerBound, otherUpperBound);
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
