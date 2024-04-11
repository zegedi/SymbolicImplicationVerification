﻿using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Patterns
{
    public abstract class ConstantPattern<V, T> : Pattern<T> where T : Type
    {
        #region Constructors

        public ConstantPattern(int identifier, T termType) : base(identifier, termType) { }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Create a deep copy of the current constant pattern.
        /// </summary>
        /// <returns>The created deep copy of the constant pattern.</returns>
        public override abstract ConstantPattern<V, T> DeepCopy();

        /// <summary>
        /// Evaluated the given pattern, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override abstract ConstantPattern<V, T> Evaluated();

        #endregion

        #region Public methods

        public override string ToString()
        {
            return "const" + Convert.ToString(Identifier);
        }

        /// <summary>
        /// Determines wheter the given <see cref="object"/> matches the pattern.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to match against the pattern.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the <see cref="object"/> matches the pattern.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Matches(object? obj)
        {
            return Matches(obj, typeof(Constant<,>));
        }

        #endregion
    }
}
