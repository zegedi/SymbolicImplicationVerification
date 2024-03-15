using SymbolicImplicationVerification.Terms;

namespace SymbolicImplicationVerification.Formulas
{
    public class NotEqual<T> : BinaryRelationFormula<T> where T : Types.Type
    {
        #region Constructors

        public NotEqual(Term<T> leftComponent, Term<T> rightComponent)
            : this(null, leftComponent, rightComponent) { }

        public NotEqual(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion
    }
}
