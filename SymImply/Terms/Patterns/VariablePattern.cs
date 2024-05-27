using SymImply.Terms.Constants;
using SymImply.Types;
using SymImply.Terms.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SymImply.Terms.Operations.Linear;

namespace SymImply.Terms.Patterns
{
    public abstract class VariablePattern<T> : Pattern<T> where T : Type
    {
        #region Constructors

        public VariablePattern(int identifier, T termType) : base(identifier, termType) { }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Create a deep copy of the current variable pattern.
        /// </summary>
        /// <returns>The created deep copy of the variable pattern.</returns>
        public override abstract VariablePattern<T> DeepCopy();

        /// <summary>
        /// Evaluated the given pattern, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override abstract VariablePattern<T> Evaluated();

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("variablePattern_{0}", identifier);
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
