using SymbolicImplicationVerification.Types;
using System;

namespace SymbolicImplicationVerification.Terms.FunctionValues
{
    public class ChiFunction : FunctionValue<Logical, IntegerType>
    {
        #region Constructors

        public ChiFunction(Term<Logical> argument) : base(argument, ZeroOrOne.Instance()) { }

        #endregion
    }
}
