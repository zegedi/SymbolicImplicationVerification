using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Terms;
using System;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification
{
    public interface IMatch
    {
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
        public abstract bool Matches(object? obj);
    }
}
