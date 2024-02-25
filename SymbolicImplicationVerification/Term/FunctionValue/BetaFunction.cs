using SymbolicImplicationVerification.Type;

namespace SymbolicImplicationVerification.Term.FunctionValue
{
    public class BetaFunction : FunctionValue<Integer, Logical>
    {
        #region Constructors

        private BetaFunction(IntegerTerm argument) : base(argument, Logical.Instance()) { }

        #endregion
    }
}
