using SymbolicImplicationVerification.Term;
using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Formula
{
    public class LessThanOrEqualTo<T> : BinaryRelationFormula<T> where T : IntegerType
    {
        #region Constructors

        public LessThanOrEqualTo(Term<T> leftComponent, Term<T> rightComponent)
            : this(null, leftComponent, rightComponent) { }

        public LessThanOrEqualTo(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion
    }
}
