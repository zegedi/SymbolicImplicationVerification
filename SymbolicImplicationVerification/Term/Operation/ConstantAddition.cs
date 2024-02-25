using SymbolicImplicationVerification.Type;
using SymbolicImplicationVerification.Term.Constant;

namespace SymbolicImplicationVerification.Term.Operation
{
    public class ConstantAddition : Addition<IntegerConstant, Integer, IntegerConstant, Integer, Integer>, IEvaluable<IntegerConstant>
    {
        #region Constructors

        public ConstantAddition(IntegerConstant leftOperand, IntegerConstant rightOperand) 
            : base(leftOperand, rightOperand, Integer.Instance()) { }

        #endregion

        #region Public methods

        public IntegerConstant Evaluate()
        {
            int additionResult = leftOperand.Value + rightOperand.Value;

            return new IntegerConstant(additionResult);
        }

        #endregion
    }
}
