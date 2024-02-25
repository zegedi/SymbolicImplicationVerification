using SymbolicImplicationVerification.Type;
using System;

namespace SymbolicImplicationVerification.Term.FunctionValue
{
    public class ChiFunction : FunctionValue<Logical, ZeroOrOne>
    {
        #region Constructors

        private ChiFunction(Term<Logical> argument) : base(argument, ZeroOrOne.Instance()) { }

        #endregion
    }
}
