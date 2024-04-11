using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Constants
{
    public class NaturalNumberConstant : IntegerTypeConstant
    {
        #region Constructors

        public NaturalNumberConstant(int value) 
            : base(value, NaturalNumber.Instance(), NaturalNumber.Instance().IsValueOutOfRange(value)) { }

        public NaturalNumberConstant(NaturalNumberConstant constant) 
            : base(constant.value, NaturalNumber.Instance()) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current natural number constant.
        /// </summary>
        /// <returns>The created deep copy of the natural number constant.</returns>
        public override NaturalNumberConstant DeepCopy()
        {
            return new NaturalNumberConstant(this);
        }

        /// <summary>
        /// Evaluated the given constant, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override NaturalNumberConstant Evaluated()
        {
            return new NaturalNumberConstant(this);
        }

        #endregion
    }
}
