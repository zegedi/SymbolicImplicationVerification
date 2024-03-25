using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Constants
{
    public class ZeroOrOneConstant : IntegerTypeConstant
    {
        #region Constructors

        public ZeroOrOneConstant(int value)
            : base(value, ZeroOrOne.Instance(), ZeroOrOne.Instance().IsValueOutOfRange(value)) { }

        public ZeroOrOneConstant(ZeroOrOneConstant constant)
            : base(constant.value, ZeroOrOne.Instance()) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current zero or one constant.
        /// </summary>
        /// <returns>The created deep copy of the zero or one constant.</returns>
        public override ZeroOrOneConstant DeepCopy()
        {
            return new ZeroOrOneConstant(this);
        }

        #endregion
    }
}
