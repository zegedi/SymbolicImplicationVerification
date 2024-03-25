﻿using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms.Operations;

namespace SymbolicImplicationVerification.Formulas.Relations
{
    public abstract class Equal<T> : BinaryRelationFormula<T> where T : Type
    {
        #region Constructors

        public Equal(Equal<T> equal) : base(
            equal.identifier, 
            equal.leftComponent .DeepCopy(), 
            equal.rightComponent.DeepCopy()) { }

        public Equal(Term<T> leftComponent, Term<T> rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public Equal(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}={1}", leftComponent.ToString(), rightComponent.ToString());
        }

        /*
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
            Equal<T> other => IdenticalComponentsEquals(other),
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
        public override bool Equivalent(Formula other)
        {
            if (other.Evaluated() is Equal<T> otherEqual)
            {
                Subtraction thisLeftRearrangement = leftComponent.DeepCopy() - rightComponent.DeepCopy();
            }
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
        public override Formula Evaluated() => (leftComponent, rightComponent) switch
        {
            (IntegerTypeConstant left, IntegerTypeConstant right) =>
                left.Value == right.Value ? TRUE.Instance() : FALSE.Instance(),

            (LogicalConstant left, LogicalConstant right) =>
                left.Value == right.Value ? TRUE.Instance() : FALSE.Instance(),

            (_, _) => leftComponent.Equals(rightComponent) ? TRUE.Instance() : new Equal<T>(this)
        };

        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override Formula Negated()
        {
            return new NotEqual<T>(null, leftComponent.DeepCopy(), rightComponent.DeepCopy());
        }

        /// <summary>
        /// Creates a deep copy of the current equal formula.
        /// </summary>
        /// <returns>The created deep copy of the equal formula.</returns>
        public override Equal<T> DeepCopy()
        {
            return new Equal<T>(this);
        }
        */

        #endregion
    }
}