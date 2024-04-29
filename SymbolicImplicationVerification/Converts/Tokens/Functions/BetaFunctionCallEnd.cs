namespace SymbolicImplicationVerification.Converts.Tokens.Functions
{
    internal class BetaFunctionCallEnd : FunctionCallEnd
    {
        #region Fields

        /// <summary>
        /// The singular instance of the BetaFunctionCallEnd class.
        /// </summary>
        private static BetaFunctionCallEnd? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private BetaFunctionCallEnd() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular BetaFunctionCallEnd instance.</returns>
        public static BetaFunctionCallEnd Instance()
        {
            if (instance is null)
            {
                instance = new BetaFunctionCallEnd();
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
        /// Determines whether the current function call end closes the open function call.
        /// </summary>
        /// <param name="start">The given start function call.</param>
        /// <returns>
        ///   <list type="bullet">
        ///     <item><see langword="true"/> - if the end closes the open function call.</item>
        ///     <item><see langword="false"/> - otherwise.</item>
        ///   </list>
        /// </returns>
        public override bool Closes(FunctionCallStart start)
        {
            return start is BetaFunctionCallStart;
        }

        #endregion
    }
}
