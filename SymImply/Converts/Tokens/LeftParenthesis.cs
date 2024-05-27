
namespace SymImply.Converts.Tokens
{
    internal class LeftParenthesis : Token
    {
        #region Fields

        /// <summary>
        /// The singular instance of the LeftParenthesis class.
        /// </summary>
        private static LeftParenthesis? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private LeftParenthesis() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular LeftParenthesis instance.</returns>
        public static LeftParenthesis Instance()
        {
            if (instance is null)
            {
                instance = new LeftParenthesis();
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
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return "(";
        }

        #endregion
    }
}
