using SymbolicImplicationVerification.Types;
using SymbolicImplicationVerification.Formulas;
using SymbolicImplicationVerification.Terms.Constants;

namespace SymbolicImplicationVerification.Terms
{
    public class FormulaTerm : LogicalTerm
    {
        #region Fields

        private Formula formula;

        #endregion

        #region Constructors

        public FormulaTerm(FormulaTerm formulaTerm) : this(formulaTerm.formula.DeepCopy()) { }

        public FormulaTerm(Formula formula) : base(Logical.Instance())
        {
            this.formula = formula;
        }

        #endregion

        #region Public properties

        public Formula Formula
        { 
            get { return formula; }
            set { formula = value; }
        }

        #endregion

        #region Public methods

        public override string Hash(HashLevel level)
        {
            return String.Empty;
        }

        /// <summary>
        /// Creates a deep copy of the current formula term.
        /// </summary>
        /// <returns>The created deep copy of the formula term.</returns>
        public override FormulaTerm DeepCopy()
        {
            return new FormulaTerm(this);
        }

        public LogicalTerm Evaluated()
        {
            Formula formulaEvaluated = formula.Evaluated();

            return formulaEvaluated switch
            {
                TRUE  => new LogicalConstant(true),
                FALSE => new LogicalConstant(false),
                _     => new FormulaTerm(formulaEvaluated)
            };
        }

        #endregion
    }
}
