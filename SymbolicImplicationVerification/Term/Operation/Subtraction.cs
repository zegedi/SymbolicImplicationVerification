using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term.Operation
{
    public class Subtraction<T> : BinaryOperationTerm<T> where T : IntegerType
    {
        #region Constructors

        public Subtraction(Term<T> leftOperand, Term<T> rightOperand, T termType)
            : base(leftOperand, rightOperand, termType) { }

        #endregion
    }
}
