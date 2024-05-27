using SymImply.Evaluations;
using SymImply.Terms;
using SymImply.Terms.Constants;
using SymImply.Terms.Operations;
using SymImply.Terms.Operations.Binary;
using SymImply.Terms.Patterns;
using SymImply.Terms.Variables;
using SymImply.Types;

namespace SymImply.Formulas.Relations
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
            new IntegerTypeConstant(1) + greaterThanOrEqualTo.LeftComponent.DeepCopy(),
            greaterThanOrEqualTo.RightComponent.DeepCopy()) { }

        public GreaterThan(LessThanOrEqualTo lessThanOrEqualTo) : base(
            lessThanOrEqualTo.Identifier,
            new IntegerTypeConstant(1) + lessThanOrEqualTo.RightComponent.DeepCopy(),
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

        /// <summary>
        /// Calculate the conjuction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the conjunction.</param>
        /// <returns>The result of the conjunction.</returns>
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
    }
}
