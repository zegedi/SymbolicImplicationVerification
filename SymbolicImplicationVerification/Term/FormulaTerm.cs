using SymbolicImplicationVerification.Type;
using SymbolicImplicationVerification.Formula;

namespace SymbolicImplicationVerification.Term
{
    public class FormulaTerm : Term<Logical>
    {
        #region Fields

        private Formula.Formula formula;

        #endregion

        #region Constructors

        public FormulaTerm(Formula.Formula formula) : base(Logical.Instance())
        {
            this.formula = formula;
        }

        #endregion

        #region Public properties

        public Formula.Formula Formula
        { 
            get { return formula; }
            set { formula = value; }
        }

        #endregion
    }
}
