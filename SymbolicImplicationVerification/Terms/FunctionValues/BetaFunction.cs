using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.FunctionValues
{
    public class BetaFunction : FunctionValue<Integer, Logical>
    {
        #region Constructors

        public BetaFunction(Term<Integer> argument) : base(argument, Logical.Instance()) { }

        public BetaFunction(BetaFunction betaFunctionValue)
            : base(betaFunctionValue.argument.DeepCopy(), Logical.Instance()) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current beta function.
        /// </summary>
        /// <returns>The created deep copy of the beta function.</returns>
        public override BetaFunction DeepCopy()
        {
            return new BetaFunction(this);
        }

        #endregion
    }
}
