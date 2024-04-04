using SymbolicImplicationVerification.Formulas.Relations;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas
{
    public class LessThanOrEqualTo : BinaryRelationFormula<IntegerType>
    {
        #region Constructors

        public LessThanOrEqualTo(LessThanOrEqualTo lessThanOrEqualTo) : base(
            lessThanOrEqualTo.identifier,
            lessThanOrEqualTo.leftComponent .DeepCopy(),
            lessThanOrEqualTo.rightComponent.DeepCopy()) { }

        public LessThanOrEqualTo(GreaterThanOrEqualTo greaterThanOrEqualTo) : base(
            greaterThanOrEqualTo.Identifier,
            greaterThanOrEqualTo.RightComponent.DeepCopy(),
            greaterThanOrEqualTo.LeftComponent .DeepCopy()) { }

        public LessThanOrEqualTo(LessThan lessThan) : base(
            lessThan.Identifier,
            (IntegerConstant)1 + lessThan.LeftComponent.DeepCopy(),
            lessThan.RightComponent.DeepCopy()) { }

        public LessThanOrEqualTo(GreaterThan greaterThan) : base(
            greaterThan.Identifier,
            (IntegerConstant)1 + greaterThan.RightComponent.DeepCopy(),
            greaterThan.LeftComponent.DeepCopy()){ }

        public LessThanOrEqualTo(IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public LessThanOrEqualTo(string? identifier, IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}<={1}", leftComponent.ToString(), rightComponent.ToString());
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is equal to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj switch
        {
            LessThanOrEqualTo other => IdenticalComponentsEquals(other),
                                  _ => false
        };

        /// <summary>
        /// Determines whether the specified formula is equivalent to the current formula.
        /// </summary>
        /// <param name="other">The formula to compare with the current formula.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the formulas are the equivalent.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Equivalent(Formula other) => (Evaluated(), other.Evaluated()) switch
        {
            (LessThanOrEqualTo thisEval, LessThanOrEqualTo otherEval)
                => IdenticalSideRearrangementEquals(thisEval, otherEval),

            (LessThanOrEqualTo thisEval, GreaterThanOrEqualTo otherEval)
                => OppositeSideRearrangementEquals(thisEval, otherEval),

            (LessThanOrEqualTo thisEval, LessThan otherEval)
                => IdenticalSideRearrangementEquals(thisEval, new LessThanOrEqualTo(otherEval)),

            (LessThanOrEqualTo thisEval, GreaterThan otherEval)
                => IdenticalSideRearrangementEquals(thisEval, new LessThanOrEqualTo(otherEval)),

            (Formula thisEval, Formula otherEval) => thisEval.Equals(otherEval)
        };

        public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        {
            bool otherIsOrdering = other is IntegerTypeEqual or IntegerTypeNotEqual
                                         or LessThan         or LessThanOrEqualTo
                                         or GreaterThan      or GreaterThanOrEqualTo;

            if (otherIsOrdering && AnyRearrangementEquals(this, other))
            {
                switch (other)
                {
                    case IntegerTypeEqual:
                        return other.DeepCopy();

                    case IntegerTypeNotEqual:
                        return new LessThan(leftComponent.DeepCopy(), rightComponent.DeepCopy());

                    case LessThan:
                        return IdenticalSideRearrangementEquals(this, other) ? 
                               other.DeepCopy() : FALSE.Instance();

                    case GreaterThan:
                        return OppositeSideRearrangementEquals(this, other) ? 
                               other.DeepCopy() : FALSE.Instance();

                    case LessThanOrEqualTo:
                        return IdenticalSideRearrangementEquals(this, other) ? DeepCopy() : 
                               new IntegerTypeEqual(leftComponent.DeepCopy(), rightComponent.DeepCopy());

                    case GreaterThanOrEqualTo:
                        return OppositeSideRearrangementEquals(this, other) ? DeepCopy() :
                               new IntegerTypeEqual(leftComponent.DeepCopy(), rightComponent.DeepCopy());
                }
            }

            //bool otherIsDivisor = other is NotDivisor or Divisor;

            //if (otherIsDivisor && IdenticalComponentsEquivalent(this, other))
            //{
            //    bool divisorFormulaTrue = other is NotDivisor;

            //    return divisorFormulaTrue ? TRUE.Instance() : FALSE.Instance();
            //}

            return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        public override Formula DisjunctionWith(BinaryRelationFormula<IntegerType> other)
        {
            bool otherIsOrdering = other is IntegerTypeEqual or IntegerTypeNotEqual
                                         or LessThan         or LessThanOrEqualTo
                                         or GreaterThan      or GreaterThanOrEqualTo;

            if (otherIsOrdering && AnyRearrangementEquals(this, other))
            {
                switch (other)
                {
                    case IntegerTypeNotEqual:
                        return TRUE.Instance();

                    case IntegerTypeEqual:
                        return DeepCopy();

                    case LessThan:
                        return IdenticalSideRearrangementEquals(this, other) ? DeepCopy() : TRUE.Instance();

                    case GreaterThan:
                        return OppositeSideRearrangementEquals(this, other) ? DeepCopy() : TRUE.Instance();

                    case LessThanOrEqualTo:
                        return IdenticalSideRearrangementEquals(this, other) ? DeepCopy() : TRUE.Instance();

                    case GreaterThanOrEqualTo:
                        return OppositeSideRearrangementEquals(this, other) ? DeepCopy() : TRUE.Instance();
                }
            }

            //if (other is Divisor && OppositeComponentsEquivalent(this, other))
            //{
            //    return new GreaterThanOrEqualTo(leftComponent.DeepCopy(), rightComponent.DeepCopy());
            //}

            //if (other is NotDivisor && OppositeComponentsEquivalent(this, other))
            //{
            //    return new IntegerTypeNotEqual(leftComponent.DeepCopy(), rightComponent.DeepCopy());
            //}

            return new DisjunctionFormula(DeepCopy(), other.DeepCopy());
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
        /// Evaluate the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated()
        {
            IntegerTypeTerm left =
                leftComponent is IntegerTypeBinaryOperationTerm leftOperation ?
                leftOperation.Simplified() : leftComponent.DeepCopy();

            IntegerTypeTerm right =
                rightComponent is IntegerTypeBinaryOperationTerm rightOperation ?
                rightOperation.Simplified() : rightComponent.DeepCopy();

            Subtraction leftMinusRight = new Subtraction(left, right);

            return leftMinusRight.Simplified() switch
            {
                IntegerTypeConstant constant => constant.Value <= 0 ? TRUE.Instance() : FALSE.Instance(),
                                           _ => new LessThanOrEqualTo(left, right)
            };
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new GreaterThan(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current less than or equal to formula.
        /// </summary>
        /// <returns>The created deep copy of the less than or equal to formula.</returns>
        public override LessThanOrEqualTo DeepCopy()
        {
            return new LessThanOrEqualTo(this);
        }

        #endregion
    }
}
