using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term.Constant
{
    public class LogicalConstant : Constant<bool, Logical>
    {
        #region Constructors

        public LogicalConstant(bool value) : base(value, Logical.Instance()) { }

        public LogicalConstant(LogicalConstant constant) : base(constant.value, Logical.Instance()) { }

        #endregion
    }
}
