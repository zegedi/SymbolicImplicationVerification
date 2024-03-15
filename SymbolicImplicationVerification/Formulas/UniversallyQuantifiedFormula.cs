using SymbolicImplicationVerification.Terms;

namespace SymbolicImplicationVerification.Formulas
{
    public class UniversallyQuantifiedFormula<T> : QuantifiedFormula<T> where T : Types.Type
    {
        #region Constructors

        public UniversallyQuantifiedFormula(Variable<T> quantifiedVariable, Formula statement) :
            this(null, quantifiedVariable, statement)
        { }

        public UniversallyQuantifiedFormula(string? identifier, Variable<T> quantifiedVariable, Formula statement)
            : base(identifier, quantifiedVariable, statement) { }

        #endregion
    }
}
