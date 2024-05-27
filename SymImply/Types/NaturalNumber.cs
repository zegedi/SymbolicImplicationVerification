using SymImply.Formulas;
using SymImply.Formulas.Relations;
using SymImply.Terms.Constants;
using System;

namespace SymImply.Types
{
    public class NaturalNumber : IntegerType, IValueValidator<int>
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

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string? ToString()
        {
            return @"\N";
        }

        /// <summary>
        /// Creates a deep copy of the current type.
        /// </summary>
        /// <returns>The created deep copy of the type.</returns>
        public override NaturalNumber DeepCopy()
        {
            return NaturalNumber.Instance();
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
        public override bool IsValueOutOfRange(int value)
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
        public override bool IsValueValid(int value)
        {
            return value >= 0;
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
            ConstantBoundedInteger bounded => bounded.LowerBoundValue >= 0,

            TermBoundedInteger bounded => bounded.LowerBound switch
            {
                IntegerTypeConstant lowerBound => lowerBound.Value >= 0,
                IntegerTypeTerm     lowerBound => lowerBound.TermType is 
                                                  NaturalNumber or PositiveInteger or ZeroOrOne
            },

            _ => assignedType is NaturalNumber or PositiveInteger or ZeroOrOne
        };

        /// <summary>
        /// Creates a program, that represents the type constraint on the given term.
        /// </summary>
        /// <param name="term">The term to formulate the constraint on.</param>
        /// <returns>The formulated constraint on the term.</returns>
        public override Formula TypeConstraintOn(IntegerTypeTerm term)
        {
            IntegerTypeConstant naturalNumberLowerBound = new IntegerTypeConstant(0);
            IntegerTypeTerm copyTerm = term.DeepCopy();

            return new GreaterThanOrEqualTo(copyTerm, naturalNumberLowerBound);
        }

        /// <summary>
        /// Calculates the intersection of the two integer types.
        /// </summary>
        /// <param name="other">The other argument of the intersection.</param>
        /// <returns>The intersection of the types.</returns>
        public override IntegerType? Intersection(IntegerType other)
        {
            const int naturalNumberLowerBound = 0;

            if (other is Integer or NaturalNumber)
            {
                return NaturalNumber.Instance();
            }

            if (other is PositiveInteger or ZeroOrOne)
            {
                return other.DeepCopy();
            }

            if (other is ConstantBoundedInteger constantBounded)
            {
                bool notEmptyIntersection = constantBounded.UpperBoundValue >= naturalNumberLowerBound;

                if (notEmptyIntersection)
                {
                    int lowerBound = Math.Max(constantBounded.LowerBoundValue, naturalNumberLowerBound);

                    return new ConstantBoundedInteger(lowerBound, constantBounded.UpperBoundValue);
                }
            }

            if (other is TermBoundedInteger bounded)
            {
                return IntersectionBounds(
                    bounded.LowerBound.DeepCopy(),
                    bounded.UpperBound.DeepCopy(),
                    new IntegerTypeConstant(naturalNumberLowerBound)
                );

                //IntegerTypeTerm otherLowerBound = bounded.LowerBound.DeepCopy();
                //IntegerTypeTerm otherUpperBound = bounded.UpperBound.DeepCopy();

                //IntegerConstant naturalNumberLower = new IntegerConstant(naturalNumberLowerBound);

                //Formula chooseOtherLower = new LessThanOrEqualTo(naturalNumberLower, otherLowerBound).Evaluated();
                //Formula chooseThisLower  = new LessThanOrEqualTo(otherLowerBound, naturalNumberLower).Evaluated();

                //Formula emptyIntersection = new LessThan(otherUpperBound, naturalNumberLower).Evaluated();

                //var possibleBounds = new List<(IntegerTypeTerm, Formula)>
                //{
                //    (otherLowerBound   , chooseThisLower ),
                //    (naturalNumberLower, chooseOtherLower)
                //};

                //foreach ((IntegerTypeTerm lower, Formula lowerResult) bounds in possibleBounds)
                //{
                //    bool chooseThisLowerBound = bounds.lowerResult is TRUE && bounds.lowerResult is TRUE;
                //    bool notEmptyIntersection = emptyIntersection  is not TRUE;

                //    if (chooseThisLowerBound && notEmptyIntersection)
                //    {
                //        return new TermBoundedInteger(bounds.lower, otherUpperBound);
                //    }
                //}
            }

            return null;
        }

        /// <summary>
        /// Calculates the union of the two integer types.
        /// </summary>
        /// <param name="other">The other argument of the union.</param>
        /// <returns>The union of the types.</returns>
        public override IntegerType? Union(IntegerType other)
        {
            const int naturalNumberLowerBound = 0;

            if (other is Integer)
            {
                return Integer.Instance();
            }

            if (other is NaturalNumber or PositiveInteger or ZeroOrOne)
            {
                return NaturalNumber.Instance();
            }

            if (other is BoundedIntegerType bounded)
            {
                Formula subsetCondition = new LessThanOrEqualTo(
                    new IntegerTypeConstant(naturalNumberLowerBound), bounded.LowerBound.DeepCopy()).Evaluated();

                return subsetCondition is TRUE ? NaturalNumber.Instance() : null;
            }

            return null;
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
            return obj is NaturalNumber;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /*========================= Addition result type selection ==========================*/

        /// <summary>
        /// Determines the result type of an addition, with a lowerBoundConstant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The lowerBoundConstant right operand of the addition.</param>
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
        /// Determines the result type of a subtraction, with a lowerBoundConstant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The lowerBoundConstant right operand of the subtraction.</param>
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
        /// Determines the result type of a multiplication, with a lowerBoundConstant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The lowerBoundConstant right operand of the multiplication.</param>
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
