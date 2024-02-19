using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term
{
    public class Addition<T> : BinaryOperationTerm<T> where T : IntegerType
    {
        #region Constructors

        public Addition(Term<T> leftOperand, Term<T> rightOperand, T termType) 
            : base(leftOperand, rightOperand, termType) { }

        #endregion
    }
}
