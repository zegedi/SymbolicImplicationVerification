using SymbolicImplicationVerification.Evaluations;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Patterns;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas.Relations
{
    public class GreaterThan : BinaryRelationFormula<IntegerType>
    {
        #region Constructors

        public GreaterThan(GreaterThan greaterThan) : base(
            greaterThan.identifier,
            greaterThan.leftComponent .DeepCopy(),
            greaterThan.rightComponent.DeepCopy()) { }

        public GreaterThan(LessThan lessThan) : base(
            lessThan.Identifier,
            lessThan.RightComponent.DeepCopy(),
            lessThan.LeftComponent .DeepCopy()) { }

        public GreaterThan(GreaterThanOrEqualTo greaterThanOrEqualTo) : base(
            greaterThanOrEqualTo.Identifier,
            (IntegerConstant)1 + greaterThanOrEqualTo.LeftComponent.DeepCopy(),
            greaterThanOrEqualTo.RightComponent.DeepCopy()) { }

        public GreaterThan(LessThanOrEqualTo lessThanOrEqualTo) : base(
            lessThanOrEqualTo.Identifier,
            (IntegerConstant)1 + lessThanOrEqualTo.RightComponent.DeepCopy(),
            lessThanOrEqualTo.LeftComponent.DeepCopy()) { }

        public GreaterThan(IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public GreaterThan(string? identifier, IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            return string.Format("{0} > {1}", leftComponent, rightComponent);
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
            GreaterThan other => IdenticalComponentsEquals(other),
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
            (GreaterThan thisEval, GreaterThan otherEval)
                => IdenticalSideRearrangementEquals(thisEval, otherEval),

            (GreaterThan thisEval, LessThan otherEval)
                => OppositeSideRearrangementEquals(thisEval, otherEval),

            (GreaterThan thisEval, GreaterThanOrEqualTo otherEval)
                => IdenticalSideRearrangementEquals(thisEval, new GreaterThan(otherEval)) ||
                   IdenticalSideRearrangementEquals(new GreaterThanOrEqualTo(thisEval), otherEval),

            (GreaterThan thisEval, LessThanOrEqualTo otherEval)
                => IdenticalSideRearrangementEquals(thisEval, new GreaterThan(otherEval)) ||
                   IdenticalSideRearrangementEquals(new LessThanOrEqualTo(thisEval), otherEval),

            (Formula thisEval, Formula otherEval) => thisEval.Equals(otherEval)
        };

        //public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        //{
        //    if (other is IntegerTypeEqual or IntegerTypeNotEqual)
        //    {
        //        return other.ConjunctionWith(this);
        //    }

        //    if (AnyRearrangementEquals(this, other))
        //    {
        //        switch (other)
        //        {
        //            case GreaterThan or GreaterThanOrEqualTo:
        //                return IdenticalSideRearrangementEquals(this, other) ? DeepCopy() : FALSE.Instance();

        //            case LessThan or LessThanOrEqualTo:
        //                return OppositeSideRearrangementEquals(this, other) ? DeepCopy() : FALSE.Instance();
        //        }
        //    }

        //    if (other is GreaterThan or LessThan or GreaterThanOrEqualTo or LessThanOrEqualTo)
        //    {
        //        GreaterThan that = this;

        //        switch (other)
        //        {
        //            case GreaterThan greaterThan:
        //                that = new GreaterThan(greaterThan);
        //                break;

        //            case LessThan lessThan: 
        //                that = new GreaterThan(lessThan);
        //                break;

        //            case GreaterThanOrEqualTo greaterThanOrEqualTo:
        //                that = new GreaterThan(greaterThanOrEqualTo);
        //                break;

        //            case LessThanOrEqualTo lessThanOrEqualTo:
        //                that = new GreaterThan(lessThanOrEqualTo);
        //                break;
        //        }

        //        GreaterThan thatMinusThis = new GreaterThan(
        //            new Subtraction(that.leftComponent .DeepCopy(), leftComponent .DeepCopy()),
        //            new Subtraction(that.rightComponent.DeepCopy(), rightComponent.DeepCopy())
        //        );

        //        if (thatMinusThis.Evaluated() is TRUE)
        //        {
        //            return new GreaterThan(this);
        //        }

        //        GreaterThan thisMinusThat = new GreaterThan(
        //            new Subtraction(leftComponent .DeepCopy(), that.leftComponent .DeepCopy()),
        //            new Subtraction(rightComponent.DeepCopy(), that.rightComponent.DeepCopy())
        //        );

        //        if (thisMinusThat.Evaluated() is TRUE)
        //        {
        //            return new GreaterThan(that);
        //        }
        //    }

        //    return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        //}

        //public override Formula UnionWith(BinaryRelationFormula<IntegerType> other)
        //{
        //    if (other is IntegerTypeEqual or IntegerTypeNotEqual)
        //    {
        //        return other.UnionWith(this);
        //    }

        //    if (AnyRearrangementEquals(this, other))
        //    {
        //        switch (other)
        //        {
        //            case GreaterThan or GreaterThanOrEqualTo:
        //                return OppositeSideRearrangementEquals(this, other) ? DeepCopy() : TRUE.Instance();

        //            case LessThan or LessThanOrEqualTo:
        //                return IdenticalSideRearrangementEquals(this, other) ? DeepCopy() : TRUE.Instance();

        //            case IntegerTypeEqual
        //        }
        //    }

        //    if (other is GreaterThan or LessThan or GreaterThanOrEqualTo or LessThanOrEqualTo)
        //    {
        //        GreaterThan that = this;

        //        switch (other)
        //        {
        //            case GreaterThan greaterThan:
        //                that = new GreaterThan(greaterThan);
        //                break;

        //            case LessThan lessThan:
        //                that = new GreaterThan(lessThan);
        //                break;

        //            case GreaterThanOrEqualTo greaterThanOrEqualTo:
        //                that = new GreaterThan(greaterThanOrEqualTo);
        //                break;

        //            case LessThanOrEqualTo lessThanOrEqualTo:
        //                that = new GreaterThan(lessThanOrEqualTo);
        //                break;
        //        }

        //        GreaterThan thatMinusThis = new GreaterThan(
        //            new Subtraction(that.leftComponent.DeepCopy(), leftComponent.DeepCopy()),
        //            new Subtraction(that.rightComponent.DeepCopy(), rightComponent.DeepCopy())
        //        );

        //        if (thatMinusThis.Evaluated() is TRUE)
        //        {
        //            return new GreaterThan(this);
        //        }

        //        GreaterThan thisMinusThat = new GreaterThan(
        //            new Subtraction(leftComponent.DeepCopy(), that.leftComponent.DeepCopy()),
        //            new Subtraction(rightComponent.DeepCopy(), that.rightComponent.DeepCopy())
        //        );

        //        if (thisMinusThat.Evaluated() is TRUE)
        //        {
        //            return new GreaterThan(that);
        //        }
        //    }

        //    return new DisjunctionFormula(DeepCopy(), other.DeepCopy());
        //}

        //public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        //{
        //    if (Equivalent(other))
        //    {
        //        return DeepCopy();
        //    }

        //    if (Complements(other))
        //    {
        //        return FALSE.Instance();
        //    }

        //    bool otherIsOrdering = other is IntegerTypeEqual or IntegerTypeNotEqual
        //                                 or LessThan         or LessThanOrEqualTo
        //                                 or GreaterThan      or GreaterThanOrEqualTo;

        //    //if (otherIsOrdering && AnyRearrangementEquals(this, other))
        //    //{
        //    //    bool allowesGreaterThan =
        //    //        other is IntegerTypeNotEqual ||
        //    //        other is GreaterThan or GreaterThanOrEqualTo && IdenticalSideRearrangementEquals(this, other) ||
        //    //        other is LessThan or LessThanOrEqualTo && OppositeSideRearrangementEquals(this, other);

        //    //    return allowesGreaterThan ? DeepCopy() : FALSE.Instance();
        //    //}

        //    if (otherIsOrdering)
        //    {

        //        if (AnyRearrangementEquals(this, other))
        //        {
        //            bool allowesGreaterThan =
        //                other is IntegerTypeNotEqual ||
        //                other is GreaterThan or GreaterThanOrEqualTo && IdenticalSideRearrangementEquals(this, other) ||
        //                other is LessThan or LessThanOrEqualTo && OppositeSideRearrangementEquals(this, other);

        //            return allowesGreaterThan ? DeepCopy() : FALSE.Instance();
        //        }

        //        if (other is LessThan or GreaterThan or LessThanOrEqualTo or GreaterThanOrEqualTo)
        //        {
        //            GreaterThan otherGreaterThan = new GreaterThan((dynamic) other);

        //            var result = SubtractionBasedConjunctionWith(this, other);

        //            if (result is not null)
        //            {
        //                return result;
        //            }
        //        }
        //    }

        //    bool otherIsDivisor = other is NotDivisor or Divisor;

        //    if (otherIsDivisor && IdenticalComponentsEquivalent(this, other))
        //    {
        //        bool divisorFormulaTrue = other is NotDivisor;

        //        return divisorFormulaTrue ? TRUE.Instance() : FALSE.Instance();
        //    }

        //    return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        //}

        public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        {
            Func<BinaryRelationFormula<IntegerType>, BinaryRelationFormula<IntegerType>, Formula> AnyRearrangementEqualsConjuctionWith
            = (greaterThan, other) =>
            {
                bool allowesGreaterThan =
                    other is IntegerTypeNotEqual ||
                    other is GreaterThan or GreaterThanOrEqualTo && IdenticalSideRearrangementEquals(greaterThan, other) ||
                    other is LessThan or LessThanOrEqualTo && OppositeSideRearrangementEquals(greaterThan, other);

                return allowesGreaterThan ? DeepCopy() : FALSE.Instance();
            };

            Func<Formula, Formula, Formula?> IdenticalComponentsEquivalentConjunctionWith
                = (lessThan, other) => other switch
                {
                    Divisor    => FALSE.Instance(),
                    NotDivisor => TRUE .Instance(),
                             _ => null
                };

            Func<Formula, Formula, Formula?> OppositeComponentsEquivalentConjunctionWith
                = (lessThan, other) => null;

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
                        return new GreaterThanOrEqualTo(leftComponent.DeepCopy(), rightComponent.DeepCopy());

                    case LessThan:
                        return OppositeSideRearrangementEquals(this, other) ? DeepCopy() :
                               new IntegerTypeNotEqual(leftComponent.DeepCopy(), rightComponent.DeepCopy());

                    case GreaterThan:
                        return IdenticalSideRearrangementEquals(this, other) ? DeepCopy() :
                               new IntegerTypeNotEqual(leftComponent.DeepCopy(), rightComponent.DeepCopy());

                    case LessThanOrEqualTo:
                        return OppositeSideRearrangementEquals(this, other) ? other.DeepCopy() : TRUE.Instance();

                    case GreaterThanOrEqualTo:
                        return IdenticalSideRearrangementEquals(this, other) ? other.DeepCopy() : TRUE.Instance();
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
                IntegerTypeConstant constant => constant.Value > 0 ? TRUE.Instance() : FALSE.Instance(),
                                           _ => ReturnOrDeepCopy(new GreaterThan(left, right))
            };
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new LessThanOrEqualTo(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current greater than program.
        /// </summary>
        /// <returns>The created deep copy of the greater than program.</returns>
        public override GreaterThan DeepCopy()
        {
            return new GreaterThan(this);
        }

        #endregion

        #region Private methods

        /*
        private IntegerTypeTerm RearrangeToLeft()
        {

        }
        */

        #endregion
    }
}
