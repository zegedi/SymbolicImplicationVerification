using SymbolicImplicationVerification.Term;

namespace SymbolicImplicationVerification.Formula
{
    public class Equal<T> : BinaryRelationFormula<T> where T : Type.Type
    {
        #region Constructors

        public Equal(Term<T> leftComponent, Term<T> rightComponent)
            : this(null, leftComponent, rightComponent) { }

        public Equal(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion
    }
}
