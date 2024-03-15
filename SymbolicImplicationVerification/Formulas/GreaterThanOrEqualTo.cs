using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas
{
    public class GreaterThanOrEqualTo<T> : BinaryRelationFormula<T> where T : IntegerType
    {
        #region Constructors

        public GreaterThanOrEqualTo(Term<T> leftComponent, Term<T> rightComponent)
            : this(null, leftComponent, rightComponent) { }

        public GreaterThanOrEqualTo(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion
    }
}
