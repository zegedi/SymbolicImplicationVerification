using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas
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
