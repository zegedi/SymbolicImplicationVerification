using SymbolicImplicationVerification.Formulas.Quantified;
using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Types;
using System.Collections.Generic;
using System.Reflection;

namespace SymbolicImplicationVerification.Formulas.Relations
{
    public abstract class BinaryRelationFormula<T> : Formula where T : Type
    {
        #region Fields

        /// <summary>
        /// The left component of the binary relation.
        /// </summary>
        protected Term<T> leftComponent;

        /// <summary>
        /// The right component of the binary relation.
        /// </summary>
        protected Term<T> rightComponent;

        #endregion

        #region Constructors

        public BinaryRelationFormula(Term<T> leftComponent, Term<T> rightComponent)
            : this(null, leftComponent, rightComponent) { }

        public BinaryRelationFormula(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier)
        {
            this.leftComponent = leftComponent;
            this.rightComponent = rightComponent;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the left component of the binary relation.
        /// </summary>
        public Term<T> LeftComponent
        {
            get { return leftComponent; }
            set { leftComponent = value; }
        }

        /// <summary>
        /// Gets or sets the right component of the binary relation.
        /// </summary>
        public Term<T> RightComponent
        {
            get { return rightComponent; }
            set { rightComponent = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Creates a deep copy of the current binary relation program.
        /// </summary>
        /// <returns>The created deep copy of the binary relation program.</returns>
        public override abstract BinaryRelationFormula<T> DeepCopy();

        /// <summary>
        /// Calculate the conjuction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the conjunction.</param>
        /// <returns>The result of the conjunction.</returns>
        public override Formula ConjunctionWith(Formula other)
        {
            if (other is QuantifiedFormula<T> quantifiedFormula)
            {
                return quantifiedFormula.ConjunctionWith(DeepCopy());
            }

            if (other is BinaryRelationFormula<T> relationFormula)
            {
                return ConjunctionWith(relationFormula);
            }

            return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        /// <summary>
        /// Calculate the disjunction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the disjunction.</param>
        /// <returns>The result of the disjunction.</returns>
        public override Formula DisjunctionWith(Formula other)
        {
            if (other is QuantifiedFormula<T> quantifiedFormula)
            {
                return quantifiedFormula.DisjunctionWith(DeepCopy());
            }

            if (other is BinaryRelationFormula<T> relationFormula)
            {
                return DisjunctionWith(relationFormula);
            }

            return new DisjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        /// <summary>
        /// Calculate the conjuction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the conjunction.</param>
        /// <returns>The result of the conjunction.</returns>
        public virtual Formula ConjunctionWith(BinaryRelationFormula<T> other)
        {
            return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        /// <summary>
        /// Calculate the disjunction of the current formula with the parameter.
        /// </summary>
        /// <param name="other">The other operand of the disjunction.</param>
        /// <returns>The result of the disjunction.</returns>
        public virtual Formula DisjunctionWith(BinaryRelationFormula<T> other)
        {
            return new DisjunctionFormula(DeepCopy(), other.DeepCopy());
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Determines whether the idential components equal.
        /// </summary>
        /// <param name="other">The other relation to compare with.</param>
        /// <returns>Returns <see langword="true"/> if the indentical components equal, otherwise <see langword="false"/>.</returns>
        protected bool IdenticalComponentsEquals(BinaryRelationFormula<T> other)
        {
            return leftComponent .Equals(other.leftComponent) &&
                   rightComponent.Equals(other.rightComponent);
        }

        /// <summary>
        /// Determines whether the opposite components equal.
        /// </summary>
        /// <param name="other">The other relation to compare with.</param>
        /// <returns>Returns <see langword="true"/> if the opposite components equal, otherwise <see langword="false"/>.</returns>
        protected bool OppositeComponentsEquals(BinaryRelationFormula<T> other)
        {
            return leftComponent .Equals(other.rightComponent) &&
                   rightComponent.Equals(other.leftComponent);
        }

        /// <summary>
        /// Determines whether the identical or opposite components equal.
        /// </summary>
        /// <param name="other">The other relation to compare with.</param>
        /// <returns>Returns <see langword="true"/> if the identical or opposite components equal, otherwise <see langword="false"/>.</returns>
        protected bool IdenticalOrOppositeComponentsEquals(BinaryRelationFormula<T> other)
        {
            return IdenticalComponentsEquals(other) || OppositeComponentsEquals(other);
        }

        /// <summary>
        /// Determines whether the identical components equivalent.
        /// </summary>
        /// <param name="first">The first relation.</param>
        /// <param name="second">The second relation.</param>
        /// <returns>Returns <see langword="true"/> if the identical components equivalent, otherwise <see langword="false"/>.</returns>
        protected bool IdenticalComponentsEquivalent(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            IntegerTypeTerm firstLeft   = first .leftComponent .Evaluated();
            IntegerTypeTerm firstRight  = first .rightComponent.Evaluated();
            IntegerTypeTerm secondLeft  = second.leftComponent .Evaluated();
            IntegerTypeTerm secondRight = second.rightComponent.Evaluated();

            return firstLeft.Equals(secondLeft) && firstRight.Equals(secondRight);
        }

        /// <summary>
        /// Determines whether the opposite components equivalent.
        /// </summary>
        /// <param name="first">The first relation.</param>
        /// <param name="second">The second relation.</param>
        /// <returns>Returns <see langword="true"/> if the opposite components equivalent, otherwise <see langword="false"/>.</returns>
        protected bool OppositeComponentsEquivalent(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            IntegerTypeTerm firstLeft   = first .leftComponent .Evaluated();
            IntegerTypeTerm firstRight  = first .rightComponent.Evaluated();
            IntegerTypeTerm secondLeft  = second.leftComponent .Evaluated();
            IntegerTypeTerm secondRight = second.rightComponent.Evaluated();

            return firstLeft.Equals(secondRight) && firstRight.Equals(secondLeft);
        }

        /// <summary>
        /// Determines whether the identical or opposite components equivalent.
        /// </summary>
        /// <param name="first">The first relation.</param>
        /// <param name="second">The second relation.</param>
        /// <returns>Returns <see langword="true"/> if the identical or opposite components equivalent, otherwise <see langword="false"/>.</returns>
        protected bool IdenticalOrOppositeComponentsEquivalent(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            IntegerTypeTerm firstLeft   = first .leftComponent .Evaluated();
            IntegerTypeTerm firstRight  = first .rightComponent.Evaluated();
            IntegerTypeTerm secondLeft  = second.leftComponent .Evaluated();
            IntegerTypeTerm secondRight = second.rightComponent.Evaluated();

            return firstLeft.Equals(secondLeft)  && firstRight.Equals(secondRight) ||
                   firstLeft.Equals(secondRight) && firstRight.Equals(secondLeft);
        }

        /// <summary>
        /// Determines whether the left and left side rearrangements equal.
        /// </summary>
        /// <param name="first">The first relation to rearrange.</param>
        /// <param name="second">The second relation to rearrange.</param>
        /// <returns>Returns <see langword="true"/> if the left and left side rearrangements equal, otherwise <see langword="false"/>.</returns>
        protected bool LeftAndLeftRearrangementEquals(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            Subtraction firstLeftRearrangement
                = new Subtraction(first.leftComponent.DeepCopy(), first.rightComponent.DeepCopy());

            Subtraction secondLeftRearrangement
                = new Subtraction(second.leftComponent.DeepCopy(), second.rightComponent.DeepCopy());

            IntegerTypeTerm firstLeftRearrangementSimplified  = firstLeftRearrangement .Evaluated();
            IntegerTypeTerm secondLeftRearrangementSimplified = secondLeftRearrangement.Evaluated();

            return firstLeftRearrangementSimplified.Equals(secondLeftRearrangementSimplified);
        }

        /// <summary>
        /// Determines whether the left and right side rearrangements equal.
        /// </summary>
        /// <param name="first">The first relation to rearrange.</param>
        /// <param name="second">The second relation to rearrange.</param>
        /// <returns>Returns <see langword="true"/> if the left and right side rearrangements equal, otherwise <see langword="false"/>.</returns>
        protected bool LeftAndRightRearrangementEquals(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            Subtraction firstLeftRearrangement
                = new Subtraction(first.leftComponent.DeepCopy(), first.rightComponent.DeepCopy());

            Subtraction secondRightRearrangement
                = new Subtraction(second.rightComponent.DeepCopy(), second.leftComponent.DeepCopy());

            IntegerTypeTerm firstLeftRearrangementSimplified   = firstLeftRearrangement  .Evaluated();
            IntegerTypeTerm secondRightRearrangementSimplified = secondRightRearrangement.Evaluated();

            return firstLeftRearrangementSimplified.Equals(secondRightRearrangementSimplified);
        }

        /// <summary>
        /// Determines whether the right and left side rearrangements equal.
        /// </summary>
        /// <param name="first">The first relation to rearrange.</param>
        /// <param name="second">The second relation to rearrange.</param>
        /// <returns>Returns <see langword="true"/> if the right and left side rearrangements equal, otherwise <see langword="false"/>.</returns>
        protected bool RightAndLeftRearrangementEquals(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            Subtraction firstRightRearrangement
                = new Subtraction(first.rightComponent.DeepCopy(), first.leftComponent.DeepCopy());

            Subtraction secondLeftRearrangement
                = new Subtraction(second.leftComponent.DeepCopy(), second.rightComponent.DeepCopy());

            IntegerTypeTerm firstRightRearrangementSimplified = firstRightRearrangement.Evaluated();
            IntegerTypeTerm secondLeftRearrangementSimplified = secondLeftRearrangement.Evaluated();

            return firstRightRearrangementSimplified.Equals(secondLeftRearrangementSimplified);
        }

        /// <summary>
        /// Determines whether the right and right side rearrangements equal.
        /// </summary>
        /// <param name="first">The first relation to rearrange.</param>
        /// <param name="second">The second relation to rearrange.</param>
        /// <returns>Returns <see langword="true"/> if the right and right side rearrangements equal, otherwise <see langword="false"/>.</returns>
        protected bool RightAndRightRearrangementEquals(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            Subtraction firstRightRearrangement
                = new Subtraction(first.rightComponent.DeepCopy(), first.leftComponent.DeepCopy());

            Subtraction secondRightRearrangement
                = new Subtraction(second.rightComponent.DeepCopy(), second.leftComponent.DeepCopy());

            IntegerTypeTerm firstRightRearrangementSimplified  = firstRightRearrangement .Evaluated();
            IntegerTypeTerm secondRightRearrangementSimplified = secondRightRearrangement.Evaluated();

            return firstRightRearrangementSimplified.Equals(secondRightRearrangementSimplified);
        }

        /// <summary>
        /// Determines whether the opposite side rearrangements equal.
        /// </summary>
        /// <param name="first">The first relation to rearrange.</param>
        /// <param name="second">The second relation to rearrange.</param>
        /// <returns>Returns <see langword="true"/> if the opposite side rearrangements equal, otherwise <see langword="false"/>.</returns>
        protected bool OppositeSideRearrangementEquals(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            return LeftAndRightRearrangementEquals(first, second) ||
                   RightAndLeftRearrangementEquals(first, second);
        }

        /// <summary>
        /// Determines whether the identical side rearrangements equal.
        /// </summary>
        /// <param name="first">The first relation to rearrange.</param>
        /// <param name="second">The second relation to rearrange.</param>
        /// <returns>Returns <see langword="true"/> if the identical side rearrangements equal, otherwise <see langword="false"/>.</returns>
        protected bool IdenticalSideRearrangementEquals(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            return LeftAndLeftRearrangementEquals(first, second) ||
                   RightAndRightRearrangementEquals(first, second);
        }

        /// <summary>
        /// Determines whether the any rearrangements equal.
        /// </summary>
        /// <param name="first">The first relation to rearrange.</param>
        /// <param name="second">The second relation to rearrange.</param>
        /// <returns>Returns <see langword="true"/> if the any rearrangements equal, otherwise <see langword="false"/>.</returns>
        protected bool AnyRearrangementEquals(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            return LeftAndLeftRearrangementEquals(first, second) ||
                   LeftAndRightRearrangementEquals(first, second) ||
                   RightAndLeftRearrangementEquals(first, second) ||
                   RightAndRightRearrangementEquals(first, second);
        }

        /// <summary>
        /// Subtracts the two relations from each other.
        /// </summary>
        /// <param name="first">The first relation.</param>
        /// <param name="second">The second relation.</param>
        /// <returns>Returns the result of the subtraction.</returns>
        protected BinaryRelationFormula<IntegerType> RelationSubtraction(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            BinaryRelationFormula<IntegerType> subtractionResult = first.DeepCopy();

            subtractionResult.leftComponent 
                = new Subtraction(subtractionResult.leftComponent , second.leftComponent.DeepCopy());
            
            subtractionResult.rightComponent
                = new Subtraction(subtractionResult.rightComponent, second.rightComponent.DeepCopy());

            return subtractionResult;
        }

        /// <summary>
        /// Subtracts the two relations from each other.
        /// </summary>
        /// <param name="first">The first relation.</param>
        /// <param name="second">The second relation.</param>
        /// <returns>Returns the result of the subtraction.</returns>
        protected BinaryRelationFormula<IntegerType>? SubtractionBasedConjunctionWith(
            BinaryRelationFormula<IntegerType> first, BinaryRelationFormula<IntegerType> second)
        {
            var firstMinusSecond = RelationSubtraction(first, second);

            if (firstMinusSecond.CompletelyEvaluated() is TRUE)
            {
                return second.DeepCopy();
            }

            var secondMinusFirst = RelationSubtraction(second, first);

            if (secondMinusFirst.CompletelyEvaluated() is TRUE)
            {
                return first.DeepCopy();
            }

            return null;
        }

        /// <summary>
        /// Calculate the conjuction of the current formula with the parameter.
        /// </summary>
        /// <returns>The result of the conjunction.</returns>
        protected Formula ConjunctionWith(
            BinaryRelationFormula<IntegerType> current, BinaryRelationFormula<IntegerType> other,
            Func<BinaryRelationFormula<IntegerType>, BinaryRelationFormula<IntegerType>, Formula> AnyRearrangementEqualsConjuctionWith,
            Func<Formula, Formula, Formula?> IdenticalComponentsEquivalentConjunctionWith,
            Func<Formula, Formula, Formula?> OppositeComponentsEquivalentConjunctionWith)
        {
            if (current.Equivalent(other))
            {
                return current.DeepCopy();
            }

            if (current.Complements(other))
            {
                return FALSE.Instance();
            }

            bool currentIsEquality = current is IntegerTypeEqual or IntegerTypeNotEqual;
            bool currentIsOrdering = current is LessThan    or LessThanOrEqualTo
                                             or GreaterThan or GreaterThanOrEqualTo;

            bool otherIsEquality = other is IntegerTypeEqual or IntegerTypeNotEqual;
            bool otherIsOrdering = other is LessThan    or LessThanOrEqualTo
                                         or GreaterThan or GreaterThanOrEqualTo;

            bool currentIsOrderingOrEquality = currentIsOrdering || currentIsEquality;
            bool otherIsOrderingOrEquality   = otherIsOrdering   || otherIsEquality;

            if (currentIsOrderingOrEquality && otherIsOrderingOrEquality && AnyRearrangementEquals(current, other))
            {
                return AnyRearrangementEqualsConjuctionWith(current, other);
            }

            Formula? result = null;

            if (currentIsOrdering && otherIsOrdering)
            {
                GreaterThan currentAsGreaterThan = new GreaterThan((dynamic) current);
                GreaterThan otherAsGreaterThan   = new GreaterThan((dynamic) other);

                result = SubtractionBasedConjunctionWith(currentAsGreaterThan, otherAsGreaterThan);
            }

            if (current is IntegerTypeEqual || other is IntegerTypeEqual)
            {
                IntegerTypeEqual equal;
                BinaryRelationFormula<IntegerType> that;

                if (current is IntegerTypeEqual)
                {
                    equal = (IntegerTypeEqual) current;
                    that  = other;
                }
                else
                {
                    equal = (IntegerTypeEqual) other;
                    that  = current;
                }

                result = equal.SubstituteVariable(that)?.CompletelyEvaluated();

                if (result is TRUE)
                {
                    result = equal.DeepCopy();
                }
                else if (result is not FALSE or NotEvaluable)
                {
                    result = null;
                }
            }

            if (result is null && IdenticalComponentsEquivalent(current, other))
            {
                result = IdenticalComponentsEquivalentConjunctionWith(current, other);
            }

            if (result is null && OppositeComponentsEquivalent(current, other))
            {
                result = OppositeComponentsEquivalentConjunctionWith(current, other);
            }

            result ??= new ConjunctionFormula(current.DeepCopy(), other.DeepCopy());

            return result;
        }

        #endregion
    }
}
