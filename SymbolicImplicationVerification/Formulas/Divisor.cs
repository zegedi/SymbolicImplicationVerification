using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Formulas
{
    public class Divisor<T> : BinaryRelationFormula<T> where T : IntegerType
    {
        #region Constructors

        public Divisor(Term<T> leftComponent, Term<T> rightComponent)
            : this(null, leftComponent, rightComponent) { }

        public Divisor(string? identifier, Term<T> leftComponent, Term<T> rightComponent)
            : base(identifier, leftComponent, rightComponent) { }

        #endregion
    }
}
