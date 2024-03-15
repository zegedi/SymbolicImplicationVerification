﻿
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Patterns
{
    public class AnythingPattern<T> : Pattern<T> where T : Type
    {
        #region Constructors

        public AnythingPattern(int identifier, T termType) : base(identifier, termType) { }

        #endregion

        #region Public methods

        public override string ToString()
        {
            return "any" + Convert.ToString(Identifier);
        }

        /// <summary>
        /// Determines wheter the given <see cref="object"/> matches the pattern.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to match against the pattern.</param>
        /// <returns>
        ///   <list runtimeType="bullet">
        ///     <item><see langword="true"/> - if the <see cref="object"/> matches the pattern.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Matches(object? obj)
        {
            return Matches(obj, typeof(Term<>));
        }

        #endregion
    }
}
