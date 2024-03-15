using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicImplicationVerification.Terms.Patterns
{
    public class VariablePattern<T> : Pattern<T> where T : Type
    {
        #region Constructors

        protected VariablePattern(int identifier, T termType) : base(identifier,termType) { }

        #endregion

        #region Public abstract methods

        public override string ToString()
        {
            return "var" + Convert.ToString(Identifier);
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
            return Matches(obj, typeof(Variable<>));
        }

        #endregion
    }
}
