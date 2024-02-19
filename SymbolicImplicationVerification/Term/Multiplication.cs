using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term
{
    public class Multiplication<T> : BinaryOperationTerm<T> where T : IntegerType
    {
        #region Constructors

        public Multiplication(Term<T> leftOperand, Term<T> rightOperand, T termType)
            : base(leftOperand, rightOperand, termType) { }

        #endregion
    }
}
