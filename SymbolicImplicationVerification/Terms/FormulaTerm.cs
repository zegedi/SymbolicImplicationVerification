using SymbolicImplicationVerification.Types;
using SymbolicImplicationVerification.Formulas;

namespace SymbolicImplicationVerification.Terms
{
    public class FormulaTerm : Term<Logical>
    {
        #region Fields

        private Formula formula;

        #endregion

        #region Constructors

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

        #endregion
    }
}
