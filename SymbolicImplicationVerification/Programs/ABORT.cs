using System;

namespace SymbolicImplicationVerification.Programs
{
    public class ABORT : Program
    {
        #region Fields

        /// <summary>
        /// The singular instance of the ABORT class.
        /// </summary>
        private static ABORT? instance;

        #endregion

        #region Constructors

        /// <summary>
        /// Private empty constructor.
        /// </summary>
        private ABORT() : base() { }

        #endregion

        #region Public methods

        /// <summary>
        /// Factory method for the singular instance.
        /// </summary>
        /// <returns>The singular ABORT instance.</returns>
        public static ABORT Instance()
        {
            if (instance is null)
            {
                instance = new ABORT();
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
