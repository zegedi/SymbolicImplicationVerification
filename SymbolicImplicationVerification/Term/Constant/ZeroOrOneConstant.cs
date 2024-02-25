using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term.Constant
{
    public class ZeroOrOneConstant : Constant<int, ZeroOrOne>
    {
        #region Constructors

        public ZeroOrOneConstant(int value) : base(value, ZeroOrOne.Instance(), ZeroOrOne.IsValueOutOfRange(value)) { }

        public ZeroOrOneConstant(ZeroOrOneConstant constant) : base(constant.value, ZeroOrOne.Instance()) { }

        #endregion
    }
}
