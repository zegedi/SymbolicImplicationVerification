using SymbolicImplicationVerification.Terms;

namespace SymbolicImplicationVerification.Formulas
{
    public class ExistentiallyQuantifiedFormula<T> : QuantifiedFormula<T> where T : Types.Type
    {
        #region Constructors

        public ExistentiallyQuantifiedFormula(Variable<T> quantifiedVariable, Formula statement) : 
            this(null, quantifiedVariable, statement) { }

        public ExistentiallyQuantifiedFormula(string? identifier, Variable<T> quantifiedVariable, Formula statement)
            : base(identifier, quantifiedVariable, statement) { }

        #endregion
    }
}
