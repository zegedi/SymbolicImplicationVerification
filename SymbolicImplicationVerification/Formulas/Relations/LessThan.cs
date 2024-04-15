using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas.Relations
{
    public class LessThan : BinaryRelationFormula<IntegerType>
    {
        #region Constructors

        public LessThan(LessThan lessThan) : base(
            lessThan.identifier,
            lessThan.leftComponent .DeepCopy(),
            lessThan.rightComponent.DeepCopy()) { }

        public LessThan(GreaterThan lessThan) : base(
            lessThan.Identifier,
            lessThan.RightComponent.DeepCopy(),
            lessThan.LeftComponent .DeepCopy()) { }

        public LessThan(LessThanOrEqualTo lessThanOrEqualTo) : base(
            lessThanOrEqualTo.Identifier,
            lessThanOrEqualTo.LeftComponent.DeepCopy(),
            (IntegerConstant)1 + lessThanOrEqualTo.RightComponent.DeepCopy()) { }

        public LessThan(GreaterThanOrEqualTo greaterThanOrEqualTo) : base(
            greaterThanOrEqualTo.Identifier,
            greaterThanOrEqualTo.RightComponent.DeepCopy(),
            (IntegerConstant)1 + greaterThanOrEqualTo.LeftComponent.DeepCopy()) { }

        public LessThan(IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public LessThan(string? identifier, IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            return string.Format("{0} < {1}", leftComponent, rightComponent);
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
            LessThan other => IdenticalComponentsEquals(other),
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
            (LessThan thisEval, LessThan otherEval)
                => IdenticalSideRearrangementEquals(thisEval, otherEval),

            (LessThan thisEval, GreaterThan otherEval)
                => OppositeSideRearrangementEquals(thisEval, otherEval),

            (LessThan thisEval, LessThanOrEqualTo otherEval)
                => IdenticalSideRearrangementEquals(thisEval, new LessThan(otherEval)) ||
                   IdenticalSideRearrangementEquals(new LessThanOrEqualTo(thisEval), otherEval),

            (LessThan thisEval, GreaterThanOrEqualTo otherEval)
                => IdenticalSideRearrangementEquals(thisEval, new LessThan(otherEval)) ||
                   IdenticalSideRearrangementEquals(new GreaterThanOrEqualTo(thisEval), otherEval),

            (Formula thisEval, Formula otherEval) => thisEval.Equals(otherEval)
        };

        //public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        //{
        //    bool otherIsOrdering = other is IntegerTypeEqual or IntegerTypeNotEqual
        //                                 or LessThan         or LessThanOrEqualTo
        //                                 or GreaterThan      or GreaterThanOrEqualTo;

        //    //if (otherIsOrdering && AnyRearrangementEquals(this, other))
        //    //{
        //    //    bool allowesLessThan =
        //    //        other is IntegerTypeNotEqual ||
        //    //        other is LessThan or LessThanOrEqualTo && IdenticalSideRearrangementEquals(this, other) ||
        //    //        other is GreaterThan or GreaterThanOrEqualTo && OppositeSideRearrangementEquals(this, other);

        //    //    return allowesLessThan ? DeepCopy() : FALSE.Instance();
        //    //}

        //    if (otherIsOrdering)
        //    {
        //        if (Equivalent(other))
        //        {
        //            return DeepCopy();
        //        }

        //        if (AnyRearrangementEquals(this, other))
        //        {
        //            bool allowesLessThan =
        //                other is IntegerTypeNotEqual ||
        //                other is LessThan or LessThanOrEqualTo && IdenticalSideRearrangementEquals(this, other) ||
        //                other is GreaterThan or GreaterThanOrEqualTo && OppositeSideRearrangementEquals(this, other);

        //            return allowesLessThan ? DeepCopy() : FALSE.Instance();
        //        }

        //        if (other is LessThan or GreaterThan or LessThanOrEqualTo or GreaterThanOrEqualTo)
        //        {
        //            LessThan otherLessThan = new LessThan((dynamic) other);

        //            var result = SubtractionBasedConjunctionWith(this, other);

        //            if (result is not null)
        //            {
        //                return result;
        //            }
        //        }


        //    }

        //    bool otherIsDivisor = other is NotDivisor or Divisor;

        //    if (otherIsDivisor && OppositeComponentsEquivalent(this, other))
        //    {
        //        bool divisorFormulaTrue = other is NotDivisor;

        //        return divisorFormulaTrue ? TRUE.Instance() : FALSE.Instance();
        //    }

        //    return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        //}

        public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        {
            Func<BinaryRelationFormula<IntegerType>, BinaryRelationFormula<IntegerType>, Formula> AnyRearrangementEqualsConjuctionWith
                = (lessThan, other) =>
                {
                    bool allowesLessThan =
                        other is IntegerTypeNotEqual ||
                        other is LessThan or LessThanOrEqualTo && IdenticalSideRearrangementEquals(lessThan, other) ||
                        other is GreaterThan or GreaterThanOrEqualTo && OppositeSideRearrangementEquals(lessThan, other);

                    return allowesLessThan ? DeepCopy() : FALSE.Instance();
                };

            Func<Formula, Formula, Formula?> IdenticalComponentsEquivalentConjunctionWith 
                = (lessThan, other) => null;

            Func<Formula, Formula, Formula?> OppositeComponentsEquivalentConjunctionWith
                = (lessThan, other) => other switch
            {
                Divisor    => FALSE.Instance(),
                NotDivisor => TRUE .Instance(),
                         _ => null
            };

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
                switch (other)
                {
                    case IntegerTypeNotEqual:
                        return DeepCopy();

                    case IntegerTypeEqual:
                        return new LessThanOrEqualTo(leftComponent.DeepCopy(), rightComponent.DeepCopy());

                    case LessThan:
                        return IdenticalSideRearrangementEquals(this, other) ? DeepCopy() : 
                               new IntegerTypeNotEqual(leftComponent.DeepCopy(), rightComponent.DeepCopy());

                    case GreaterThan:
                        return OppositeSideRearrangementEquals(this, other) ? DeepCopy() :
                               new IntegerTypeNotEqual(leftComponent.DeepCopy(), rightComponent.DeepCopy());

                    case LessThanOrEqualTo:
                        return IdenticalSideRearrangementEquals(this, other) ? other.DeepCopy() : TRUE.Instance();

                    case GreaterThanOrEqualTo:
                        return OppositeSideRearrangementEquals(this, other) ? other.DeepCopy() : TRUE.Instance();
                }
            }

            if (other is Divisor && IdenticalComponentsEquivalent(this, other))
            {
                return new LessThanOrEqualTo(leftComponent.DeepCopy(), rightComponent.DeepCopy());
            }

            if (other is NotDivisor && IdenticalComponentsEquivalent(this, other))
            {
                return new IntegerTypeNotEqual(leftComponent.DeepCopy(), rightComponent.DeepCopy());
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
                IntegerTypeConstant constant => constant.Value < 0 ? TRUE.Instance() : FALSE.Instance(),
                                           _ => ReturnOrDeepCopy(new LessThan(left, right))
            };
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new GreaterThanOrEqualTo(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current less than formula.
        /// </summary>
        /// <returns>The created deep copy of the less than formula.</returns>
        public override LessThan DeepCopy()
        {
            return new LessThan(this);
        }

        #endregion
    }
}
