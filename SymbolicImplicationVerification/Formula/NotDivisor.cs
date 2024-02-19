using SymbolicImplicationVerification.Term;
using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Formula
{
    public class NotDivisor<T> : BinaryRelationFormula<T> where T : IntegerType
    {
        #region Constructors

        public NotDivisor(Term<T> leftComponent, Term<T> rightComponent)
            : this(null, leftComponent, rightComponent) { }

        public NotDivisor(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion
    }
}
