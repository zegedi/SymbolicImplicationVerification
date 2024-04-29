using SymbolicImplicationVerification.Converts.Tokens.Operands;
using SymbolicImplicationVerification.Terms.FunctionValues;

namespace SymbolicImplicationVerification.Converts.Tokens.Functions
{
    internal class ChiFunctionCallStart : FunctionCallStart
    {
        #region Fields

        /// <summary>
        /// The singular instance of the ChiFunctionCallStart class.
        /// </summary>
        private static ChiFunctionCallStart? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private ChiFunctionCallStart() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular ChiFunctionCallStart instance.</returns>
        public static ChiFunctionCallStart Instance()
        {
            if (instance is null)
            {
                instance = new ChiFunctionCallStart();
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
            return Evaluated(operand, argument => new ChiFunction(argument.DeepCopy()));
        }

        #endregion
    }
}
