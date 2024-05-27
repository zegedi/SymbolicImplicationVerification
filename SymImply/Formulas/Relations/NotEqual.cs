using SymImply.Formulas.Relations;
using SymImply.Terms;
using SymImply.Terms.Constants;

namespace SymImply.Formulas
{
    public abstract class NotEqual<T> : BinaryRelationFormula<T> where T : Type
    {
        #region Constructors

        public NotEqual(NotEqual<T> equal) : base(
            equal.identifier,
            equal.leftComponent.DeepCopy(),
            equal.rightComponent.DeepCopy()) { }

        public NotEqual(Term<T> leftComponent, Term<T> rightComponent)
            : base(null, leftComponent, rightComponent) { }

        public NotEqual(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a LaTeX code that represents the current object.
        /// </summary>
        /// <returns>A string of LaTeX code that represents the current object.</returns>
        public override string ToLatex()
        {
            return string.Format("{0} \\neq {1}", leftComponent, rightComponent);
        }

        #endregion
    }
}
