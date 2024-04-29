using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Variables;
using System;

namespace SymbolicImplicationVerification.Types
{
    public class ZeroOrOne : IntegerType, ISingleton<ZeroOrOne>
    {
        #region Fields

        /// <summary>
        /// The singular instance of the <see cref="ZeroOrOne"/> class.
        /// </summary>
        private static ZeroOrOne? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private ZeroOrOne() : base() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular <see cref="ZeroOrOne"/> instance.
        /// </summary>
        /// <returns>The singular <see cref="ZeroOrOne"/> instance.</returns>
        public static ZeroOrOne Instance()
        {
            if (instance is null)
            {
                instance = new ZeroOrOne();
            }

            return instance;
        }

        /// <summary>
        /// Destroy method for the singular <see cref="ZeroOrOne"/> instance.
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
            return "\\{0, 1\\}";
        }

        /// <summary>
        /// Creates a deep copy of the current type.
        /// </summary>
        /// <returns>The created deep copy of the type.</returns>
        public override ZeroOrOne DeepCopy()
        {
            return ZeroOrOne.Instance();
        }

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is out of range of the <see cref="ZeroOrOne"/> type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is neither zero or one.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool IsValueOutOfRange(int value)
        {
            return value != 0 && value != 1;
        }

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is valid for the <see cref="ZeroOrOne"/> type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the value is zero or one.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool IsValueValid(int value)
        {
            return value == 0 || value == 1;
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
            ConstantBoundedInteger bounded => bounded.LowerBoundValue >= 0 &&
                                              bounded.UpperBoundValue <= 1,

            _ => assignedType is ZeroOrOne
        };

        /// <summary>
        /// Creates a program, that represents the type constraint on the given term.
        /// </summary>
        /// <param name="term">The term to formulate the constraint on.</param>
        /// <returns>The formulated constraint on the term.</returns>
        public override Formula TypeConstraintOn(IntegerTypeTerm term)
        {
            IntegerConstant zero = new IntegerConstant(0);
            IntegerConstant one  = new IntegerConstant(1);
            
            IntegerTypeTerm firstCopy  = term.DeepCopy();
            IntegerTypeTerm secondCopy = term.DeepCopy();

            IntegerTypeEqual equalsZero = new IntegerTypeEqual(firstCopy, zero);
            IntegerTypeEqual equalsOne  = new IntegerTypeEqual(secondCopy, one);

            return new DisjunctionFormula(equalsZero, equalsOne);
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
            return obj is ZeroOrOne;
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
            const int zeroOrOneLowerBound = 0;
            const int zeroOrOneUpperBound = 1;

            if (other is Integer or NaturalNumber or PositiveInteger or ZeroOrOne)
            {
                return ZeroOrOne.Instance();
            }

            if (other is ConstantBoundedInteger constantBounded)
            {
                bool emptyIntersection =
                    constantBounded.LowerBoundValue > zeroOrOneUpperBound ||
                    constantBounded.UpperBoundValue < zeroOrOneLowerBound;

                int lowerBound = Math.Max(constantBounded.LowerBoundValue, zeroOrOneLowerBound);
                int upperBound = Math.Min(constantBounded.UpperBoundValue, zeroOrOneUpperBound);

                return emptyIntersection ? null : new ConstantBoundedInteger(lowerBound, upperBound);
            }

            if (other is TermBoundedInteger bounded)
            {
                return IntersectionBounds(
                    bounded.LowerBound.DeepCopy(),
                    bounded.UpperBound.DeepCopy(),
                    new IntegerConstant(zeroOrOneLowerBound),
                    new IntegerConstant(zeroOrOneUpperBound)
                );
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
            const int zeroOrOneLowerBound = 0;
            const int zeroOrOneUpperBound = 1;

            if (other is Integer or NaturalNumber or PositiveInteger or ZeroOrOne)
            {
                return other.DeepCopy();
            }

            if (other is BoundedIntegerType bounded)
            {
                IntegerTypeTerm otherLowerBound = bounded.LowerBound.DeepCopy();
                IntegerTypeTerm otherUpperBound = bounded.UpperBound.DeepCopy();
                IntegerTypeTerm thisLowerBound  = new IntegerConstant(zeroOrOneLowerBound);
                IntegerTypeTerm thisUpperBound  = new IntegerConstant(zeroOrOneUpperBound);

                return UnionBounds(thisLowerBound, thisUpperBound, otherLowerBound, otherUpperBound);
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
            > 0 => PositiveInteger.Instance(),  // The right operand is a positive integer.
              0 => ZeroOrOne.Instance(),        // The right operand is zero.
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

            return (rightOperand.LowerBoundValue, rightOperand.UpperBoundValue) switch
            {
                (   0, 0) => ZeroOrOne.Instance(),        // The right operand can only be zero.
                (>= 1, _) => PositiveInteger.Instance(),  // The right operand is a positive integer.
                (   0, _) => NaturalNumber.Instance(),    // The right operand is a natural number.
                (   _, _) => Integer.Instance()           // The right operand is an integer (could be negative).
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
              0 => ZeroOrOne.Instance(),        // The right operand is zero.
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

            return (rightOperand.LowerBoundValue, rightOperand.UpperBoundValue) switch
            {
                (0,     0) => ZeroOrOne.Instance(),         // The right operand can only be zero.
                (_,     0) => NaturalNumber.Instance(),     // The right operand is a non positive integer.
                (_, <= -1) => PositiveInteger.Instance(),   // The right operand is a negative integer.
                (_,     _) => Integer.Instance()            // The right operand is an integer (could be positive).
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
        /// Determines the result type of a multiplication, with a constant <see cref="int"/> type right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public override IntegerType MultiplicationWithType(int rightOperandValue) => rightOperandValue switch
        {
               > 1 => NaturalNumber.Instance(),  // The right operand is an integer greater than one.
            0 or 1 => ZeroOrOne.Instance(),      // The right operand is zero or one.
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
                (>= 0, <= 1) => ZeroOrOne.Instance(),      // The right operand is zero or one.
                (>= 0,    _) => NaturalNumber.Instance(),  // The right operand is a natural number.
                (   _,    _) => Integer.Instance()         // The right operand is an integer.
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
            return ZeroOrOne.Instance();
        }

        #endregion
    }
}
