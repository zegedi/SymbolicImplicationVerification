using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Constants
{
    public class NaturalNumberConstant : IntegerTypeConstant
    {
        #region Constructors

        public NaturalNumberConstant(int value) 
            : base(value, NaturalNumber.Instance(), NaturalNumber.IsValueOutOfRange(value)) { }

        public NaturalNumberConstant(NaturalNumberConstant constant) 
            : base(constant.value, NaturalNumber.Instance()) { }

        #endregion
    }
}
