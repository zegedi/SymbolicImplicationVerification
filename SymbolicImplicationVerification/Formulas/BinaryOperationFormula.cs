using SymbolicImplicationVerification.Terms;

namespace SymbolicImplicationVerification.Formulas
{
    public abstract class BinaryOperationFormula : Formula
    {
        #region Fields

        protected Formula leftOperand;

        protected Formula rightOperand;

        #endregion

        #region Constructors

        public BinaryOperationFormula(Formula leftOperand, Formula rightOperand) : this(null, leftOperand, rightOperand) { }

        public BinaryOperationFormula(string? identifier, Formula leftOperand, Formula rightOperand) : base(identifier)
        {
            this.leftOperand  = leftOperand;
            this.rightOperand = rightOperand;
        }

        #endregion

        #region Public properties

        public Formula LeftOperand
        {
            get { return leftOperand; }
            set { leftOperand = value; }
        }

        public Formula RightOperand
        {
            get { return rightOperand; }
            set { rightOperand = value; }
        }

        #endregion
    }
}
