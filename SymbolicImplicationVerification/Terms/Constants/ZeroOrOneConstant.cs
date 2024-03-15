using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Constants
{
    public class ZeroOrOneConstant : IntegerTypeConstant
    {
        #region Constructors

        public ZeroOrOneConstant(int value)
            : base(value, ZeroOrOne.Instance(), ZeroOrOne.IsValueOutOfRange(value)) { }

        public ZeroOrOneConstant(ZeroOrOneConstant constant)
            : base(constant.value, ZeroOrOne.Instance()) { }

        #endregion
    }
}
