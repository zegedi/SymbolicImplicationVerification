using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;

namespace SymbolicImplicationVerification.Types
{
    public class TermBoundedInteger : BoundedIntegerType
    {
        #region Constructors

        public TermBoundedInteger(TermBoundedInteger termBoundedInteger)
            : base(termBoundedInteger.lowerBound.DeepCopy(), termBoundedInteger.upperBound.DeepCopy()) { }

        public TermBoundedInteger(IntegerTypeTerm lowerBound, IntegerTypeTerm upperBound)
            : base(lowerBound, upperBound) { }

        public TermBoundedInteger(IntegerTypeTerm lowerBound, int upperBound)
            : base(lowerBound, new IntegerConstant(upperBound)) { }

        public TermBoundedInteger(int lowerBound, IntegerTypeTerm upperBound)
            : base(new IntegerConstant(lowerBound), upperBound) { }

        #endregion

        #region Public properties

        public override bool IsEmpty
        {
            get
            {
                Subtraction lowerMinusUpper = new Subtraction(lowerBound.DeepCopy(), upperBound.DeepCopy());

                return lowerMinusUpper.Evaluated() is IntegerTypeConstant constant && constant.Value > 0;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a deep copy of the current type.
        /// </summary>
        /// <returns>The created deep copy of the type.</returns>
        public override TermBoundedInteger DeepCopy()
        {
            return new TermBoundedInteger(this);
        }

        /// <summary>
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override TermBoundedInteger Evaluated()
        {
            return new TermBoundedInteger(lowerBound.Evaluated(), upperBound.Evaluated());
        }

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is out of range 
        /// for the <see cref="TermBoundedInteger"/> type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns><see langword="false"/> -  accept all <see cref="int"/> values.</returns>
        public override bool IsValueOutOfRange(int value)
        {
            return false;
        }

        /// <summary>
        /// Determines wheter the given <see cref="int"/> value is valid
        /// for the <see cref="TermBoundedInteger"/> type.
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to validate.</param>
        /// <returns><see langword="true"/> - accept all <see cref="int"/> values.</returns>
        public override bool IsValueValid(int value)
        {
            return true;
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
        public override bool TypeAssignable(Type assignedType)
        {
            return assignedType is TermBoundedInteger bounded &&
                   bounded.lowerBound.Equals(lowerBound) &&
                   bounded.upperBound.Equals(upperBound);
        }

        /// <summary>
        /// Creates a formula, that represents the type constraint on the given term.
        /// </summary>
        /// <param name="term">The term to formulate the constraint on.</param>
        /// <returns>The formulated constraint on the term.</returns>
        public override Formula TypeConstraintOn(IntegerTypeTerm term)
        {
            IntegerTypeTerm lower = lowerBound.DeepCopy();
            IntegerTypeTerm upper = upperBound.DeepCopy();

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
            return obj is TermBoundedInteger other &&
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
                int numbersetLowerBound =
                    other is PositiveInteger ? positiveIntegerLowerBound : naturalNumberLowerBound;

                otherLowerBound = new IntegerConstant(numbersetLowerBound);

                return IntersectionBounds(lowerBound.DeepCopy(), upperBound.DeepCopy(), otherLowerBound);
            }

            return IntersectionBounds(lowerBound.DeepCopy(), upperBound.DeepCopy(), otherLowerBound, otherUpperBound);

            //Formula chooseOtherLower = new LessThanOrEqualTo(lowerBound.DeepCopy(), otherLowerBound).Evaluated();
            //Formula chooseOtherUpper = new LessThanOrEqualTo(otherUpperBound, upperBound.DeepCopy()).Evaluated();
            //Formula chooseThisLower  = new LessThanOrEqualTo(otherLowerBound, lowerBound.DeepCopy()).Evaluated();
            //Formula chooseThisUpper  = new LessThanOrEqualTo(upperBound.DeepCopy(), otherUpperBound).Evaluated();

            //var possibleBounds = new List<(IntegerTypeTerm, IntegerTypeTerm, Formula, Formula)>
            //{
            //    (lowerBound.DeepCopy(), upperBound.DeepCopy(), chooseThisLower , chooseThisUpper ),
            //    (otherLowerBound      , upperBound.DeepCopy(), chooseOtherLower, chooseThisUpper ),
            //    (lowerBound.DeepCopy(), otherUpperBound      , chooseThisLower , chooseOtherUpper),
            //    (otherLowerBound      , otherUpperBound      , chooseOtherLower, chooseOtherUpper)
            //};

            //foreach ((IntegerTypeTerm lower, IntegerTypeTerm upper, Formula lowerResult, Formula upperResult) bounds in possibleBounds)
            //{
            //    bool chooseTheseBounds = bounds.lowerResult is TRUE && bounds.upperResult is TRUE;

            //    if (chooseTheseBounds)
            //    {
            //        return new TermBoundedInteger(bounds.lower, bounds.upper);
            //    }
            //}

            //return null;
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
        public override IntegerType AdditionWithType(int rightOperandValue)
        {
            return Integer.Instance();
        }

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

            return Integer.Instance();
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
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="PositiveInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public override IntegerType AdditionWithType(PositiveInteger rightOperand)
        {
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of an addition, with a <see cref="ZeroOrOne"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the addition.</param>
        /// <returns>The result <see cref="IntegerType"/> of the addition.</returns>
        public override IntegerType AdditionWithType(ZeroOrOne rightOperand)
        {
            return Integer.Instance();
        }


        /*========================= Subtraction result type selection ==========================*/

        /// <summary>
        /// Determines the result type of a subtraction, with a constant <see cref="int"/> right operand.
        /// </summary>
        /// <param name="rightOperandValue">The constant right operand of the subtraction.</param>
        /// <returns>The result <see cref="IntegerType"/> of the subtraction.</returns>
        public override IntegerType SubtractionWithType(int rightOperandValue)
        {
            return Integer.Instance();
        }

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

            return Integer.Instance();
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
        public override IntegerType MultiplicationWithType(int rightOperandValue)
        {
            return Integer.Instance();
        }

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

            return Integer.Instance();
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
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="PositiveInteger"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public override IntegerType MultiplicationWithType(PositiveInteger rightOperand)
        {
            return Integer.Instance();
        }

        /// <summary>
        /// Determines the result type of a multiplication, with a <see cref="ZeroOrOne"/> right operand.
        /// </summary>
        /// <param name="rightOperand">The right operand of the multiplication.</param>
        /// <returns>The result <see cref="IntegerType"/> of the multiplication.</returns>
        public override IntegerType MultiplicationWithType(ZeroOrOne rightOperand)
        {
            return Integer.Instance();
        }

        #endregion
    }
}
