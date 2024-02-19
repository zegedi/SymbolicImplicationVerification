using SymbolicImplicationVerification.Term;
using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Formula
{
    public class LessThan<T> : BinaryRelationFormula<T> where T : IntegerType
    {
        #region Constructors

        public LessThan(Term<T> leftComponent, Term<T> rightComponent)
            : this(null, leftComponent, rightComponent) { }

        public LessThan(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion
    }
}
