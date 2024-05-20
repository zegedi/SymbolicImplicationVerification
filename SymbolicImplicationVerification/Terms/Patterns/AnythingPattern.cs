
using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Patterns
{
    public class AnythingPattern<T> : Pattern<T> where T : Type
    {
        #region Constructors

        public AnythingPattern(AnythingPattern<T> anything) : base(anything.identifier, (T) anything.termType.DeepCopy()) { }

        public AnythingPattern(int identifier, T termType) : base(identifier, termType) { }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Create a deep copy of the current anything pattern.
        /// </summary>
        /// <returns>The created deep copy of the anything pattern.</returns>
        public override AnythingPattern<T> DeepCopy()
        {
            return new AnythingPattern<T>(this);
        }

        /// <summary>
        /// Evaluated the given pattern, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override AnythingPattern<T> Evaluated()
        {
            return DeepCopy();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
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
