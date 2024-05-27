using SymImply.Formulas.Relations;
using SymImply.Terms;
using SymImply.Terms.Constants;
using SymImply.Terms.Operations;
using SymImply.Types;

namespace SymImply.Formulas
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
            new IntegerTypeConstant(1) + lessThan.LeftComponent.DeepCopy(),
            lessThan.RightComponent.DeepCopy()) { }

        public LessThanOrEqualTo(GreaterThan greaterThan) : base(
            greaterThan.Identifier,
            new IntegerTypeConstant(1) + greaterThan.RightComponent.DeepCopy(),
            greaterThan.LeftComponent.DeepCopy()){ }

        public LessThanOrEqualTo(IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public LessThanOrEqualTo(string? identifier, IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            return string.Format("{0} \\leq {1}", leftComponent, rightComponent);
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
            (LessThanOrEqualTo thisEval, LessThanOrEqualTo otherEval)
                => IdenticalSideRearrangementEquals(thisEval, otherEval),

            (LessThanOrEqualTo thisEval, GreaterThanOrEqualTo otherEval)
                => OppositeSideRearrangementEquals(thisEval, otherEval),

            (LessThanOrEqualTo thisEval, LessThan otherEval)
                => IdenticalSideRearrangementEquals(thisEval, new LessThanOrEqualTo(otherEval)) ||
                   IdenticalSideRearrangementEquals(new LessThan(thisEval), otherEval),

            (LessThanOrEqualTo thisEval, GreaterThan otherEval)
                => IdenticalSideRearrangementEquals(thisEval, new LessThanOrEqualTo(otherEval)) ||
                   IdenticalSideRearrangementEquals(new GreaterThan(thisEval), otherEval),

            (Formula thisEval, Formula otherEval) => thisEval.Equals(otherEval)
        };

        /// <summary>
        /// Calculate the conjuction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the conjunction.</param>
        /// <returns>The result of the conjunction.</returns>
        public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        {
            Func<BinaryRelationFormula<IntegerType>, BinaryRelationFormula<IntegerType>, Formula> AnyRearrangementEqualsConjuctionWith
                = (lessThanOrEqualTo, other) =>
                {
                    switch (other)
                    {
                        case IntegerTypeEqual:
                            return other.DeepCopy();

                        case IntegerTypeNotEqual:
                            return new LessThan(lessThanOrEqualTo.LeftComponent .DeepCopy(), 
                                                lessThanOrEqualTo.RightComponent.DeepCopy());

                        case LessThan:
                            return IdenticalSideRearrangementEquals(lessThanOrEqualTo, other) ?
                                   other.DeepCopy() : FALSE.Instance();

                        case GreaterThan:
                            return OppositeSideRearrangementEquals(lessThanOrEqualTo, other) ?
                                   other.DeepCopy() : FALSE.Instance();

                        case LessThanOrEqualTo:
                            return IdenticalSideRearrangementEquals(lessThanOrEqualTo, other) ? DeepCopy() :
                                   new IntegerTypeEqual(lessThanOrEqualTo.LeftComponent .DeepCopy(),
                                                        lessThanOrEqualTo.RightComponent.DeepCopy());

                        default: //GreaterThanOrEqualTo
                            return OppositeSideRearrangementEquals(lessThanOrEqualTo, other) ? DeepCopy() :
                                   new IntegerTypeEqual(lessThanOrEqualTo.LeftComponent .DeepCopy(),
                                                        lessThanOrEqualTo.RightComponent.DeepCopy());
                    }
                };

            Func<Formula, Formula, Formula?> IdenticalComponentsEquivalentConjunctionWith
                = (lessThanOrEqualTo, other) => null;

            Func<Formula, Formula, Formula?> OppositeComponentsEquivalentConjunctionWith
                = (lessThanOrEqualTo, other) => other switch
                {
                    Divisor    divisor    => new IntegerTypeEqual(divisor.LeftComponent .DeepCopy(), 
                                                                  divisor.RightComponent.DeepCopy()),
                    NotDivisor notDivisor => new IntegerTypeNotEqual(notDivisor.LeftComponent .DeepCopy(),
                                                                     notDivisor.RightComponent.DeepCopy()),
                    _ => null
                };

            return ConjunctionWith(this, other, AnyRearrangementEqualsConjuctionWith,
                IdenticalComponentsEquivalentConjunctionWith, OppositeComponentsEquivalentConjunctionWith);
        }

        /// <summary>
        /// Calculate the disjunction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the disjunction.</param>
        /// <returns>The result of the disjunction.</returns>
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

            Subtraction leftMinusRight = new Subtraction(left, right);

            return leftMinusRight.Evaluated() switch
            {
                IntegerTypeConstant constant => constant.Value <= 0 ? TRUE.Instance() : FALSE.Instance(),
                                           _ => ReturnOrDeepCopy(new LessThanOrEqualTo(left, right))
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
        /// Creates a deep copy of the current less than or equal to program.
        /// </summary>
        /// <returns>The created deep copy of the less than or equal to program.</returns>
        public override LessThanOrEqualTo DeepCopy()
        {
            return new LessThanOrEqualTo(this);
        }

        #endregion
    }
}
