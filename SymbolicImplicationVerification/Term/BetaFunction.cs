using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term
{
    public class BetaFunction : FunctionValue<Integer, Logical>
    {
        #region Constructors

        private BetaFunction(Term<Integer> argument) : base(argument, Logical.Instance()) { }

        #endregion
    }
}
