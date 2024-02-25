using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term.Constant
{
    public class IntegerConstant : Constant<int, Integer>
    {
        #region Constructors

        public IntegerConstant(int value) : base(value, Integer.Instance()) { }


        public IntegerConstant(IntegerConstant constant) : base(constant.value, Integer.Instance()) { }

        #endregion
    }
}
