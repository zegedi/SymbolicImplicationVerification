using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.FunctionValues
{
    public class BetaFunction : FunctionValue<Integer, Logical>
    {
        #region Constructors

        public BetaFunction(Term<Integer> argument) : base(argument, Logical.Instance()) { }

        //public BetaFunction(BetaFunction betaFunctionValue) 
        //    : base(DeepCopy((dynamic) betaFunctionValue.argument), Logical.Instance()) { }

        #endregion
    }
}
