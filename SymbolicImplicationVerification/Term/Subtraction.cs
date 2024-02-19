using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term
{
    public class Subtraction<T> : Term<T> where T : IntegerType
    {
        #region Constructors

        public Subtraction(Term<T> leftOperand, Term<T> rightOperand, T termType)
            : base(leftOperand, rightOperand, termType) { }

        #endregion
    }
}
