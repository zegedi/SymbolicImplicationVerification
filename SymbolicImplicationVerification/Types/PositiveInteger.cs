using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Terms.Constants;
using System;

namespace SymbolicImplicationVerification.Types
{
    public class PositiveInteger : IntegerType, IValueValidator<int>
    {
        #region Fields

        /// <summary>
        /// The singular instance of the <see cref="PositiveInteger"/> class.
        /// </summary>
        private static PositiveInteger? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private PositiveInteger() : base() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular <see cref="PositiveInteger"/> instance.
        /// </summary>
        /// <returns>The singular <see cref="PositiveInteger"/> instance.</returns>
        public static PositiveInteger Instance()
        {
            if (instance is null)
            {
                instance = new PositiveInteger();
            }

            return instance;
        }

        /// <summary>
        /// Destroy method for the singular <see cref="PositiveInteger"/> instance.
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
            return @"\posN";
        }

        /// <summary>
        /// Creates a deep copy of the current type.
        /// </summary>
        /// <returns>The created deep copy of the type.</returns>
        public override PositiveInteger DeepCopy()
        {
            return PositiveInteger.Instance();
        }

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is out of range
        /// for the <see cref="PositiveInteger"/> type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is less than or equal to zero.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool IsValueOutOfRange(int value)
        {
            return value <= 0;
        }

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is valid
        /// for the <see cref="PositiveInteger"/> type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is greather than zero.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool IsValueValid(int value)
        {
            return value > 0;
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
                IntegerTypeTerm     lowerBound => lowerBound.TermType is PositiveInteger
            },

            _ => assignedType is PositiveInteger
        };

        /// <summary>
        /// Creates a program, that represents the type constraint on the given term.
        /// </summary>
        /// <param name="term">The term to formulate the constraint on.</param>
        /// <returns>The formulated constraint on the term.</returns>
        public override Formula TypeConstraintOn(IntegerTypeTerm term)
        {
            IntegerTypeConstant positiveIntegerLowerBound = new IntegerTypeConstant(1);
            IntegerTypeTerm copyTerm = term.DeepCopy();

            return new GreaterThanOrEqualTo(copyTerm, positiveIntegerLowerBound);
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
            return obj is PositiveInteger;
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
            const int positiveIntegerLowerBound = 1;

            if (other is Integer or NaturalNumber or PositiveInteger)
            {
                return PositiveInteger.Instance();
            }

            if (other is ZeroOrOne)
            {
                return other.DeepCopy();
            }

            if (other is ConstantBoundedInteger constantBounded)
            {
                bool notEmptyIntersection = constantBounded.UpperBoundValue >= positiveIntegerLowerBound;

                if (notEmptyIntersection)
                {
                    int lowerBound = Math.Max(constantBounded.LowerBoundValue, positiveIntegerLowerBound);

                    return new ConstantBoundedInteger(lowerBound, constantBounded.UpperBoundValue);
                }
            }

            if (other is TermBoundedInteger bounded)
            {
                return IntersectionBounds(
                    bounded.LowerBound.DeepCopy(), 
                    bounded.UpperBound.DeepCopy(), 
                    new IntegerTypeConstant(positiveIntegerLowerBound)
                );

                //IntegerTypeTerm otherLowerBound = bounded.LowerBound.DeepCopy();
                //IntegerTypeTerm otherUpperBound = bounded.UpperBound.DeepCopy();

                //IntegerConstant positiveIntegerLower = new IntegerConstant(positiveIntegerLowerBound);

                //Formula chooseOtherLower = new LessThanOrEqualTo(positiveIntegerLower, otherLowerBound).Evaluated();
                //Formula chooseThisLower  = new LessThanOrEqualTo(otherLowerBound, positiveIntegerLower).Evaluated();

                //Formula emptyIntersection = new LessThan(otherUpperBound, positiveIntegerLower).Evaluated();

                //var possibleBounds = new List<(IntegerTypeTerm, Formula)>
                //{
                //    (otherLowerBound     , chooseThisLower ),
                //    (positiveIntegerLower, chooseOtherLower)
                //};

                //foreach ((IntegerTypeTerm lower, Formula lowerResult) bounds in possibleBounds)
                //{
                //    bool chooseThisLowerBound = bounds.lowerResult is     TRUE;
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
            const int positiveIntegerLowerBound = 0;

            if (other is Integer or NaturalNumber)
            {
                return other.DeepCopy();
            }

            if (other is PositiveInteger or ZeroOrOne)
            {
                return NaturalNumber.Instance();
            }

            if (other is BoundedIntegerType bounded)
            {
                Formula subsetCondition = new LessThanOrEqualTo(
                    new IntegerTypeConstant(positiveIntegerLowerBound), bounded.LowerBound.DeepCopy()).Evaluated();

                return subsetCondition is TRUE ? NaturalNumber.Instance() : null;
            }

            return null;
        }

        /*========================= Addition result type selection ==========================*/

        /// <summary>
        /// Determines the result type of an addition, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public override IntegerType AdditionWithType(int rightOperandValue) => rightOperandValue switch
        {
            > -1 => PositiveInteger.Instance(),  // The right operand is a natural number.
              -1 => NaturalNumber.Instance(),    // The right operand is -1.
               _ => Integer.Instance()           // The rigth operand is less than -1.
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
                > -1 => PositiveInteger.Instance(),    // The right operand is a natural number.
                  -1 => NaturalNumber.Instance(),      // The right operand is an integer greater than or equal to -1.
                   _ => Integer.Instance(),            // The right operand can be a negative integer less than -1.
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
            return PositiveInteger.Instance();
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
            return PositiveInteger.Instance();
        }


        /*========================= Subtraction result type selection ==========================*/

        /// <summary>
        /// Determines the result type of a subtraction, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public override IntegerType SubtractionWithType(int rightOperandValue) => rightOperandValue switch
        {
            < 1 => PositiveInteger.Instance(),  // The right operand is a non positive integer.
              1 => NaturalNumber.Instance(),    // The right operand is 1.
              _ => Integer.Instance(),          // The right operand is an integer greater than 1.
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
                < 1 => PositiveInteger.Instance(),  // The right operand a non positive integer.
                  1 => NaturalNumber.Instance(),    // The right operand is a non positive integer or one.
                  _ => Integer.Instance(),          // The right operand could be a positive integer greater than one.
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
            return NaturalNumber.Instance();
        }


        /*========================= Multiplication result type selection ==========================*/

        /// <summary>
        /// Determines the result type of a multiplication, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public override IntegerType MultiplicationWithType(int rightOperandValue) => rightOperandValue switch
        {
            > 0 => PositiveInteger.Instance(),  // The right operand is a positive integer.
              0 => ZeroOrOne.Instance(),        // The right operand is zero.
              _ => Integer.Instance()           // The right operand is a negativ integer.
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
                (   0, 0) => ZeroOrOne.Instance(),          // The right operand can only be zero.
                (>= 1, _) => PositiveInteger.Instance(),    // The right operand is a positive integer.
                (   0, _) => NaturalNumber.Instance(),      // The right operand is a natural number.
                (   _, _) => Integer.Instance()             // The right operand is an integer.
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
            return PositiveInteger.Instance();
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
