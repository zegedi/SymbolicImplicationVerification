using SymbolicImplicationVerification.Terms.Operations.Binary;
using SymbolicImplicationVerification.Terms.Operations;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.Constants
{
    public class IntegerConstant : IntegerTypeConstant //, IArithmetic<IntegerConstant, IntegerTypeTerm>
    {
        #region Constructors

        public IntegerConstant(int value) : base(value, Integer.Instance()) { }


        public IntegerConstant(IntegerConstant constant) : base(constant.value, Integer.Instance()) { }

        #endregion

        #region Implicit conversions

        public static implicit operator IntegerConstant(int value) => new IntegerConstant(value);

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current integer constant.
        /// </summary>
        /// <returns>The created deep copy of the integer constant.</returns>
        public override IntegerConstant DeepCopy()
        {
            return new IntegerConstant(this);
        }

        #endregion
    }
}
