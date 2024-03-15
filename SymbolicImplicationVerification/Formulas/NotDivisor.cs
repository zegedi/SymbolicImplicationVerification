using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas
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
