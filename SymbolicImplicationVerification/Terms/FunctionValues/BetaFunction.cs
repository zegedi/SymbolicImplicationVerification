using SymbolicImplicationVerification.Terms.Constants;
using SymbolicImplicationVerification.Types;

namespace SymbolicImplicationVerification.Terms.FunctionValues
{
    public class BetaFunction : FunctionValue<IntegerType, Logical>
    {
        #region Constructors

        public BetaFunction(IntegerTypeTerm argument) : base(argument, Logical.Instance()) { }

        public BetaFunction(BetaFunction betaFunctionValue)
            : base(betaFunctionValue.argument.DeepCopy(), Logical.Instance()) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return @$"\betafunc{{{argument}}}";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   <see langword="true"/> if the specified object is equal to the current object; 
        ///   otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is BetaFunction other && argument.Equals(other.argument);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Create a deep copy of the current beta function.
        /// </summary>
        /// <returns>The created deep copy of the beta function.</returns>
        public override BetaFunction DeepCopy()
        {
            return new BetaFunction(this);
        }

        /// <summary>
        /// Evaluated the given term, without modifying the original.
        /// </summary>
        /// <returns>The newly created instance of the result.</returns>
        public override LogicalTerm Evaluated()
        {
            return new BetaFunction(argument.Evaluated());
        }

        #endregion
    }
}
