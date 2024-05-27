using SymImply.Formulas.Relations;
using SymImply.Terms;
using SymImply.Terms.Constants;
using SymImply.Terms.Operations;
using SymImply.Types;

namespace SymImply.Formulas
{
    public class GreaterThanOrEqualTo : BinaryRelationFormula<IntegerType>
    {
        #region Constructors

        public GreaterThanOrEqualTo(GreaterThanOrEqualTo greaterThanOrEqualTo) : base(
            greaterThanOrEqualTo.identifier,
            greaterThanOrEqualTo.leftComponent.DeepCopy(),
            greaterThanOrEqualTo.rightComponent.DeepCopy()) { }

        public GreaterThanOrEqualTo(LessThanOrEqualTo lessThanOrEqualTo) : base(
            lessThanOrEqualTo.Identifier,
            lessThanOrEqualTo.RightComponent.DeepCopy(),
            lessThanOrEqualTo.LeftComponent .DeepCopy()) { }

        public GreaterThanOrEqualTo(GreaterThan greaterThan) : base(
            greaterThan.Identifier,
            greaterThan.LeftComponent.DeepCopy(),
            new IntegerTypeConstant(1) + greaterThan.RightComponent.DeepCopy()) { }

        public GreaterThanOrEqualTo(LessThan lessThan) : base(
            lessThan.Identifier,
            lessThan.RightComponent.DeepCopy(),
            new IntegerTypeConstant(1) + lessThan.LeftComponent.DeepCopy()) { }

        public GreaterThanOrEqualTo(IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public GreaterThanOrEqualTo(string? identifier, IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            return string.Format("{0} \\geq {1}", leftComponent, rightComponent);
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
            GreaterThanOrEqualTo other => IdenticalComponentsEquals(other),
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
            (GreaterThanOrEqualTo thisEval, GreaterThanOrEqualTo otherEval)
                => IdenticalSideRearrangementEquals(thisEval, otherEval),

            (GreaterThanOrEqualTo thisEval, LessThanOrEqualTo otherEval)
                => OppositeSideRearrangementEquals(thisEval, otherEval),

            (GreaterThanOrEqualTo thisEval, GreaterThan otherEval)
                => IdenticalSideRearrangementEquals(thisEval, new GreaterThanOrEqualTo(otherEval)) ||
                   IdenticalSideRearrangementEquals(new GreaterThan(thisEval), otherEval),

            (GreaterThanOrEqualTo thisEval, LessThan otherEval)
                => IdenticalSideRearrangementEquals(thisEval, new GreaterThanOrEqualTo(otherEval)) ||
                   IdenticalSideRearrangementEquals(new LessThan(thisEval), otherEval),

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
                = (greaterThanOrEqualTo, other) =>
                {
                    switch (other)
                    {
                        case IntegerTypeEqual:
                            return other.DeepCopy();

                        case IntegerTypeNotEqual:
                            return new GreaterThan(greaterThanOrEqualTo.LeftComponent .DeepCopy(),
                                                   greaterThanOrEqualTo.RightComponent.DeepCopy());

                        case LessThan:
                            return OppositeSideRearrangementEquals(greaterThanOrEqualTo, other) ?
                                   other.DeepCopy() : FALSE.Instance();

                        case GreaterThan:
                            return IdenticalSideRearrangementEquals(greaterThanOrEqualTo, other) ?
                                   other.DeepCopy() : FALSE.Instance();

                        case LessThanOrEqualTo:
                            return OppositeSideRearrangementEquals(greaterThanOrEqualTo, other) ? DeepCopy() :
                                   new IntegerTypeEqual(greaterThanOrEqualTo.LeftComponent .DeepCopy(), 
                                                        greaterThanOrEqualTo.RightComponent.DeepCopy());

                        default: //GreaterThanOrEqualTo:
                            return IdenticalSideRearrangementEquals(greaterThanOrEqualTo, other) ? DeepCopy() :
                                   new IntegerTypeEqual(greaterThanOrEqualTo.LeftComponent .DeepCopy(),
                                                        greaterThanOrEqualTo.RightComponent.DeepCopy());
                    }
                };

            Func<Formula, Formula, Formula?> IdenticalComponentsEquivalentConjunctionWith
                = (greaterThanOrEqualTo, other) => other switch
                {
                    Divisor divisor       => new IntegerTypeEqual(divisor.LeftComponent.DeepCopy(),
                                                                  divisor.RightComponent.DeepCopy()),
                    NotDivisor notDivisor => new IntegerTypeNotEqual(notDivisor.LeftComponent.DeepCopy(),
                                                                     notDivisor.RightComponent.DeepCopy()),
                    _ => null
                };

            Func<Formula, Formula, Formula?> OppositeComponentsEquivalentConjunctionWith
                = (lessThanOrEqualTo, other) => null;

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
                        return OppositeSideRearrangementEquals(this, other) ? DeepCopy() : TRUE.Instance();

                    case GreaterThan:
                        return IdenticalSideRearrangementEquals(this, other) ? DeepCopy() : TRUE.Instance();

                    case LessThanOrEqualTo:
                        return OppositeSideRearrangementEquals(this, other) ? DeepCopy() : TRUE.Instance();

                    case GreaterThanOrEqualTo:
                        return IdenticalSideRearrangementEquals(this, other) ? DeepCopy() : TRUE.Instance();
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
                IntegerTypeConstant constant => constant.Value >= 0 ? TRUE.Instance() : FALSE.Instance(),
                                           _ => ReturnOrDeepCopy(new GreaterThanOrEqualTo(left, right))
            };
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new LessThan(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current greater than or equal to program.
        /// </summary>
        /// <returns>The created deep copy of the greater than or equal to program.</returns>
        public override GreaterThanOrEqualTo DeepCopy()
        {
            return new GreaterThanOrEqualTo(this);
        }

        #endregion
    }
}
