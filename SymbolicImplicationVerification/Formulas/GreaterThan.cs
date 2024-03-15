using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas
{
    public class GreaterThan<T> : BinaryRelationFormula<T> where T : IntegerType
    {
        #region Constructors

        public GreaterThan(Term<T> leftComponent, Term<T> rightComponent)
            : this(null, leftComponent, rightComponent) { }

        public GreaterThan(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion
    }
}
