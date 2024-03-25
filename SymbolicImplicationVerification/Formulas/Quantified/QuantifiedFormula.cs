using SymbolicImplicationVerification.Terms;
using SymbolicImplicationVerification.Terms.Variables;

namespace SymbolicImplicationVerification.Formulas.Quantified
{
    public abstract class QuantifiedFormula<T> : Formula where T : Type
    {
        #region Fields

        protected Variable<T> quantifiedVariable;

        protected Formula statement;

        #endregion

        #region Constructors

        public QuantifiedFormula(Variable<T> quantifiedVariable, Formula statement) : this(null, quantifiedVariable, statement) { }

        public QuantifiedFormula(string? identifier, Variable<T> quantifiedVariable, Formula statement) : base(identifier)
        {
            this.quantifiedVariable = quantifiedVariable;
            this.statement = statement;
        }

        #endregion

        #region Public properties

        public Variable<T> QuantifiedVariable
        {
            get { return quantifiedVariable; }
            set { quantifiedVariable = value; }
        }

        public Formula Statement
        {
            get { return statement; }
            set { statement = value; }
        }

        #endregion

        #region Public abstract methods

        /// <summary>
        /// Creates a deep copy of the current quantified formula.
        /// </summary>
        /// <returns>The created deep copy of the quantified formula.</returns>
        public override abstract QuantifiedFormula<T> DeepCopy();

        #endregion
    }
}
