using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicImplicationVerification.Converts.Tokens
{
    internal class RightParenthesis : Token
    {
        #region Fields

        /// <summary>
        /// The singular instance of the RightParenthesis class.
        /// </summary>
        private static RightParenthesis? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private RightParenthesis() { }

        #endregion

        #region Public static methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular RightParenthesis instance.</returns>
        public static RightParenthesis Instance()
        {
            if (instance is null)
            {
                instance = new RightParenthesis();
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
            return ")";
        }

        #endregion
    }
}
