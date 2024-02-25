using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term.Operation
{
    public class Multiplication : BinaryOperationTerm<IntegerType, IntegerType, IntegerType>
    {
        #region Constructors

        public Multiplication(Term<IntegerType> leftOperand, Term<IntegerType> rightOperand)
            
            : base(leftOperand, rightOperand, IntegerTypeConversions.Select(leftOperand.TermType, rightOperand.TermType)) { }

        #endregion

        #region Private methods



        #endregion

        #region Private static methods

        private static IntegerType SelectType()

        #endregion
    }
}
