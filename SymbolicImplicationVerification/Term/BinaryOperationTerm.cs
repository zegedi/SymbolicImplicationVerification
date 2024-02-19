
namespace SymbolicImplicationVerification.Term
{
    public abstract class BinaryOperationTerm<T> : Term<T> where T : Type.Type
    {
        #region Fields

        protected Term<T> leftOperand;
        protected Term<T> rightOperand;

        #endregion

        #region Constructors

        public BinaryOperationTerm(Term<T> leftOperand, Term<T> rightOperand, T termType) : base(termType)
        {
            this.leftOperand  = leftOperand;
            this.rightOperand = rightOperand;
        }

        #endregion

        #region Public properties

        public Term<T> LeftOperand
        {
            get { return leftOperand; }
            private set { leftOperand = value; }
        }

        public Term<T> RightOperand
        {
            get { return rightOperand; }
            private set { rightOperand = value; }
        }

        #endregion
    }
}
