using SymbolicImplicationVerification.Type;
using System;

namespace SymbolicImplicationVerification.Term
{
    public class ChiFunction : FunctionValue<Logical, ZeroOrOne>
    {
        #region Constructors

        private ChiFunction(Term<Logical> argument) : base(argument, ZeroOrOne.Instance()) { }

        #endregion
    }
}
