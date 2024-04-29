using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas.Relations
{
    public class IntegerTypeNotEqual : NotEqual<IntegerType>
    {
        #region Constructors

        public IntegerTypeNotEqual(IntegerTypeNotEqual equal) : base(
            equal.identifier,
            equal.leftComponent .DeepCopy(),
            equal.rightComponent.DeepCopy()) { }

        public IntegerTypeNotEqual(IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public IntegerTypeNotEqual(
            string? identifier, IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the specified object is notEqual to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is notEqual to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj switch
        {
            IntegerTypeNotEqual other => IdenticalComponentsEquals(other),
                                    _ => false
        };

        /// <summary>
        /// Determines whether the specified program is equivalent to the current program.
        /// </summary>
        /// <param name="other">The program to compare with the current program.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the formulas are the equivalent.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Equivalent(Formula other) => (Evaluated(), other.Evaluated()) switch
        {
            (IntegerTypeNotEqual thisEval, IntegerTypeNotEqual otherEval)
                => AnyRearrangementEquals(thisEval, otherEval),

            (Formula thisEval, Formula otherEval) => thisEval.Equals(otherEval)
        };

        //public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        //{
        //    bool otherIsOrdering = other is IntegerTypeEqual or IntegerTypeNotEqual
        //                                 or LessThan         or LessThanOrEqualTo
        //                                 or GreaterThan      or GreaterThanOrEqualTo;

        //    if (otherIsOrdering && AnyRearrangementEquals(this, other))
        //    {
        //        switch (other)
        //        {
        //            case IntegerTypeEqual:
        //                return FALSE.Instance();

        //            case IntegerTypeNotEqual or LessThan or GreaterThan:
        //                return other.DeepCopy();

        //            case LessThanOrEqualTo lessThanOrEqual:
        //                return new LessThan(lessThanOrEqual.LeftComponent .DeepCopy(), 
        //                                    lessThanOrEqual.RightComponent.DeepCopy());

        //            case GreaterThanOrEqualTo greaterThanOrEqual:
        //                return new GreaterThan(greaterThanOrEqual.LeftComponent .DeepCopy(), 
        //                                       greaterThanOrEqual.RightComponent.DeepCopy());
        //        }
        //    }

        //    if (other is NotDivisor notDivisor && IdenticalOrOppositeComponentsEquivalent(this, other))
        //    {
        //        return notDivisor.DeepCopy();
        //    }

        //    return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        //}

        public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        {
            Func<BinaryRelationFormula<IntegerType>, BinaryRelationFormula<IntegerType>, Formula> AnyRearrangementEqualsConjuctionWith
                = (notEqual, other) =>
                {
                    switch (other)
                    {
                        case IntegerTypeNotEqual or LessThan or GreaterThan:
                            return other.DeepCopy();

                        case LessThanOrEqualTo lessThanOrEqual:
                            return new LessThan(lessThanOrEqual.LeftComponent.DeepCopy(),
                                                lessThanOrEqual.RightComponent.DeepCopy());

                        case GreaterThanOrEqualTo greaterThanOrEqual:
                            return new GreaterThan(greaterThanOrEqual.LeftComponent.DeepCopy(),
                                                   greaterThanOrEqual.RightComponent.DeepCopy());

                        default: //IntegerTypeEqual:
                            return FALSE.Instance();
                    }
                };

            Func<Formula, Formula, Formula?> IdenticalComponentsEquivalentConjunctionWith
                = (equal, other) => other is NotDivisor notDivisor ? notDivisor.DeepCopy() : null;

            Func<Formula, Formula, Formula?> OppositeComponentsEquivalentConjunctionWith
                = (equal, other) => other is NotDivisor notDivisor ? notDivisor.DeepCopy() : null;

            return ConjunctionWith(this, other, AnyRearrangementEqualsConjuctionWith,
                IdenticalComponentsEquivalentConjunctionWith, OppositeComponentsEquivalentConjunctionWith);
        }

        public override Formula DisjunctionWith(BinaryRelationFormula<IntegerType> other)
        {
            bool otherIsOrdering = other is IntegerTypeEqual or IntegerTypeNotEqual
                                         or LessThan         or LessThanOrEqualTo
                                         or GreaterThan      or GreaterThanOrEqualTo;

            if (otherIsOrdering && AnyRearrangementEquals(this, other))
            {
                bool allowesEquality = other is IntegerTypeEqual or LessThanOrEqualTo or GreaterThanOrEqualTo;

                return allowesEquality ? TRUE.Instance() : DeepCopy();
            }

            if (other is Divisor && IdenticalOrOppositeComponentsEquivalent(this, other))
            {
                return TRUE.Instance();
            }

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
        /// Evaluated the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Evaluated()
        {
            IntegerTypeTerm left  = leftComponent .Evaluated();
            IntegerTypeTerm right = rightComponent.Evaluated();

            if (left is IntegerTypeBinaryOperationTerm leftOperation &&
                leftOperation.RearrangementEquals(leftComponent))
            {
                left = leftComponent;
            }

            if (right is IntegerTypeBinaryOperationTerm rightOperation &&
                rightOperation.RearrangementEquals(rightComponent))
            {
                right = rightComponent;
            }

            Subtraction leftMinusRight = new Subtraction(left, right);

            return leftMinusRight.Evaluated() switch
            {
                IntegerTypeConstant constant => constant.Value != 0 ? TRUE.Instance() : FALSE.Instance(),
                                           _ => ReturnOrDeepCopy(new IntegerTypeNotEqual(left, right))
            };
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new IntegerTypeEqual(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current notEqual program.
        /// </summary>
        /// <returns>The created deep copy of the notEqual program.</returns>
        public override IntegerTypeNotEqual DeepCopy()
        {
            return new IntegerTypeNotEqual(this);
        }

        #endregion
    }
}
