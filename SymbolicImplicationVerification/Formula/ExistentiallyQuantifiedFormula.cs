using SymbolicImplicationVerification.Term;

namespace SymbolicImplicationVerification.Formula
{
    public class ExistentiallyQuantifiedFormula<T> : QuantifiedFormula<T> where T : Type.Type
    {
        #region Constructors

        public ExistentiallyQuantifiedFormula(Variable<T> quantifiedVariable, Formula statement) : 
            this(null, quantifiedVariable, statement) { }

        public ExistentiallyQuantifiedFormula(string? identifier, Variable<T> quantifiedVariable, Formula statement)
            : base(identifier, quantifiedVariable, statement) { }

        #endregion
    }
}
