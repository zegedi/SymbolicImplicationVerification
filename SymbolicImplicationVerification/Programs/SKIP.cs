using System;

namespace SymbolicImplicationVerification.Programs
{
    public class SKIP : Program
    {
        #region Fields

        /// <summary>
        /// The singular instance of the SKIP class.
        /// </summary>
        private static SKIP? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private SKIP() : base() { }

        #endregion

        #region Public methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular SKIP instance.</returns>
        public static SKIP Instance()
        {
            if (instance is null)
            {
                instance = new SKIP();
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
    }
}
