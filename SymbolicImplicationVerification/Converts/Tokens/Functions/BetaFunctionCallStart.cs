using SymbolicImplicationVerification.Converts.Tokens.Operands;
using SymbolicImplicationVerification.Terms.FunctionValues;

namespace SymbolicImplicationVerification.Converts.Tokens.Functions
{
    internal class BetaFunctionCallStart : FunctionCallStart
    {
        #region Fields

        /// <summary>
        /// The singular instance of the BetaFunctionCallStart class.
        /// </summary>
        private static BetaFunctionCallStart? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private BetaFunctionCallStart() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular BetaFunctionCallStart instance.</returns>
        public static BetaFunctionCallStart Instance()
        {
            if (instance is null)
            {
                instance = new BetaFunctionCallStart();
            }

            return instance;
        }

        /// <summary>
        /// Destroy method for the singular instance.
        /// </summary>
        public static void Destroy()
        {
            if (instance is not null)
            {
                instance = null;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates an evaluated token from the operand.
        /// </summary>
        /// <param name="operand">The operand of the unary operation.</param>
        /// <returns>The token that represents the evaluation.</returns>
        public override TermOperand Evaluated(Operand operand)
        {
            return Evaluated(operand, argument => new BetaFunction(argument.DeepCopy()));
        }

        #endregion
    }
}
