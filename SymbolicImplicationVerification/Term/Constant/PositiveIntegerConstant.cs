using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term.Constant
{
    public class PositiveIntegerConstant : Constant<int, PositiveInteger>
    {
        #region Constructors

        public PositiveIntegerConstant(int value) : base(value, PositiveInteger.Instance(), PositiveInteger.IsValueOutOfRange(value)) { }

        public PositiveIntegerConstant(PositiveIntegerConstant constant) : base(constant.value, PositiveInteger.Instance()) { }

        #endregion
    }
}
