using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term.Constant
{
    public class NaturalNumberConstant : Constant<int, NaturalNumber>
    {
        #region Constructors

        public NaturalNumberConstant(int value) : base(value, NaturalNumber.Instance(), NaturalNumber.IsValueOutOfRange(value)) { }

        public NaturalNumberConstant(NaturalNumberConstant constant) : base(constant.value, NaturalNumber.Instance()) { }

        #endregion
    }
}
