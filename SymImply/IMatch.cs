using SymImply.Terms.Constants;
using SymImply.Terms;
using System;
using SymImply.Types;

namespace SymImply
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
