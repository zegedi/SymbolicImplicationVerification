using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Constants
{
    public class PositiveIntegerConstant : IntegerTypeConstant
    {
        #region Constructors

        public PositiveIntegerConstant(int value) 
            : base(value, PositiveInteger.Instance(), PositiveInteger.IsValueOutOfRange(value)) { }

        public PositiveIntegerConstant(PositiveIntegerConstant constant) 
            : base(constant.value, PositiveInteger.Instance()) { }

        #endregion
    }
}
