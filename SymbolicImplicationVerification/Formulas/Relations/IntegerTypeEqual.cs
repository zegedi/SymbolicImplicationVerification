﻿using SymbolicImplicationVerification.Evaluations;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Terms.Variables;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas.Relations
{
    public class IntegerTypeEqual : Equal<IntegerType>
    {
        #region Constructors

        public IntegerTypeEqual(IntegerTypeEqual equal) : base(
            equal.identifier,
            equal.leftComponent .DeepCopy(),
            equal.rightComponent.DeepCopy()) { }

        public IntegerTypeEqual(IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public IntegerTypeEqual(
            string? identifier, IntegerTypeTerm leftComponent, IntegerTypeTerm rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

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
            IntegerTypeEqual other => IdenticalComponentsEquals(other),
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
            (IntegerTypeEqual thisEval, IntegerTypeEqual otherEval)
                => AnyRearrangementEquals(thisEval, otherEval),

            (Formula thisEval, Formula otherEval) => thisEval.Equals(otherEval)
        };

        //public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        //{
        //    bool otherIsDivisor  = other is NotDivisor or Divisor;
        //    bool otherIsOrdering = other is IntegerTypeEqual or IntegerTypeNotEqual 
        //                                 or LessThan         or LessThanOrEqualTo 
        //                                 or GreaterThan      or GreaterThanOrEqualTo;

        //    bool handleOrdering = otherIsOrdering && AnyRearrangementEquals(this, other);
        //    bool handleDivisor  = otherIsDivisor  && IdenticalOrOppositeComponentsEquivalent(this, other);

        //    if (handleOrdering || handleDivisor)
        //    {
        //        bool allowesEquality = other is GreaterThanOrEqualTo or LessThanOrEqualTo 
        //                                     or IntegerTypeEqual     or Divisor;

        //        return allowesEquality ? DeepCopy() : FALSE.Instance();
        //    }

        //    Formula? substituted = SubstituteVariable(other.DeepCopy());

        //    if (substituted is not null)
        //    {
        //        Formula substitutedEval = substituted.CompletelyEvaluated();

        //        switch (substitutedEval)
        //        {
        //            case TRUE:
        //                return DeepCopy();

        //            case FALSE or NotEvaluable:
        //                return FALSE.Instance();
        //        }
        //    }

        //    return new ConjunctionFormula(DeepCopy(), other.DeepCopy());
        //}

        public override Formula ConjunctionWith(BinaryRelationFormula<IntegerType> other)
        {
            Func<BinaryRelationFormula<IntegerType>, BinaryRelationFormula<IntegerType>, Formula> AnyRearrangementEqualsConjuctionWith
                = (equal, other) =>
                {
                    bool allowesEquality = other is GreaterThanOrEqualTo or LessThanOrEqualTo
                                                 or IntegerTypeEqual     or Divisor;

                    return allowesEquality ? DeepCopy() : FALSE.Instance();
                };

            Func<Formula, Formula, Formula?> IdenticalComponentsEquivalentConjunctionWith
                = (equal, other) =>
                {
                    bool allowesEquality = other is GreaterThanOrEqualTo or LessThanOrEqualTo
                                                 or IntegerTypeEqual     or Divisor;

                    return allowesEquality ? DeepCopy() : FALSE.Instance();
                };

            Func<Formula, Formula, Formula?> OppositeComponentsEquivalentConjunctionWith
                = (equal, other) =>
                {
                    bool allowesEquality = other is GreaterThanOrEqualTo or LessThanOrEqualTo
                                                 or IntegerTypeEqual     or Divisor;

                    return allowesEquality ? DeepCopy() : FALSE.Instance();
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
                        return TRUE.Instance();

                    case IntegerTypeEqual or LessThanOrEqualTo or GreaterThanOrEqualTo:
                        return other.DeepCopy();

                    case LessThan lessThan:
                        return new LessThanOrEqualTo(
                            lessThan.LeftComponent.DeepCopy(), lessThan.RightComponent.DeepCopy());

                    case GreaterThan greaterThan:
                        return new GreaterThanOrEqualTo(
                            greaterThan.LeftComponent.DeepCopy(), greaterThan.RightComponent.DeepCopy());
                }
            }

            if (other is Divisor divisor && IdenticalOrOppositeComponentsEquivalent(this, other))
            {
                return divisor.DeepCopy();
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
                IntegerTypeConstant constant => constant.Value == 0 ? TRUE.Instance() : FALSE.Instance(),
                                           _ => ReturnOrDeepCopy(new IntegerTypeEqual(left, right))
            };
        }

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new IntegerTypeNotEqual(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current equal program.
        /// </summary>
        /// <returns>The created deep copy of the equal program.</returns>
        public override IntegerTypeEqual DeepCopy()
        {
            return new IntegerTypeEqual(this);
        }

        #endregion
    }
}
