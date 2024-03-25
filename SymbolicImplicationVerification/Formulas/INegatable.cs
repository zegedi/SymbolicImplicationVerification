using System;

namespace SymbolicImplicationVerification.Formulas
{
    public interface INegatable<T>
    {
        /// <summary>
        /// Negates the given expression, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public T Negated();
    }
}
