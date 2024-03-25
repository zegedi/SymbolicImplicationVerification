using SymbolicImplicationVerification.Types;
using System;

namespace SymbolicImplicationVerification.Terms.FunctionValues
{
    public class ChiFunction : FunctionValue<Logical, IntegerType>
    {
        #region Constructors

        public ChiFunction(LogicalTerm argument) : base(argument, ZeroOrOne.Instance()) { }

        public ChiFunction(ChiFunction chiFunction) : this(chiFunction.argument.DeepCopy()) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Create a deep copy of the current chi function.
        /// </summary>
        /// <returns>The created deep copy of the chi function.</returns>
        public override ChiFunction DeepCopy()
        {
            return new ChiFunction(this);
        }

        #endregion
    }
}
