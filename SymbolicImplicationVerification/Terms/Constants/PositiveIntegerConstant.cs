using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Constants
{
    public class PositiveIntegerConstant : IntegerTypeConstant
    {
        #region Constructors

        public PositiveIntegerConstant(int value) 
            : base(value, PositiveInteger.Instance(), PositiveInteger.Instance().IsValueOutOfRange(value)) { }

        public PositiveIntegerConstant(PositiveIntegerConstant constant) 
            : base(constant.value, PositiveInteger.Instance()) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current positive integer constant.
        /// </summary>
        /// <returns>The created deep copy of the positive integer constant.</returns>
        public override PositiveIntegerConstant DeepCopy()
        {
            return new PositiveIntegerConstant(this);
        }

        #endregion
    }
}
