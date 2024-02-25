global using IntegerTypeBinaryOperationTerm =
    SymbolicImplicationVerification.Term.Operation.BinaryOperationTerm<
        SymbolicImplicationVerification.Term.Term<SymbolicImplicationVerification.Type.IntegerType>,
        SymbolicImplicationVerification.Type.IntegerType,
        SymbolicImplicationVerification.Term.Term<SymbolicImplicationVerification.Type.IntegerType>,
        SymbolicImplicationVerification.Type.IntegerType,
        SymbolicImplicationVerification.Type.IntegerType>;

using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term.Operation
{
    public abstract class BinaryOperationTerm<LTerm, LType, RTerm, RType, T> : Term<T> 
        where LTerm : Term<LType>
        where LType : Type.Type
        where RTerm : Term<RType>
        where RType : Type.Type
        where T : Type.Type
    {
        #region Fields

        protected LTerm leftOperand;
        protected RTerm rightOperand;

        #endregion

        #region Constructors

        public BinaryOperationTerm(LTerm leftOperand, RTerm rightOperand, T termType) : base(termType)
        {
            this.leftOperand  = leftOperand;
            this.rightOperand = rightOperand;
        }

        #endregion

        #region Public properties

        public LTerm LeftOperand
        {
            get { return leftOperand; }
            private set { leftOperand = value; }
        }

        public RTerm RightOperand
        {
            get { return rightOperand; }
            private set { rightOperand = value; }
        }

        #endregion
    }
}
