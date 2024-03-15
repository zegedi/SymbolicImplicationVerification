using SymbolicImplicationVerification.Terms;

namespace SymbolicImplicationVerification.Formulas
{
    public class Equal<T> : BinaryRelationFormula<T> where T : Types.Type
    {
        #region Constructors

        public Equal(Term<T> leftComponent, Term<T> rightComponent)
            : this(null, leftComponent, rightComponent) { }

        public Equal(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion
    }
}
